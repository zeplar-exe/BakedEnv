namespace BakedEnv.Interpreter.Sources;

/// <summary>
/// A baked source which enumerates a raw string.
/// </summary>
public class RawStringSource : IBakedSource
{
    private string String { get; }

    public RawStringSource(string value)
    {
        ArgumentNullException.ThrowIfNull(value); 
        
        String = value;
    }

    /// <inheritdoc />
    public IEnumerable<char> EnumerateCharacters()
    {
        return String;
    }
}