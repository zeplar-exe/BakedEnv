using BakedEnv.Interpreter;
using BakedEnv.Interpreter.Expressions;
using BakedEnv.Interpreter.Instructions;
using BakedEnv.Interpreter.Scopes;
using BakedEnv.Objects;

namespace BakedEnv.ControlStatements;

public class IfStatementDefinition : ControlStatementDefinition
{
    public override bool Match(string name, int parameterCount)
    {
        return name == "if" && parameterCount == 1;
    }

    public override void Execute(InvocationContext context, BakedExpression[] parameters,
        IEnumerable<InterpreterInstruction> instructions)
    {
        var statementScope = new BakedScope(context.Scope);
        
        if (parameters[0].Evaluate(context).Equals(true))
        {
            foreach (var instruction in instructions)
            {
                instruction.Execute(context with { Scope = statementScope });
            }
        }
    }
}