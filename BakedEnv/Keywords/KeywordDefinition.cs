using BakedEnv.Interpreter.Expressions;
using BakedEnv.Interpreter.Instructions;
using BakedEnv.Interpreter.Scopes;

namespace BakedEnv.Keywords;

public abstract class KeywordDefinition
{
    public abstract bool Match(string name, int parameterCount);
    public abstract InterpreterInstruction CreateInstruction(InvocationContext context, BakedExpression[] parameters);
}