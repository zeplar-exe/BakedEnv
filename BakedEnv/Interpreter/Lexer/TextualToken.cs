namespace BakedEnv.Interpreter.Lexer;

public class TextualToken
{
    public string Text { get; }
    public TextualTokenType Type { get; }
    public ulong Index { get; }
    public int Length => Text.Length;

    public TextualToken(string text, TextualTokenType type, ulong index)
    {
        Text = text;
        Type = type;
        Index = index;
    }
}