using BakedEnv.Interpreter.Lexer;



namespace BakedEnv.Interpreter.IntermediateTokens.Raw;

public class QuotationToken : RawIntermediateToken
{
    public QuotationToken(TextualToken token) : base(token, 
        TextualTokenType.SingleQuotation, TextualTokenType.DoubleQuotation)
    {
        
    }
}