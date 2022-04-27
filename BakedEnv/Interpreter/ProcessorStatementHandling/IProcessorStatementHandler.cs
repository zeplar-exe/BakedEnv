using BakedEnv.Interpreter.Instructions;

namespace BakedEnv.Interpreter.ProcessorStatementHandling;

/// <summary>
/// Handler for <see cref="ProcessorStatementInstruction">ProcessorStatementInstructions</see>.
/// Used in <see cref="BakedInterpreter"/>.
/// </summary>
public interface IProcessorStatementHandler
{
    /// <summary>
    /// Attempt to handle a <see cref="ProcessorStatementInstruction"/>.
    /// </summary>
    /// <param name="instruction"></param>
    /// <param name="interpreter">The interpreter targeted by the processor statement.</param>
    /// <returns>Whether the processor statement could be handled.</returns>
    public bool TryHandle(ProcessorStatementInstruction instruction, BakedInterpreter interpreter);
}