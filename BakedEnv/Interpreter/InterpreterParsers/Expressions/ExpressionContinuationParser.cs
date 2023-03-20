using BakedEnv.Common;
using BakedEnv.Interpreter.Expressions;
using BakedEnv.Interpreter.Expressions.Arithmetic;
using BakedEnv.Interpreter.IntermediateTokens.Raw;

namespace BakedEnv.Interpreter.InterpreterParsers.Expressions;

internal class ExpressionContinuationParser
{
    public OperationResult<BakedExpression> Parse(BakedExpression initial, InterpreterIterator iterator, ParserContext context)
    {
        if (iterator.TryMoveNext(out var next))
        {
            switch (next)
            {
                case LeftParenthesisToken:
                {
                    var tupleParser = new ExpressionListParser();
                    var parameters = tupleParser.Parse(iterator, context);

                    if (parameters.HasError)
                        return OperationResult<BakedExpression>.Failure(parameters.Error);

                    return OperationResult<BakedExpression>.Success(new InvocationExpression(initial, parameters.Value));
                }
                // what about chained continuations?
                default:
                {
                    iterator.Reserve();
                    var defaultError = BakedError.EUnknownExpression(next.GetType().Name, next.StartIndex);

                    return OperationResult<BakedExpression>.Failure(defaultError);
                }
            }
        }
        
        var error = BakedError.EEarlyEndOfFile(iterator.Current!.EndIndex);

        return OperationResult<BakedExpression>.Failure(error);
    }
}