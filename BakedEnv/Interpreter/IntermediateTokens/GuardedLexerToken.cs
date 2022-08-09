using BakedEnv.Interpreter.IntermediateParsers;
using BakedEnv.Interpreter.IntermediateParsers.Common;

using TokenCs;

namespace BakedEnv.Interpreter.IntermediateTokens;

public class GuardedLexerToken
{
    private LexerToken Token { get; set; }
    private LexerTokenType[] ExpectedTypes { get; }
    
    public GuardedLexerToken(LexerToken initial, LexerTokenType expectedTypes)
    {
        Token = initial;
        ExpectedTypes = new[] { expectedTypes };
    }
    
    public GuardedLexerToken(LexerToken initial, params LexerTokenType[] expectedTypes)
    {
        Token = initial;
        ExpectedTypes = expectedTypes;
    }

    public LexerToken Get()
    {
        return Token;
    }

    public void Set(LexerToken token)
    {
        if (ExpectedTypes.Length == 0)
        {
            Token = token;
            
            return;
        }
        
        Token = token.AssertIsType(ExpectedTypes);
    }

    public override string ToString()
    {
        return Token.ToString();
    }
}