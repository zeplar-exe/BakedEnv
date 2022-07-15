using BakedEnv.Interpreter.Expressions;
using BakedEnv.Interpreter.Instructions;
using BakedEnv.Interpreter.Scopes;

namespace BakedEnv.ControlStatements;

public abstract class ControlStatementDefinition
{
    public abstract bool Match(string name, int parameterCount);
    public abstract void Execute(InvocationContext context, BakedExpression[] parameters, IEnumerable<InterpreterInstruction> instructions);
}