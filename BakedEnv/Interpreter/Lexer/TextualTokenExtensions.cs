namespace BakedEnv.Interpreter.Lexer;

public static class TextualTokenExtensions
{
    public static bool IsNewline(this TextualToken token)
    {
        return token.Type is 
            TextualTokenType.CarriageReturn or 
            TextualTokenType.LineFeed or 
            TextualTokenType.CarriageReturnLineFeed;
    }
}