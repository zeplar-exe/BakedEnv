using BakedEnv.Interpreter.IntermediateTokens;
using BakedEnv.Interpreter.Lexer;

namespace BakedEnv.Interpreter.IntermediateParsers.Common;

public abstract class MatchParser : ParserBase
{
    public abstract bool Match(TextualToken first);
    public abstract IntermediateToken Parse(TextualToken first, LexerIterator iterator);
    
    protected bool TestTokenIs(TextualToken token, params TextualTokenType[] types)
    {
        return types.Any(t => token.Type == t);
    }
}