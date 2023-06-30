using System.Diagnostics.CodeAnalysis;

using BakedEnv.Common;
using BakedEnv.Environment;
using BakedEnv.Interpreter.Instructions;
using BakedEnv.Interpreter.IntermediateParsers;
using BakedEnv.Interpreter.IntermediateParsers.Common;
using BakedEnv.Interpreter.IntermediateTokens;
using BakedEnv.Interpreter.InterpreterParsers;
using BakedEnv.Interpreter.InterpreterParsers.Nodes;
using BakedEnv.Interpreter.Lexer;
using BakedEnv.Interpreter.Scopes;
using BakedEnv.Sources;

namespace BakedEnv.Interpreter;


/// <summary>
/// Main interface for interpreting scripts.
/// </summary>
public sealed class BakedInterpreter : IDisposable
{
    private InterpreterIterator? Iterator { get; set; }
    private IBakedScope? CurrentScope { get; set; }

    /// <summary>
    /// The current environment used during interpretation.
    /// </summary>
    public BakedEnvironment? Environment { get; set; }
    
    /// <summary>
    /// Externally accessible context of the interpreter. Can be used to edit variables and such at runtime.
    /// </summary>
    public InterpreterContext? Context { get; private set; }

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

    public BakedInterpreter(IBakedSource source)
    {
        ArgumentNullException.ThrowIfNull(source);
        
        Source = source;
        Error = new ErrorReporter();
    }

    public BakedInterpreter(BakedEnvironment environment, IBakedSource source)
    {
        ArgumentNullException.ThrowIfNull(environment);
        ArgumentNullException.ThrowIfNull(source);
        
        Environment = environment;
        Source = source;
        Error = new ErrorReporter();
    }

    /// <summary>
    /// Reset interpreter state (parser iteration, scopes, errors).
    /// </summary>
    [MemberNotNull(nameof(Context))]
    [MemberNotNull(nameof(CurrentScope))]
    public void ResetState()
    {
        Iterator?.Dispose();
        
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

        if (!Iterator.TryMoveNext(out var next))
            return false;

        if (!next.IsComplete)
        {
            instruction = new InvalidInstruction(
                BakedError.EIncompleteIntermediateToken(next.GetType().Name, next.StartIndex));

            return true;
        }

        var tree = new InterpreterParserTree();
        
        tree.RootParserNodes.Add(new ProcessorStatementParserNode());
        tree.RootParserNodes.Add(new StatementParserNode());

        try
        {
            var result = tree.Descend(next);

            if (result.IsSuccess)
            {
                var context = new ParserContext(this, Context);
            
                instruction = result.Parser.Parse(next, Iterator, context);
            }
            else
            {
                instruction = new InvalidInstruction(BakedError.EUnknownTokenSequence(next.StartIndex));
            }

            return true;
        }
        catch (InterpreterInternalException e)
        {
            instruction = new InvalidInstruction(e.Error);

            return false;
        }
    }

    [MemberNotNull(nameof(Iterator), nameof(Context), nameof(CurrentScope))]
    private void EnsureIterator()
    {
        if (Iterator == null)
        {
            var root = new AnyIntermediateParser();
            var lexer = new TextLexer(Source.EnumerateCharacters());
            var iterator = new LexerIterator(lexer);
            
            Iterator = new InterpreterIterator(root.Parse(iterator));
            
            Iterator.Ignore<SingleLineCommentToken>();
            Iterator.Ignore<MultiLineCommentToken>();
        }

        Context ??= new InterpreterContext();
        CurrentScope ??= CurrentScope;
    }

    public void Dispose()
    {
        Iterator?.Dispose();
    }
}