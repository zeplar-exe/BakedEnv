using BakedEnv.Interpreter.Expressions;
using BakedEnv.Interpreter.Expressions.Arithmetic;
using BakedEnv.Interpreter.Parsers;
using TokenCs;

namespace BakedEnv.Interpreter.ParserModules.Expressions;

internal class ArithmeticParser : ParserModule
{
    public ArithmeticParser(InterpreterInternals internals) : base(internals)
    {
        
    }

    public ArithmeticParserResult ParseFrom(TailExpressionParserResult start)
    {
        var builder = new ArithmeticParserResult.Builder();
        var previous = start;
        
        while (true)
        {
            if (Internals.TestEndOfFile(out var first, out var eofResult))
            {
                return builder.Build(false);
            }
    
            if (!IsArithmetic(first))
            {
                Internals.Iterator.PushCurrent();
                
                return builder.Build(true);
            }
    
            Internals.IteratorTools.SkipWhitespaceAndNewlines();
    
            var expressionParser = new TailExpressionParser(Internals);
            var result = expressionParser.Parse(ArithmeticInclusionMode.Exclude);
            
            if (!result.IsComplete)
            {
                return builder.Build(false);
            }

            builder.WithOperator(new OperatorInfo(first, previous, result));

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