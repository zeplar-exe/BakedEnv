using BakedEnv.Interpreter.Expressions;
using BakedEnv.Interpreter.Instructions;
using BakedEnv.Interpreter.Scopes;

namespace BakedEnv.ControlStatements;

public class WhileStatementDefinition : ControlStatementDefinition
{
    public override bool Match(string name, int parameterCount)
    {
        return name == "while" && parameterCount == 1;
    }

    public override void Execute(InvocationContext context, BakedExpression[] parameters, 
        IEnumerable<InterpreterInstruction> instructions)
    {
        var statementScope = new BakedScope(context.Scope);
        
        while (parameters[0].Evaluate(context).Equals(true))
        {
            foreach (var instruction in instructions)
            {
                instruction.Execute(context with { Scope = statementScope });
            }
        }
    }
}