using BakedEnv.Interpreter.Expressions;
using BakedEnv.Interpreter.IntermediateTokens;
using BakedEnv.Interpreter.IntermediateTokens.Raw;

namespace BakedEnv.Interpreter.InterpreterParsers.Nodes;

public class StatementContinuationNode : BranchParser
{
    private BakedExpression Expression { get; }
    private IntermediateToken ExpressionFirstToken { get; }

    public StatementContinuationNode(BakedExpression expression, IntermediateToken expressionFirstToken)
    {
        Expression = expression;
        ExpressionFirstToken = expressionFirstToken;
    }

    public override DescendResult Descend(IntermediateToken token)
    {
        switch (token)
        {
            case EqualsToken:
                return DescendResult.Successful(new VariableAssignmentParserNode(Expression, ExpressionFirstToken));
        }
        
        return DescendResult.Failure();
    }
}