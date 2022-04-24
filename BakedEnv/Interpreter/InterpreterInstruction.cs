namespace BakedEnv.Interpreter;

/// <summary>
/// A single instruction executed by the interpreter. Can be used during script debugging.
/// </summary>
public abstract class InterpreterInstruction
{
    /// <summary>
    /// Helper value to determine this instruction's location within an IBakedSource.
    /// </summary>
    public int SourceIndex { get; }

    /// <param name="sourceIndex">The source index of this instruction.</param>
    protected InterpreterInstruction(int sourceIndex = 0)
    {
        SourceIndex = sourceIndex;
    }

    /// <summary>
    /// Execute this instruction inside of the provided scope.
    /// </summary>
    /// <param name="scope">The contextual scope.</param>
    public abstract void Execute(IBakedScope scope);

    /// <inheritdoc/>
    public abstract override string ToString();
}