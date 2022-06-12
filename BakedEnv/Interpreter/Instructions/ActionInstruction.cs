namespace BakedEnv.Interpreter.Instructions;

/// <summary>
/// Instruction to invoke an <see cref="Action{T1, T2}"/>"/> where a BakedInterpreter and IBakedScope are passed.
/// </summary>
public class ActionInstruction : InterpreterInstruction
{
    /// <summary>
    /// Action to invoke.
    /// </summary>
    public Action<BakedInterpreter, IBakedScope> Action { get; set; }

    /// <summary>
    /// Initialize an ActionInstruction with it's action.
    /// </summary>
    /// <param name="action">Action to invoke during execution.</param>
    /// <param name="sourceIndex">Source index used internally. Defaults to -1.</param>
    public ActionInstruction(Action<BakedInterpreter, IBakedScope> action, int sourceIndex) : base(sourceIndex)
    {
        Action = action;
    }

    /// <inheritdoc />
    public override void Execute(BakedInterpreter interpreter, IBakedScope scope)
    {
        Action.Invoke(interpreter, scope);
    }
}