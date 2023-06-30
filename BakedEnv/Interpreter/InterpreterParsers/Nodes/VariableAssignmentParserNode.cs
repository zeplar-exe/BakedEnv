using BakedEnv.Interpreter.Expressions;
using BakedEnv.Interpreter.Instructions;
using BakedEnv.Interpreter.IntermediateTokens;
using BakedEnv.Interpreter.InterpreterParsers.Expressions;
using BakedEnv.Interpreter.Lexer;

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
        return DescendResult.SuccessIf(this, () => token.IsRawType(TextualTokenType.Equals));
    }

    public override InterpreterInstruction Parse(IntermediateToken first, InterpreterIterator iterator, ParserContext context)
    {
        if (AssignExpression is not IAssignableExpression assignable)
        {
            BakedError.ENonAssignable(AssignExpression.GetType().Name, ExpressionFirstToken.StartIndex).Throw();

            return new EmptyInstruction(0);
        }

        var next = iterator.MoveNextOrThrow();

        var expressionParser = new FullExpressionParser();
        var valueExpression = expressionParser.Parse(next!, iterator, context);
        
        return new AssignmentInstruction(assignable, valueExpression, next.StartIndex);
    }
}