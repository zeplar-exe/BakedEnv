using BakedEnv.Interpreter;
using BakedEnv.Objects;

namespace BakedEnv.ControlStatements;

public abstract class ControlStatementExecution
{
    public abstract BakedObject Execute(BakedInterpreter interpreter, IBakedScope scope, BakedObject[] parameters);
}