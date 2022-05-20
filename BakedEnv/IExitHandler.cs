using BakedEnv.Interpreter;

namespace BakedEnv;

public interface IExitHandler
{
    public void BeforeTearDown(BakedInterpreter interpreter);
}