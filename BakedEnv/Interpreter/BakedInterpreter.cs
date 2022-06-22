using System.Diagnostics.CodeAnalysis;
using System.Text;
using BakedEnv.Interpreter.Instructions;
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
    private IBakedScope? CurrentScope { get; set; }
    
    /// <summary>
    /// The current environment used during interpretation.
    /// </summary>
    public BakedEnvironment? Environment { get; private set; }
    
    /// <summary>
    /// Externally accessible context of the interpreter. Can be used to edit variables and such at runtime.
    /// </summary>
    public InterpreterContext? Context { get; private set; }

    /// <summary>
    /// Informative property of whether the interpreter is ready for parsing.
    /// </summary>
    [MemberNotNullWhen(true, nameof(Context))]
    [MemberNotNullWhen(true, nameof(CurrentScope))]
    [MemberNotNullWhen(true, nameof(Iterator))]
    [MemberNotNullWhen(true, nameof(Source))]
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

    /// <summary>
    /// Initialize a BakedInterpreter.
    /// </summary>
    public BakedInterpreter()
    {
        ErrorReporter = new CommonErrorReporter(this);
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
        State = new StateMachine<ParserState>(ParserState.Any);
        Context = new InterpreterContext();
        CurrentScope = Context;
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
    public BakedInterpreter WithEnvironment(BakedEnvironment? environment)
    {
        Environment = environment;

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
        CurrentScope = null;

        SourceLocked = false;
    }
    
    /// <summary>
    /// Report an error using the current token as a source.
    /// </summary>
    /// <param name="id">Optional error ID.</param>
    /// <param name="message">Error message.</param>
    public BakedError ReportError(string? id, string message)
    {
        AssertReady();
        
        return ReportError(id, message, Iterator.Current.Span.Start);
    }

    /// <summary>
    /// Report an error with the required arguments to create a <see cref="BakedError"/>.
    /// </summary>
    /// <param name="id">Optional error ID.</param>
    /// <param name="message">Error message.</param>
    /// <param name="sourceIndex">Error index.</param>
    public BakedError ReportError(string? id, string message, int sourceIndex)
    {
        AssertReady();
        
        return ReportError(new BakedError(id, message, sourceIndex));
    }

    /// <summary>
    /// Report a raw <see cref="BakedError"/>.
    /// </summary>
    /// <param name="error">Error to report.</param>
    public BakedError ReportError(BakedError error)
    {
        AssertReady();
        
        ErrorReported?.Invoke(this, error);

        return error;
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
        
        instruction = null;

        if (!Iterator.TryMoveNext(out var first))
        {
            return false;
        }
        
        if (first.Id is LexerTokenId.Whitespace or LexerTokenId.Newline)
            SkipWhitespaceAndNewlines();

        switch (Iterator.Current.Id)
        {
            case LexerTokenId.OpenBracket: // Processor statement
            {
                SkipWhitespaceAndNewlines();
                
                var nameToken = Iterator.Current;

                if (ErrorReporter.TestUnexpectedTokenType(nameToken, out var nameError,
                        LexerTokenId.Alphabetic, LexerTokenId.AlphaNumeric))
                {
                    instruction = new InvalidInstruction(nameError.Value);

                    break;
                }

                var name = nameToken.ToString();
                
                SkipWhitespaceAndNewlines();
                
                var colonToken = Iterator.Current;
                
                if (ErrorReporter.TestUnexpectedTokenType(colonToken, out var colonError, 
                        LexerTokenId.Colon))
                {
                    instruction = new InvalidInstruction(colonError.Value);

                    break;
                }
                
                SkipWhitespaceAndNewlines();

                var parseResult = TryParseValue(out var value);
                
                if (!parseResult.Success)
                {
                    instruction = new InvalidInstruction(ErrorReporter.ReportInvalidValue(nameToken));

                    break;
                }

                instruction = new ProcessorStatementInstruction(name, value, nameToken.Span.Start);
                
                break;
            }
            case LexerTokenId.Alphabetic:
            case LexerTokenId.AlphaNumeric: // 
            {
                switch (Iterator.Current.ToString())
                {
                    case "method":
                    {
                        break; // TODO
                    }
                    case "return":
                    {
                        break; // TODO
                    }
                    default: // Variable, invocation, control statement
                    {
                        var startToken = Iterator.Current;
                        var identifierParseResult = TryParseIdentifier(out var path);
                        
                        if (!identifierParseResult.Success)
                        {
                            instruction = new InvalidInstruction(identifierParseResult.Error);

                            break;
                        }

                        SkipWhitespaceAndNewlinesReserved();

                        var reference = GetVariableReference(path);

                        switch (Iterator.Current.Id)
                        {
                            case LexerTokenId.LeftParenthesis:
                                SkipWhitespaceAndNewlines();

                                var valueExpected = true;
                                var parameters = new List<BakedObject>();
                                
                                if (!reference.TryGetVariable(out var variable))
                                {
                                    instruction = new InvalidInstruction(new BakedError(
                                        null,
                                        "Cannot invoke a null value.",
                                        startToken.Span.Start));

                                    break;
                                }

                                if (variable.Value is not IBakedCallable callable)
                                {
                                    instruction = new InvalidInstruction(new BakedError(
                                        null,
                                        "Cannot invoke a non-callable object.",
                                        startToken.Span.Start));

                                    break;
                                }

                                while (Iterator.TryMoveNext(out var next))
                                {
                                    switch (next.Id)
                                    {
                                        case LexerTokenId.RightParenthesis:
                                            goto ParametersCompleted;
                                        case LexerTokenId.Comma:
                                            if (valueExpected)
                                                parameters.Add(new BakedNull());
                                            
                                            valueExpected = true;
                                            break;
                                        default:
                                            var valueResult = TryParseValue(out var parameter);

                                            if (!valueResult.Success)
                                            {
                                                instruction = new InvalidInstruction(valueResult.Error);
                                                
                                                break;
                                            }
                                            
                                            parameters.Add(parameter);
                                            valueExpected = false;
                                            
                                            break;
                                    }
                                }
                                
                                ParametersCompleted:

                                instruction = new ObjectInvocationInstruction(callable, parameters.ToArray(), Iterator.Current.Span.Start);
                                
                                break;
                            case LexerTokenId.Equals:
                                SkipWhitespaceAndNewlines();

                                var valueParseResult = TryParseValue(out var value);
                                
                                if (!valueParseResult.Success)
                                {
                                    instruction = new InvalidInstruction(valueParseResult.Error);

                                    break;
                                }

                                instruction = new VariableAssignmentInstruction(reference, value, Iterator.Current.Span.Start);

                                break;
                            default:
                                instruction = new InvalidInstruction(new BakedError()); // TODO

                                break;
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
    [MemberNotNull(nameof(Context))]
    [MemberNotNull(nameof(CurrentScope))]
    [MemberNotNull(nameof(Iterator))]
    [MemberNotNull(nameof(Source))]
    public void AssertReady()
    {
        if (!IsReady)
            throw new InvalidOperationException(
                $"The interpreter has not been initialized. Try calling '{nameof(Init)}' or '{nameof(WithSource)}' first.");
    }
    
    private TryResult TryParseValue(out BakedObject value)
    {
        AssertReady();
        
        value = new BakedNull();
        var startToken = Iterator.Current;
        
        switch (Iterator.Current.Id)
        {
            case LexerTokenId.Alphabetic:
            case LexerTokenId.AlphaNumeric:
            {
                var identifierResult = TryParseIdentifier(out var path);
                
                if (!identifierResult.Success)
                    return identifierResult;

                var reference = GetVariableReference(path);
                
                if (reference.TryGetVariable(out var variable))
                {
                    value = variable.Value;
                    
                    return new TryResult(true);
                }

                return new TryResult(false, 
                    new BakedError(
                        null, 
                        $"Variable, variable path, or part of path " + 
                        $"'{string.Join(".", path.AsEnumerable())}' does not exist.",
                        startToken.Span.Start));
            }
            case LexerTokenId.Numeric:
            {
                value = new BakedInteger(Iterator.Current.ToString());

                return new TryResult(true);
            }
            case LexerTokenId.Quote:
            case LexerTokenId.DoubleQuote:
            {
                var quoteType = Iterator.Current.Id;
                var builder = new StringBuilder();
                var escaped = false;
                
                while (Iterator.TryMoveNext(out var next))
                {
                    if (next.Id == quoteType && !escaped)
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
                    
                    escaped = !escaped && next.Is(LexerTokenId.Backslash);
                }

                value = new BakedString(builder.ToString());

                return new TryResult(true);
            }
        }

        return new TryResult(false,
            new BakedError(
                null,
                "Expected a value (variable, string, integer, etc).",
                startToken.Span.Start));
    }
    
    private VariableReference GetVariableReference(LexerToken[] path)
    {
        return new VariableReference(path.Select(c => c.ToString()), this, CurrentScope);
    }

    private TryResult TryParseIdentifier(out LexerToken[] path)
    {
        AssertReady();
        
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
                    if (ErrorReporter.TestUnexpectedTokenType(next, out var error,
                            LexerTokenId.Alphabetic, LexerTokenId.AlphaNumeric))
                    {
                        return new TryResult(false, error.Value);
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

        return new TryResult(true);
    }
    
    private int SkipWhitespace()
    {
        AssertReady();
        
        var stride = 0;

        foreach (var token in Iterator.TakeWhile(t => t.Id is LexerTokenId.Whitespace))
        {
            stride += token.Span.Size;
        }

        return stride;
    }
    
    private int SkipWhitespaceAndNewlines()
    {
        AssertReady();
        
        var stride = 0;

        foreach (var token in Iterator.TakeWhile(t => t.Id is LexerTokenId.Whitespace or LexerTokenId.Newline))
        {
            stride += token.Span.Size;
        }

        return stride;
    }
    
    private int SkipWhitespaceAndNewlinesReserved()
    {
        AssertReady();
        
        if (Iterator.Current.Id is not LexerTokenId.Whitespace or LexerTokenId.Newline)
            return 0;
        
        var stride = 0;

        foreach (var token in Iterator.TakeWhile(t => t.Id is LexerTokenId.Whitespace or LexerTokenId.Newline))
        {
            stride += token.Span.Size;
        }

        return stride;
    }

    private readonly record struct TryResult(bool Success, BakedError Error = default);

    private enum ParserState
    {
        Any,
        
        MethodBody,
        ControlStatementBody,
    }
}