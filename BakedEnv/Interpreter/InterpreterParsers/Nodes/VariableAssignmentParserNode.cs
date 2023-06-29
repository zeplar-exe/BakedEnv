using BakedEnv.Interpreter.Expressions;
using BakedEnv.Interpreter.Instructions;
using BakedEnv.Interpreter.IntermediateTokens;
using BakedEnv.Interpreter.InterpreterParsers.Expressions;

namespace BakedEnv.Interpreter.InterpreterParsers.Nodes;

public class VariableAssignmentParserNode : InterpreterParserNode
{
    private BakedExpression AssignExpression { get; }
    private IntermediateToken ExpressionFirstToken { get; }

    public VariableAssignmentParserNode(BakedExpression assignExpression, IntermediateToken expressionFirstToken)
    {
        AssignExpression = assignExpression;
        ExpressionFirstToken = expressionFirstToken;
    }

    public override DescendResult Descend(IntermediateToken token)
    {
        return DescendResult.SuccessIf(this, () => token is EqualsToken);
    }

    public override InterpreterInstruction Parse(IntermediateToken first, InterpreterIterator iterator, ParserContext context)
    {
        if (AssignExpression is not IAssignableExpression assignable)
        {
            BakedError.ENonAssignable(AssignExpression.GetType().Name, ExpressionFirstToken.StartIndex).Throw();

            return new EmptyInstruction(0);
        }

        if (!TryMoveNext(iterator, out var next, out var nextError))
            nextError.Throw();

        var expressionParser = new FullExpressionParser();
        var valueExpression = expressionParser.Parse(next, iterator, context);
        
        return new AssignmentInstruction(assignable, valueExpression, next.StartIndex);
    }
}