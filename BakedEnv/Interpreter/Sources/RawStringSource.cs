namespace BakedEnv.Interpreter.Sources;

public class RawStringSource : IBakedSource
{
    private string String { get; }

    public RawStringSource(string s)
    {
        String = s;
    }

    public IEnumerable<char> EnumerateCharacters()
    {
        return String;
    }
}