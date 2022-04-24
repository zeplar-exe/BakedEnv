namespace BakedEnv.Interpreter.Sources;

/// <inheritdoc />
public class RawStringSource : IBakedSource
{
    private string String { get; }

    public RawStringSource(string s)
    {
        String = s;
    }

    /// <inheritdoc />
    public IEnumerable<char> EnumerateCharacters()
    {
        return String;
    }
}