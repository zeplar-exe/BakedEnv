using BakedEnv.Interpreter.Expressions;
using BakedEnv.Interpreter.IntermediateTokens;

namespace BakedEnv.Interpreter.InterpreterParsers.Expressions;

internal class ExpressionParser
{
    public BakedExpression Parse(IntermediateToken first, InterpreterIterator iterator, ParserContext context)
    {
        using var reporter = context.CreateReporter();
        
        var selector = new ExpressionSelector(context.Interpreter.Environment);
        var parsers = selector.SelectParsers(first);

        var marker = iterator.CreateMarker();
        
        foreach (var parser in parsers)
        {
            try
            {
                return parser.Parse(first, iterator, context);
            }
            catch (InterpreterInternalException e)
            {
                reporter.Report(e.Errors);
                
                marker.Restore();
            }
        }
        
        reporter.Report(BakedError.EUnknownExpression(first.GetType().Name, first.StartIndex));

        return new NullExpression();
    }
}