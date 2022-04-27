namespace BakedEnv.Interpreter.Instructions;

/// <summary>
/// A single instruction executed by the interpreter.
/// </summary>
public abstract class InterpreterInstruction
{
    /// <summary>
    /// Helper value to determine this instruction's location within an IBakedSource.
    /// </summary>
    public int SourceIndex { get; }

    /// <param name="sourceIndex">The source index of this instruction.</param>
    protected InterpreterInstruction(int sourceIndex)
    {
        SourceIndex = sourceIndex;
    }

    /// <summary>
    /// Execute this instruction inside of the provided scope.
    /// </summary>
    /// <param name="interpreter">The target interpreter.</param>
    /// <param name="scope">The contextual scope.</param>
    public abstract void Execute(BakedInterpreter interpreter, IBakedScope scope);
}