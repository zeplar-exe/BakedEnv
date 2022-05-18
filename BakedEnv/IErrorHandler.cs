using BakedEnv.Interpreter.Instructions;

namespace BakedEnv;

public interface IErrorHandler
{
    public bool Handle(InvalidInstruction instruction);
}