namespace BakedEnv.Interpreter.Lexer;

public class TextualToken
{
    public string Text { get; }
    public TextualTokenType Type { get; }
    public ulong StartIndex { get; }
    public int Length => Text.Length;
    public ulong EndIndex => StartIndex + (ulong)Length;

    public TextualToken(string text, TextualTokenType type, ulong index)
    {
        Text = text;
        Type = type;
        StartIndex = index;
    }

    public override string ToString()
    {
        return Text;
    }
}