namespace BakedEnv.Interpreter.Instructions;

public class ActionInstruction : InterpreterInstruction
{
    public Action<BakedInterpreter, IBakedScope> Action { get; }

    public ActionInstruction(Action<BakedInterpreter, IBakedScope> action) : base(-1)
    {
        Action = action;
    }

    public override void Execute(BakedInterpreter interpreter, IBakedScope scope)
    {
        Action.Invoke(interpreter, scope);
    }
}