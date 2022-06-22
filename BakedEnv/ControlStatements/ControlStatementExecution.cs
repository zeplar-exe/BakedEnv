using BakedEnv.Interpreter;
using BakedEnv.Interpreter.Instructions;
using BakedEnv.Objects;

namespace BakedEnv.ControlStatements;

public abstract class ControlStatementExecution
{
    public abstract BakedObject Execute(BakedInterpreter interpreter, IBakedScope scope, BakedObject[] parameters, IEnumerable<InterpreterInstruction> instructions);
}