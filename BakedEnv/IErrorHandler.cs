using BakedEnv.Interpreter;

namespace BakedEnv;

public interface IErrorHandler
{
    public void HandleError(BakedError error, BakedInterpreter interpreter);
}