using BakedEnv.Interpreter.IntermediateParsers;

using TokenCs;

namespace BakedEnv.Interpreter.IntermediateTokens;

public class GuardedLexerToken
{
    private LexerToken Token { get; set; }
    private LexerTokenType[] ExpectedTypes { get; }
    
    public GuardedLexerToken(LexerToken initial, LexerTokenType expectedTypes)
    {
        ExpectedTypes = new[] { expectedTypes };
        
        Set(initial);
    }
    
    public GuardedLexerToken(LexerToken initial, params LexerTokenType[] expectedTypes)
    {
        ExpectedTypes = expectedTypes;
        
        Set(initial);
    }

    public LexerToken Get()
    {
        return Token;
    }

    public void Set(LexerToken token)
    {
        Token = token.AssertIsType(ExpectedTypes);
    }

    public override string ToString()
    {
        return Token.ToString();
    }
}