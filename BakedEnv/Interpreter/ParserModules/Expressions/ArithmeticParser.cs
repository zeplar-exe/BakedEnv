using BakedEnv.Interpreter.Parsers;
using TokenCs;

namespace BakedEnv.Interpreter.ParserModules.Expressions;

internal class ArithmeticParser : ParserModule
{
    public ArithmeticParser(ParserEnvironment internals) : base(internals)
    {
        
    }

    public ArithmeticParserResult ParseFrom(ExpressionParserResult start)
    {
        var builder = new ArithmeticParserResult.Builder();
        var previous = start;
        var operatorCount = 0;
        
        while (true)
        {
            if (!Internals.Iterator.TryMoveNext(out var first))
            {
                return builder.Build(false);
            }

            builder.WithToken(first);
    
            if (!IsArithmetic(first))
            {
                if (operatorCount == 0)
                    return builder.Build(false);
                
                Internals.Iterator.PushCurrent();
                
                return builder.Build(true);
            }
    
            Internals.IteratorTools.SkipWhitespaceAndNewlines();
    
            var expressionParser = new ExpressionParser(Internals);
            var result = expressionParser.Parse(ArithmeticInclusionMode.Exclude);
            
            if (!result.IsComplete)
            {
                return builder.Build(false);
            }

            builder.WithOperator(new OperatorInfo(first, previous, result));
            operatorCount++;

            previous = result;
        }
    }
    
    private static bool IsArithmetic(LexerToken token)
    {
        return token.Type is
            LexerTokenType.Plus or
            LexerTokenType.Dash or
            LexerTokenType.Star or
            LexerTokenType.Slash or
            LexerTokenType.Caret or
            LexerTokenType.Percent;
    }
}