using BakedEnv.Interpreter;

namespace BakedEnv;

public class ExitHandler : IExitHandler
{
    private Action<BakedInterpreter> Action { get; }

    public ExitHandler(Action<BakedInterpreter> action)
    {
        Action = action;
    }

    public void BeforeTearDown(BakedInterpreter interpreter)
    {
        Action.Invoke(interpreter);
    }
}