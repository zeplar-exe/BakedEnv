using BakedEnv.Interpreter;

namespace BakedEnv;

/// <summary>
/// Outline of a BakedEnv method.
/// </summary>
public class BakedMethod
{
    /// <summary>
    /// Instructions to execute in order.
    /// </summary>
    public List<InterpreterInstruction> Instructions { get; }

    public BakedMethod()
    {
        Instructions = new List<InterpreterInstruction>();
    }
}