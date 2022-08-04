using TokenCs;

namespace BakedEnv.Interpreter.IntermediateTokens.Raw;

public class QuotationToken : RawIntermediateToken
{
    public QuotationToken(LexerToken token) : base(token, 
        LexerTokenType.SingleQuotation, LexerTokenType.DoubleQuotation)
    {
        
    }
}