using System.Diagnostics.CodeAnalysis;
using System.Text;
using BakedEnv.Interpreter.Instructions;
using BakedEnv.Interpreter.ProcessorStatementHandling;
using BakedEnv.Interpreter.Sources;
using BakedEnv.Interpreter.Variables;
using BakedEnv.Objects;
using Jammo.ParserTools;
using Jammo.ParserTools.Lexing;
using Jammo.ParserTools.Tools;

namespace BakedEnv.Interpreter;

/// <summary>
/// Main interface for interpreting BakedEnv scripts.
/// </summary>
public class BakedInterpreter
{
    private CommonErrorReporter ErrorReporter { get; set; }
    private EnumerableIterator<LexerToken>? Iterator { get; set; }
    private StateMachine<ParserState>? State { get; set; }
    
    /// <summary>
    /// The current environment used during interpretation.
    /// </summary>
    public BakedEnvironment? Environment { get; private set; }
    
    /// <summary>
    /// Externally accessible context of the interpreter. Can be used to edit variables and such at runtime.
    /// </summary>
    public InterpreterContext? Context { get; private set; }
    
    /// <summary>
    /// Processor statement handlers.
    /// </summary>
    public List<IProcessorStatementHandler> ProcessorStatementHandlers { get; }

    /// <summary>
    /// Informative property of whether the interpreter is ready for parsing.
    /// </summary>
    public bool IsReady { get; private set; }
    
    /// <summary>
    /// The current source being interpreted.
    /// </summary>
    public IBakedSource? Source { get; private set; }
    
    /// <summary>
    /// Informative property of whether or not the interpreter's source is locked.
    /// </summary>
    public bool SourceLocked { get; private set; }
    
    /// <summary>
    /// Event invoked via <see cref="ReportError(BakedError)"/>. Retrieves error information during parsing or execution.
    /// </summary>
    public event EventHandler<BakedError>? ErrorReported;

    public BakedInterpreter()
    {
        ErrorReporter = new CommonErrorReporter(this);
        ProcessorStatementHandlers = new List<IProcessorStatementHandler>();
    }

    /// <summary>
    /// Initialize the interpreter and reset necessary values.
    /// </summary>
    /// <exception cref="InvalidOperationException">The source has not been set.</exception>
    public void Init()
    {
        if (Source == null)
            throw new InvalidOperationException(
                $"Cannot initialize from an unset source. Call '{nameof(WithSource)}' first.");
        
        Iterator = new Lexer(Source.EnumerateCharacters()).ToIterator();
        State = new StateMachine<ParserState>(ParserState.Root);
        Context = new InterpreterContext();
        IsReady = true;

        SourceLocked = true;
    }
    
    /// <summary>
    /// Specify the IBakedSource to be parsed.
    /// </summary>
    /// <param name="source">The IBakedSource to be parsed.</param>
    /// <exception cref="InvalidOperationException">The interpreter is locked and disallows manipulation of its source.</exception>
    public BakedInterpreter WithSource(IBakedSource source)
    {
        if (SourceLocked)
            throw new InvalidOperationException("Cannot use a source while the interpreter is locked.");
        
        Source = source;

        return this;
    }

    /// <summary>
    /// Specify the environment the interpreter should run in.
    /// </summary>
    /// <param name="environment"></param>
    /// <exception cref="InvalidOperationException">The interpreter is locked and disallows manipulation of its environment.</exception>
    public BakedInterpreter WithEnvironment(BakedEnvironment environment)
    {
        if (SourceLocked)
            throw new InvalidOperationException("Cannot use a source while the interpreter is locked.");
        
        Environment = environment;

        return this;
    }
    
    /// <summary>
    /// Add a <see cref="DefaultStatementHandler"/> if it doesn't already exist.
    /// </summary>
    public BakedInterpreter WithDefaultStatementHandler()
    {
        WithDefaultStatementHandler(out _);

        return this;
    }

    /// <summary>
    /// Add a <see cref="DefaultStatementHandler"/> if it doesn't already exist.
    /// </summary>
    /// <param name="handler">Output of the created or existing <see cref="DefaultStatementHandler"/>.</param>
    public BakedInterpreter WithDefaultStatementHandler(out DefaultStatementHandler handler)
    {
        var existing = ProcessorStatementHandlers
            .OfType<DefaultStatementHandler>()
            .FirstOrDefault();

        if (existing == null)
        {
            handler = new DefaultStatementHandler();
            
            ProcessorStatementHandlers.Add(handler);
        }
        else
        {
            handler = existing;
        }

        return this;
    }

    /// <summary>
    /// Add an array of <see cref="IProcessorStatementHandler"/> to the interpreter.
    /// </summary>
    /// <param name="handlers">Handlers to add.</param>
    public BakedInterpreter WithStatementHandlers(params IProcessorStatementHandler[] handlers)
    {
        ProcessorStatementHandlers.AddRange(handlers);

        return this;
    }
    
