using System.Diagnostics.CodeAnalysis;
using BakedEnv.Interpreter.Instructions;
using BakedEnv.Interpreter.Parsers;
using BakedEnv.Interpreter.Sources;
using BakedEnv.Interpreter.Variables;
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
    private IteratorTools? IteratorTools { get; set; }
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
    /// Informative property of whether the interpreter is ready for parsing.
    /// </summary>
    [MemberNotNullWhen(true, nameof(Context))]
    [MemberNotNullWhen(true, nameof(Iterator))]
    [MemberNotNullWhen(true, nameof(Source))]
    [MemberNotNullWhen(true, nameof(State))]
    [MemberNotNullWhen(true, nameof(IteratorTools))]
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
    /// [MemberNotNullWhen(true, nameof(Context))]
    [MemberNotNullWhen(true, nameof(Context))]
    [MemberNotNullWhen(true, nameof(Iterator))]
    [MemberNotNullWhen(true, nameof(Source))]
    [MemberNotNullWhen(true, nameof(State))]
    [MemberNotNullWhen(true, nameof(IteratorTools))]
    public void Init()
    {
        if (Source == null)
            throw new InvalidOperationException(
                $"Cannot initialize from an unset source. Call '{nameof(WithSource)}' first.");
        
        Iterator = new Lexer(Source.EnumerateCharacters()).ToIterator();
        IteratorTools = new IteratorTools(this, Iterator);
        State = new StateMachine<ParserState>(ParserState.Any);
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
        IteratorTools = null;

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
            IteratorTools.SkipWhitespaceAndNewlines();

        switch (Iterator.Current.Id)
        {
            case LexerTokenId.OpenBracket: // Processor statement
            {
                var parser = CreateProcessorStatementParser();

                instruction = parser.Parse();
                
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
                        var identifierParser = CreateIdentifierParser();
                        var identifierParseResult = identifierParser.TryParseIdentifier(out var path);
                        
                        if (!identifierParseResult.Success)
                        {
                            instruction = new InvalidInstruction(identifierParseResult.Error);

                            break;
                        }

                        IteratorTools.SkipWhitespaceAndNewlinesReserved();
                        
                        var reference = identifierParser.GetVariableReference(path, Context);

                        switch (Iterator.Current.Id)
                        {
                            case LexerTokenId.LeftParenthesis:
                            {
                                var invokeParser = CreateInvocationParser(reference);

                                instruction = invokeParser.Parse();
                                
                                break;
                            }
                            case LexerTokenId.Equals:
                            {
                                IteratorTools.SkipWhitespaceAndNewlines();

                                var expressionParser = CreateExpressionParser();
                                var expResult = expressionParser.TryParseExpression(out var expression);
                                
                                if (!expResult.Success)
                                {
                                    instruction = new InvalidInstruction(expResult.Error);
                                    
                                    break;
                                }
                                
                                instruction = new VariableAssignmentInstruction(
                                    reference, 
                                    expression!, 
                                    Iterator.Current.Span.Start);

                                break;
                            }
                            default:
                            {
                                instruction = new InvalidInstruction(new BakedError()); // TODO

                                break;
                            }
                        }

                        break;
                    }
                }
                
                break;
            }
            case LexerTokenId.CloseCurlyBracket:
            {
                if (State.Current != ParserState.ControlStatementBody)
                {
                    instruction = new InvalidInstruction(new BakedError()); // TODO

                    break;
                }

                instruction = new EmptyInstruction(Iterator.Current.Span.Start);

                State.MoveLast();
                
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
    [MemberNotNull(nameof(Iterator))]
    [MemberNotNull(nameof(Source))]
    [MemberNotNull(nameof(State))]
    [MemberNotNull(nameof(IteratorTools))]
    public void AssertReady()
    {
        if (!IsReady)
            throw new InvalidOperationException(
                $"The interpreter has not been initialized. Try calling '{nameof(Init)}' or '{nameof(WithSource)}' first.");
    }

    internal ValueParser CreateValueParser()
    {
        return new ValueParser(this, Iterator, IteratorTools, Context, ErrorReporter);
    }
    
    internal ParameterParser CreateParameterParser()
    {
        return new ParameterParser(this, Iterator);
    }

    internal ProcessorStatementParser CreateProcessorStatementParser()
    {
        return new ProcessorStatementParser(this, Iterator, IteratorTools, ErrorReporter);
    }

    internal InvocationParser CreateInvocationParser(VariableReference reference)
    {
        return new InvocationParser(this, ErrorReporter, Iterator, IteratorTools, State, reference);
    }

    internal ExpressionParser CreateExpressionParser()
    {
        return new ExpressionParser(this, Iterator);
    }

    internal IdentifierParser CreateIdentifierParser()
    {
        return new IdentifierParser(this, ErrorReporter, IteratorTools, Iterator);
    }
}