using BakedEnv.Interpreter.IntermediateParsers;
using BakedEnv.Interpreter.IntermediateParsers.Common;
using BakedEnv.Interpreter.Lexer;



namespace BakedEnv.Interpreter.IntermediateTokens;

public class GuardedLexerToken
{
    private TextualToken Token { get; set; }
    private TextualTokenType[] ExpectedTypes { get; }
    
    public GuardedLexerToken(TextualToken initial, TextualTokenType expectedTypes)
    {
        Token = initial;
        ExpectedTypes = new[] { expectedTypes };
    }
    
    public GuardedLexerToken(TextualToken initial, params TextualTokenType[] expectedTypes)
    {
        Token = initial;
        ExpectedTypes = expectedTypes;
    }

    public TextualToken Get()
    {
        return Token;
    }

    public void Set(TextualToken token)
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