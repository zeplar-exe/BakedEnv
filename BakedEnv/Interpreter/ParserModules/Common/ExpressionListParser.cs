using BakedEnv.Interpreter.ParserModules.Expressions;
using BakedEnv.Interpreter.Parsers;
using TokenCs;
using ExpressionParser = BakedEnv.Interpreter.ParserModules.Expressions.ExpressionParser;

namespace BakedEnv.Interpreter.ParserModules.Common;

internal class ExpressionListParser : ParserModule
{
    public ExpressionListParser(ParserEnvironment internals) : base(internals)
    {
        
    }

    public ExpressionListParserResult Parse()
    {
        var builder = new ExpressionListParserResult.Builder();
        
        var expectValue = true;

        while (Internals.Iterator.TryMoveNext(out var token))
        {
            switch (token.Type)
            {
                case LexerTokenType.Comma:
                {
                    if (expectValue)
                    {
                        return builder.Build(false);
                    }

                    builder.WithSeparator(token);
                    expectValue = true;
                    
                    break;
                }
                default:
                {
                    Internals.Iterator.PushCurrent();

                    if (!expectValue)
                    {
                        return builder.Build(true);
                    }

                    var expressionParser = new ExpressionParser(Internals);
                    var result = expressionParser.Parse();
                    
                    builder.WithTailExpression(result);

                    if (!result.IsComplete)
                    {
                        return builder.Build(false);
                    }
                    
                    expectValue = false;
                    
                    break;
                }
            }
            
            Internals.IteratorTools.SkipWhitespaceAndNewlines();
        }

        return builder.Build(false);
    }
}