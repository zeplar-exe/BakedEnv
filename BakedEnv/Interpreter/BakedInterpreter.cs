using System.Diagnostics.CodeAnalysis;

using BakedEnv.Common;
using BakedEnv.Environment;
using BakedEnv.Interpreter.Instructions;
using BakedEnv.Interpreter.IntermediateParsers;
using BakedEnv.Interpreter.IntermediateParsers.Common;
using BakedEnv.Interpreter.Lexer;
using BakedEnv.Interpreter.Scopes;
using BakedEnv.Sources;

namespace BakedEnv.Interpreter;


/// <summary>
/// Main interface for interpreting BakedEnv scripts.
/// </summary>
public sealed class BakedInterpreter : IDisposable
{
    private InterpreterIterator? Iterator { get; set; }
    private IBakedScope CurrentScope { get; set; }

    /// <summary>
    /// The current environment used during interpretation.
    /// </summary>
    public BakedEnvironment? Environment { get; set; }
    
    /// <summary>
    /// Externally accessible context of the interpreter. Can be used to edit variables and such at runtime.
    /// </summary>
    public InterpreterContext Context { get; private set; }

    private IBakedSource b_source;

    /// <summary>
    /// The current source being interpreted.
    /// </summary>
    public IBakedSource Source
    {
        get => b_source;
        [MemberNotNull(nameof(b_source))]
        set
        {
            ArgumentNullException.ThrowIfNull(value);

            b_source = value;
        }
    }
    
    public ErrorReporter Error { get; }

    public BakedInterpreter(IBakedSource source) : this(new BakedEnvironment(), source)
    {
        
    }

    public BakedInterpreter(BakedEnvironment environment, IBakedSource source)
    {
        ArgumentNullException.ThrowIfNull(environment);
        ArgumentNullException.ThrowIfNull(source);
        
        Source = source;
        Environment = environment;
        Error = new ErrorReporter();
        
        ResetState();
    }

    /// <summary>
    /// Reset interpreter state (parser iteration, scopes).
    /// </summary>
    [MemberNotNull(nameof(Context))]
    [MemberNotNull(nameof(CurrentScope))]
    public void ResetState()
    {
        Iterator = null;
        Context = new InterpreterContext();
        CurrentScope = Context;
        Error.Clear();
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
        instruction = null;

        if (Iterator == null)
        {
            var root = new AnyParser();
            var lexer = new TextLexer(Source.EnumerateCharacters());
            var iterator = new LexerIterator(lexer);
            
            Iterator = new InterpreterIterator(root.Parse(iterator));
        }

        return instruction != null;
    }

    public void Dispose()
    {
        Iterator?.Dispose();
        Environment?.Dispose();
    }
}