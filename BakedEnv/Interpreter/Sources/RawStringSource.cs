namespace BakedEnv.Interpreter.Sources;

/// <summary>
/// A baked source which enumerates a raw string.
/// </summary>
public class RawStringSource : IBakedSource
{
    private string String { get; }

    /// <summary>
    /// Initialize a RawStringSource with a string.
    /// </summary>
    /// <param name="value"></param>
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