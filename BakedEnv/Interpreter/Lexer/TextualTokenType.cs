namespace BakedEnv.Interpreter.Lexer;

public enum TextualTokenType
{
    Unhandled = 0,
    
    Alphabetic,
    Numeric,
    
    Tilde,
    Backtick,
    ExclamationMark,
    At,
    Hashtag,
    Dollar,
    Percent,
    Caret,
    Ampersand,
    Star,
    LeftParenthesis,
    RightParenthesis,
    Underscore,
    Dash,
    Plus,
    Equals,
    LeftCurlyBracket,
    RightCurlyBracket,
    LeftBracket,
    RightBracket,
    Vertical,
    Backslash,
    Colon,
    Semicolon,
    DoubleQuotation,
    SingleQuotation,
    LessThan,
    GreaterThan,
    Comma,
    Period,
    QuestionMark,
    Slash,

    Space, Tab,
    CarriageReturn,
    LineFeed,
    CarriageReturnLineFeed,
}