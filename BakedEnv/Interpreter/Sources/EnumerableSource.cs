namespace BakedEnv.Interpreter.Sources;

/// <summary>
/// A baked source which uses a raw character <see cref="IEnumerable"/> .
/// </summary>
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