using BakedEnv.Interpreter.Expressions;
using BakedEnv.Interpreter.Instructions;
using BakedEnv.Interpreter.IntermediateTokens;
using BakedEnv.Interpreter.IntermediateTokens.Raw;
using BakedEnv.Interpreter.InterpreterParsers.Expressions;

namespace BakedEnv.Interpreter.InterpreterParsers.Nodes;

public class StatementParserNode : InterpreterParserNode
{
    public override DescendResult Descend(IntermediateToken token)
    {
        return DescendResult.Successful(this);
    }

    public override InterpreterInstruction Parse(IntermediateToken first, InterpreterIterator iterator, ParserContext context)
    {
        var expressionParser = new ExpressionParser();
        var expression = expressionParser.Parse(first, iterator, context, out var error);

        if (error != null)
        {
            return error.Value.ToInstruction();
        }

        if (!TryMoveNext(iterator, out var next, out var nextError))
            return nextError.ToInstruction();

        switch (next)
        {
            case EqualsToken:
            {
                if (expression is not IAssignableExpression assignable)
                    return BakedError.ENonAssignable(expression.GetType().Name, next.StartIndex).ToInstruction();

                if (!TryMoveNext(iterator, out next, out nextError))
                    return nextError.ToInstruction();
                
                var assignExpression = expressionParser.Parse(next, iterator, context, out error);

                if (error != null)
                {
                    return error.Value.ToInstruction();
                }

                return new AssignmentInstruction(assignable, assignExpression, next.StartIndex);
            }
        }
        
        return BakedError.EUnknownStatement(next.StartIndex).ToInstruction();
    }
}