    /// <summary>
    /// Tear down values used for parsing. Resets the state.
    /// </summary>
    public void TearDown()
    {
        Iterator = null;
        State = null;
        Context = null;
        IsReady = false;

        SourceLocked = false;
    }
    
    /// <summary>
    /// Report an error using the current token as a source.
    /// </summary>
    /// <param name="id">Optional error ID.</param>
    /// <param name="message">Error message.</param>
    public void ReportError(string? id, string message)
    {
        ReportError(id, message, Iterator.Current.Span.Start);
    }

    /// <summary>
    /// Report an error with the required arguments to create a <see cref="BakedError"/>.
    /// </summary>
    /// <param name="id">Optional error ID.</param>
    /// <param name="message">Error message.</param>
    /// <param name="sourceIndex">Error index.</param>
    public void ReportError(string? id, string message, int sourceIndex)
    {
        ReportError(new BakedError(id, message, sourceIndex));
    }

    /// <summary>
    /// Report a raw <see cref="BakedError"/>.
    /// </summary>
    /// <param name="error">Error to report.</param>
    public void ReportError(BakedError error)
    {
        AssertReady();
        
        ErrorReported?.Invoke(this, error);
    }

    /// <summary>
    /// Consume the next token(s) and retrieve an instruction.
    /// </summary>
    /// <param name="instruction">Interpreted instruction information.</param>
    /// <returns>Whether an instruction could be parsed. Should only return false upon reaching the
    /// end of an IBakedSource's content.</returns>
    /// <exception cref="InvalidOperationException">The interpreter has not been initialized.</exception>
    public bool TryGetNextInstruction([NotNullWhen(true)] out InterpreterInstruction? instruction)
    {
        AssertReady();

        InvalidInstruction CreateInvalidInstruction() => new(Iterator.Current.Span.Start);
        
        instruction = null;

        if (!Iterator.TryMoveNext(out var first))
        {
            return false;
        }
        
        if (first.Id is LexerTokenId.Whitespace or LexerTokenId.Newline)
            SkipWhitespaceAndNewlines();

        switch (Iterator.Current.Id)
        {
            case LexerTokenId.OpenBracket:
            {
                SkipWhitespaceAndNewlines();
                
                var nameToken = Iterator.Current;

                if (ErrorReporter.TestUnexpectedTokenTypeError(nameToken, 
                        LexerTokenId.Alphabetic, LexerTokenId.AlphaNumeric))
                {
                    instruction = new InvalidInstruction(nameToken.Span.Start);

                    break;
                }

                var name = nameToken.ToString();
                
                SkipWhitespaceAndNewlines();
                
                var colonToken = Iterator.Current;
                
                if (ErrorReporter.TestUnexpectedTokenTypeError(colonToken, LexerTokenId.Colon))
                {
                    instruction = new InvalidInstruction(colonToken.Span.Start);

                    break;
                }
                
                SkipWhitespaceAndNewlines();

                if (!TryParseValue(out var value))
                {
                    instruction = new InvalidInstruction(nameToken.Span.Start);
                    ErrorReporter.ReportInvalidValue(nameToken);
                    
                    break;
                }

                instruction = new ProcessorStatementInstruction(nameToken.Span.Start)
                {
                    Name = name,
                    Value = value
                };
                
                break;
            }
            case LexerTokenId.Alphabetic:
            case LexerTokenId.AlphaNumeric:
            {
                switch (Iterator.Current.ToString())
                {
                    case "method":
                    {
                        break;
                    }
                    case "return":
                    {
                        break;
                    }
                    default:
                    {
                        if (!TryParseIdentifier(out var path))
                        {
                            instruction = CreateInvalidInstruction();

                            break;
                        }

                        SkipWhitespaceAndNewlinesReserved();
                        
                        if (ErrorReporter.TestUnexpectedTokenTypeError(Iterator.Current, LexerTokenId.Equals))
                        {
                            instruction = CreateInvalidInstruction();

                            break;
                        }

                        if (!TryGetVariableReference(path, out var reference))
                        {
                            if (path.Length == 1)
                            {
                                Context.Variables[path.First().ToString()] = new BakedString(string.Empty);

                                reference = new VariableReference(path.Select(c => c.ToString()), this);
                            }
                            else
                            {
                                instruction = CreateInvalidInstruction();

                                break;
                            }
                        }

                        switch (Iterator.Current.Id)
                        {
                            case LexerTokenId.Equals:
                                SkipWhitespaceAndNewlines();
                                
                                if (!TryParseValue(out var value))
                                {
                                    instruction = CreateInvalidInstruction();

                                    break;
                                }

                                instruction = new VariableAssignmentInstruction(Iterator.Current.Span.Start)
                                {
                                    Reference = reference,
                                    Value = value
                                };

                                break;
                            default:
                                break; // ??
                        }

                        break;
                    }
                }
                
                break;
            }
        }
        
        return instruction != null;
    }
    
