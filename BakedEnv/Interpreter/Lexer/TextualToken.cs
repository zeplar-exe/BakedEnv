namespace BakedEnv.Interpreter.Lexer;

public class TextualToken : ILowLevelToken
{
    public string Text { get; }
    public TextualTokenType Type { get; }
    public long StartIndex { get; }
    public long Length => Text.Length;
    public long EndIndex => StartIndex + Length;

    public TextualToken(string text, TextualTokenType type, long index)
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