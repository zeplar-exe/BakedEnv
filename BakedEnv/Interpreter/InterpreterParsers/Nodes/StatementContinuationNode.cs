using BakedEnv.Interpreter.Expressions;
using BakedEnv.Interpreter.IntermediateTokens;
using BakedEnv.Interpreter.Lexer;

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
        if (token.IsRawType(TextualTokenType.Equals))
            return DescendResult.Success(new VariableAssignmentParserNode(Expression, ExpressionFirstToken));

        return DescendResult.Failure();
    }
}