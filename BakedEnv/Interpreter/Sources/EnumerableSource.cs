namespace BakedEnv.Interpreter.Sources;

/// <inheritdoc />
public class EnumerableSource : IBakedSource
{
    private IEnumerable<char> Characters { get; }
    
    public EnumerableSource(IEnumerable<char> characters)
    {
        Characters = characters;
    }

    /// <inheritdoc />
    public IEnumerable<char> EnumerateCharacters()
    {
        return Characters;
    }
}