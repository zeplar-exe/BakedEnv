using BakedEnv.Interpreter;
using BakedEnv.Interpreter.Instructions;
using BakedEnv.Interpreter.Scopes;
using BakedEnv.Objects;

namespace BakedEnv.ControlStatements;

public abstract class ControlStatementExecution
{
    public abstract void Execute(InvocationContext context, BakedObject[] parameters, IEnumerable<InterpreterInstruction> instructions);
}