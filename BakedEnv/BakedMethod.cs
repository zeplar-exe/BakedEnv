using BakedEnv.Interpreter;
using BakedEnv.Interpreter.Instructions;
using BakedEnv.Objects;

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
    
    public BakedMethod(IEnumerable<InterpreterInstruction> instructions)
    {
        Instructions = new List<InterpreterInstruction>(instructions);
    }
}