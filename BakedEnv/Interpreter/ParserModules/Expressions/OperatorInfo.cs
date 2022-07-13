using BakedEnv.Interpreter.Expressions;
using BakedEnv.Interpreter.Expressions.Arithmetic;
using TokenCs;

namespace BakedEnv.Interpreter.ParserModules.Expressions;

internal record OperatorInfo(LexerToken Token, TailExpressionParserResult Left, TailExpressionParserResult Right)
{
    public IEnumerable<LexerToken> AllTokens
    {
        get
        {
            foreach (var token in Left.AllTokens)
            {
                yield return token;
            }

            yield return Token;
            
            foreach (var token in Right.AllTokens)
            {
                yield return token;
            }
        }
    }
}