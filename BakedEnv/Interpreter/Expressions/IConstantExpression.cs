using BakedEnv.Objects;

namespace BakedEnv.Interpreter.Expressions;

public interface IConstantExpression
{
    public BakedObject EvaluateConstant();
}