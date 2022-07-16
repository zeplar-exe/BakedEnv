using BakedEnv.Interpreter.Expressions;
using BakedEnv.Interpreter.Instructions;
using BakedEnv.Interpreter.Scopes;

namespace BakedEnv.Keywords;

public class ReturnKeywordDefinition : KeywordDefinition
{

    public override bool Match(string name, int parameterCount)
    {
        return name == "return" && parameterCount == 1;
    }

    public override InterpreterInstruction CreateInstruction(InvocationContext context, BakedExpression[] parameters)
    {
        return new ReturnInstruction(context.SourceIndex, parameters[0]);
    }
}