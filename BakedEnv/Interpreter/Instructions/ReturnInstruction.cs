using BakedEnv.Interpreter.Expressions;
using BakedEnv.Interpreter.Scopes;

namespace BakedEnv.Interpreter.Instructions;

public class ReturnInstruction : InterpreterInstruction
{
    public BakedExpression ReturnValue { get; }

    public ReturnInstruction(int sourceIndex, BakedExpression expression) : base(sourceIndex)
    {
        ReturnValue = expression;
    }

    public override void Execute(InvocationContext context)
    {
        
    }
}