using BakedEnv.Interpreter;
using BakedEnv.Interpreter.Instructions;
using BakedEnv.Interpreter.Scopes;
using BakedEnv.Objects;

namespace BakedEnv.ControlStatements;

public class IfStatementExecution : ControlStatementExecution
{
    public override void Execute(BakedInterpreter interpreter, IBakedScope scope, BakedObject[] parameters,
        IEnumerable<InterpreterInstruction> instructions)
    {
        var statementScope = new BakedScope(scope);
        
        if (parameters[0].Equals(true))
        {
            foreach (var instruction in instructions)
            {
                instruction.Execute(interpreter, statementScope);
            }
        }
    }
}