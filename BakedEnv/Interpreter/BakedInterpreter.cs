using System.Diagnostics.CodeAnalysis;
using BakedEnv.Interpreter.Sources;
using Jammo.ParserTools;
using Jammo.ParserTools.Lexing;
using Jammo.ParserTools.Tools;

namespace BakedEnv.Interpreter;

/// <summary>
/// Main interface for interpreting BakedEnv scripts.
/// </summary>
public class BakedInterpreter
{
    private EnumerableIterator<LexerToken>? Iterator { get; set; }
    private StateMachine<ParserState>? State { get; set; }
    
    private int SourceIndex { get; set; }
    
    /// <summary>
    /// Externally accessible context of the interpreter. Can be used to edit variables and such at runtime.
    /// </summary>
    public InterpreterContext? Context { get; private set; }
    
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
    /// Specify the IBakedSource to be parsed.
    /// </summary>
    /// <param name="source">The IBakedSource to be parsed.</param>
    /// <exception cref="InvalidOperationException">The interpreter is locked and disallows manipulation of the source.</exception>
    public void UseSource(IBakedSource source)
    {
        if (SourceLocked)
            throw new InvalidOperationException("Cannot use a source while the interpreter is locked.");
        
        Source = source;
    }

    /// <summary>
    /// Initialize the interpreter and reset necessary values.
    /// </summary>
    /// <exception cref="InvalidOperationException">The source has not been set.</exception>
    public void Init()
    {
        if (Source == null)
            throw new InvalidOperationException(
                $"Cannot initialize from an unset source. Call '{nameof(UseSource)}' first.");
        
        Iterator = new Lexer(Source.EnumerateCharacters()).ToIterator();
        State = new StateMachine<ParserState>(ParserState.Root);
        Context = new InterpreterContext();
        IsReady = true;

        SourceLocked = true;
    }

    /// <summary>
    /// Initialize the interpreter with a specified IBakedSource.
    /// </summary>
    /// <param name="source">The baked source to use.</param>
    public void InitWith(IBakedSource source)
    {
        UseSource(source);
        Init();
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
    /// Consume the next token(s) and retrieve an instruction.
    /// </summary>
    /// <param name="instruction">Interpreted instruction information.</param>
    /// <returns>Whether an instruction could be parsed. Should only return false upon reaching the
    /// end of an IBakedSource's content.</returns>
    /// <exception cref="InvalidOperationException">The interpreter has not been initalized.</exception>
    public bool TryGetNextInstruction([NotNullWhen(true)] out InterpreterInstruction? instruction)
    {
        if (!IsReady)
            throw new InvalidOperationException(
                $"The interpreter has not been initialized. Try calling '{nameof(Init)}' or '{nameof(InitWith)}' first.");
        
        instruction = null;
        
        var sourceStride = 0;

        if (!Iterator.TryMoveNext(out var first))
        {
            IsReady = false;
            
            return false;
        }

        sourceStride += first.Span.Size;

        switch (first.Id)
        {
            case LexerTokenId.OpenBracket:
            {
                break; // Processor statements
            }
            case LexerTokenId.Alphabetic:
            {
                switch (first.ToString())
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
                        break;
                    }
                }
                
                break;
            }
            case LexerTokenId.AlphaNumeric:
            {
                break;
            }
        }

        SourceIndex += sourceStride;
        
        return true;
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