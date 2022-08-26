namespace BakedEnv.Interpreter.Lexer;

public enum TextualTokenType
{
    Unhandled,
    
    Alphabetic,
    AlphaNumeric,
    Numeric,

    Space, Tab,
    CarriageReturn,
    LineFeed,
    CarriageReturnLineFeed,
}