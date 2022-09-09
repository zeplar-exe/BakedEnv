using BakedEnv.Interpreter.Expressions;
using BakedEnv.Objects;

namespace BakedEnv.Interpreter.IntermediateTokens.Interfaces;

public interface IExpressionToken
{
    public BakedExpression CreateExpression();
}