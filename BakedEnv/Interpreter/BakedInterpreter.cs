using System.Diagnostics.CodeAnalysis;

using BakedEnv.Common;
using BakedEnv.Environment;
using BakedEnv.Interpreter.Instructions;
using BakedEnv.Interpreter.IntermediateParsers;
using BakedEnv.Interpreter.IntermediateParsers.Common;
using BakedEnv.Interpreter.InterpreterParsers;
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
    
    private InterpreterParserTreeCreator ParserTreeCreator { get; set; } = InterpreterParserTreeCreator.Default();

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

    public BakedInterpreter(IBakedSource source) : this(null, source)
    {
        
    }

    public BakedInterpreter(BakedEnvironment? environment, IBakedSource source)
    {
        ArgumentNullException.ThrowIfNull(source);
        
        Source = source;
        Environment = environment;
        Error = new ErrorReporter();
        
        ResetState();
    }

    public void UseParserTreeCreator(InterpreterParserTreeCreator creator)
    {
        ArgumentNullException.ThrowIfNull(creator);
        
        ParserTreeCreator = creator;
    }

    /// <summary>
    /// Reset interpreter state (parser iteration, scopes, errors).
    /// </summary>
    [MemberNotNull(nameof(Context))]
    [MemberNotNull(nameof(CurrentScope))]
    public void ResetState()
    {
        Iterator?.Dispose();
        Environment?.Dispose();
        
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
    public bool TryGetNextInstruction([NotNullWhen(true)] out InterpreterInstruction? instruction)
    {
        instruction = null;

        EnsureIterator();

        if (!Iterator.TryPeekNext(out var next))
            return false;

        if (!next.IsComplete)
        {
            instruction = new InvalidInstruction(
                BakedError.EIncompleteIntermediateToken(next.GetType().Name, next.StartIndex));
        }

        var tree = ParserTreeCreator.Create();
        var result = tree.Descend(next);

        if (result.Success)
        {
            instruction = result.Parser.Parse(next, Iterator);
        }
        else
        {
            instruction = new InvalidInstruction(BakedError.EUnknownTokenSequence(next.StartIndex));
        }

        return true;
    }

    [MemberNotNull(nameof(Iterator))]
    private void EnsureIterator()
    {
        if (Iterator == null)
        {
            var root = new AnyParser();
            var lexer = new TextLexer(Source.EnumerateCharacters());
            var iterator = new LexerIterator(lexer);
            
            Iterator = new InterpreterIterator(root.Parse(iterator));
        }
    }

    public void Dispose()
    {
        Iterator?.Dispose();
        Environment?.Dispose();
    }
}