    /// <summary>
    /// Assert whether the BakedInterpreter has been initialized,
    /// following with an <see cref="InvalidOperationException"/> if it is not.
    /// </summary>
    public void AssertReady()
    {
        if (!IsReady)
            throw new InvalidOperationException(
                $"The interpreter has not been initialized. Try calling '{nameof(Init)}' or '{nameof(WithSource)}' first.");
    }
    
    private bool TryParseValue([NotNullWhen(true)] out BakedObject? value)
    {
        value = null;
        
        switch (Iterator.Current.Id)
        {
            case LexerTokenId.Alphabetic:
            case LexerTokenId.AlphaNumeric:
            {
                if (!TryParseIdentifier(out var path))
                    return false;

                if (TryGetVariableReference(path, out var reference))
                {
                    return reference.TryGetValue(out value);
                }

                return false;
            }
            case LexerTokenId.Numeric:
            {
                value = new BakedInteger(Iterator.Current.ToString());

                return true;
            }
            case LexerTokenId.DoubleQuote:
            {
                var builder = new StringBuilder();
                var escaped = false;
                
                while (Iterator.TryMoveNext(out var next))
                {
                    if (next.Is(LexerTokenId.DoubleQuote) && !escaped)
                    {
                        Iterator.Skip();
                        break;
                    }

                    switch (next.Id, next.ToString(), escaped)
                    {
                        case (LexerTokenId.Alphabetic, "n", true): builder.Append('\n'); break; // new line
                        case (LexerTokenId.Alphabetic, "r", true): builder.Append('\r'); break; // carriage return
                        case (LexerTokenId.Alphabetic, "t", true): builder.Append('\t'); break; // horizontal tab
                        case (LexerTokenId.Alphabetic, "a", true): builder.Append('\a'); break; // bell
                        case (LexerTokenId.Alphabetic, "f", true): builder.Append('\f'); break; // form
                        case (LexerTokenId.Alphabetic, "v", true): builder.Append('\v'); break; // vertical tab
                        case (LexerTokenId.Backslash, "\\", true): builder.Append('\\'); break; // backslash
                        default:
                            if (escaped) // if nothing was escaped, append backslash
                                builder.Append('\\');
                            
                            builder.Append(next);
                            break;
                    }
                    
                    escaped = false;

                    if (next.Is(LexerTokenId.Backslash))
                        escaped = true;
                }

                value = new BakedString(builder.ToString());

                return true;
            }
        }

        return false;
    }
    
    private bool TryGetVariableReference(LexerToken[] path, [NotNullWhen(true)] out VariableReference? reference)
    {
        reference = null;

        if (path.Length == 0)
            return false;

        reference = new VariableReference(path.Select(c => c.ToString()), this);

        return true;
    }

    private bool TryParseIdentifier(out LexerToken[] path)
    {
        path = Array.Empty<LexerToken>();
        
        var pathList = new List<LexerToken> { Iterator.Current };
        
        if (Iterator.TryMoveNext(out var next) && next.Is(LexerTokenId.Period))
        {
            var parseNextIdentifier = true;

            while (Iterator.TryMoveNext(out next))
            {
                SkipWhitespaceAndNewlines();
                
                if (parseNextIdentifier)
                {
                    if (ErrorReporter.TestUnexpectedTokenTypeError(next, 
                            LexerTokenId.Alphabetic, LexerTokenId.AlphaNumeric))
                    {
                        return false;
                    }
                    
                    pathList.Add(next);

                    parseNextIdentifier = false;
                }
                else
                {
                    if (next.Id is not LexerTokenId.Period)
                    {
                        break;
                    }

                    parseNextIdentifier = true;
                }
            }
        }
        
        path = pathList.ToArray();

        return true;
    }
    
    private int SkipWhitespace()
    {
        var stride = 0;

        foreach (var token in Iterator.TakeWhile(t => t.Id is LexerTokenId.Whitespace))
        {
            stride += token.Span.Size;
        }

        return stride;
    }
    
    private int SkipWhitespaceAndNewlines()
    {
        var stride = 0;

        foreach (var token in Iterator.TakeWhile(t => t.Id is LexerTokenId.Whitespace or LexerTokenId.Newline))
        {
            stride += token.Span.Size;
        }

        return stride;
    }
    
    private int SkipWhitespaceAndNewlinesReserved()
    {
        if (Iterator.Current.Id is not LexerTokenId.Whitespace or LexerTokenId.Newline)
            return 0;
        
        var stride = 0;

        foreach (var token in Iterator.TakeWhile(t => t.Id is LexerTokenId.Whitespace or LexerTokenId.Newline))
        {
            stride += token.Span.Size;
        }

        return stride;
    }

    private enum ParserState
    {
        Root,
        ProcessorStatement,
        
        Identifier,
        TableInitialization,
        
        MethodIdentifier,
        MethodParameters,
        MethodBody,
        
        ControlFlowCondition,
        ControlFlowBody
    }
}