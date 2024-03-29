using BakedEnv.Interpreter.Scopes;

namespace BakedEnv.Interpreter.Instructions;

/// <summary>
/// A single instruction executed by the interpreter.
/// </summary>
public abstract class InterpreterInstruction
{
    /// <summary>
    /// Helper value to determine this instruction's location within an IBakedSource.
    /// </summary>
    public ulong SourceIndex { get; }

    /// <param name="sourceIndex">The source index of this instruction.</param>
    protected InterpreterInstruction(ulong sourceIndex)
    {
        SourceIndex = sourceIndex;
    }

    /// <summary>
    /// Execute this instruction inside of the provided scope.
    /// </summary>
    /// <param name="interpreter">The target interpreter.</param>
    /// <remarks>This overload implicitly passes the interpreter's context to
    /// <see cref="Execute(BakedEnv.Interpreter.BakedInterpreter, IBakedScope)"/></remarks>
    public virtual void Execute(BakedInterpreter interpreter)
    {
        Execute(new InvocationContext(interpreter, interpreter.Context, SourceIndex));
    }

    /// <summary>
    /// Execute this instruction inside of the provided scope.
    /// </summary>
    /// <param name="interpreter">The target interpreter.</param>
    /// <param name="scope">The contextual scope.</param>
    public abstract void Execute(InvocationContext context);
}