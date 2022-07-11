using BakedEnv.Interpreter;
using BakedEnv.Interpreter.Instructions;
using BakedEnv.Interpreter.Scopes;
using BakedEnv.Objects;

namespace BakedEnv.ControlStatements;

public class IfStatementExecution : ControlStatementExecution
{
    public override void Execute(InvocationContext context, BakedObject[] parameters,
        IEnumerable<InterpreterInstruction> instructions)
    {
        var statementScope = new BakedScope(context.Scope);
        
        if (parameters[0].Equals(true))
        {
            foreach (var instruction in instructions)
            {
                instruction.Execute(context with { Scope = statementScope });
            }
        }
    }
}