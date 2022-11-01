using System.Diagnostics.CodeAnalysis;

using BakedEnv.Common;
using BakedEnv.Environment;
using BakedEnv.Interpreter.Instructions;
using BakedEnv.Interpreter.IntermediateParsers;
using BakedEnv.Interpreter.IntermediateParsers.Common;
using BakedEnv.Interpreter.IntermediateTokens;
using BakedEnv.Interpreter.Lexer;
using BakedEnv.Interpreter.Scopes;
using BakedEnv.Sources;

namespace BakedEnv.Interpreter;


/// <summary>
/// Main interface for interpreting BakedEnv scripts.
/// </summary>
public class BakedInterpreter
{
    private InterpreterIterator? Iterator { get; set; }
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
    
    public ErrorReporter? Error { get; private set; }

    /// <summary>
    /// Initialize the interpreter and reset necessary values.
    /// </summary>
    /// <exception cref="InvalidOperationException">The source has not been set.</exception>
    public void Init()
    {
        if (Source == null)
            throw new InvalidOperationException(
                $"Cannot initialize from an unset source. Call '{nameof(WithSource)}' first.");

        var root = new AnyParser();
        var lexer = new TextLexer(Source.EnumerateCharacters());
        var iterator = new ParserIterator(lexer);

        Iterator = new InterpreterIterator(root.Parse(iterator));
        Context = new InterpreterContext();
        CurrentScope = Context;
        IsReady = true;
        Error = new ErrorReporter();

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
        Context = null;
        IsReady = false;
        CurrentScope = null;
       
        SourceLocked = false;
        Error = null;
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
}