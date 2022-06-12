namespace BakedEnv.Interpreter.Sources;

/// <summary>
/// A baked source which uses a raw character <see cref="IEnumerable{T}"/> .
/// </summary>
public class EnumerableSource : IBakedSource
{
    private IEnumerable<char> Characters { get; }
    
    /// <summary>
    /// Create an EnumerableSource with its characters.
    /// </summary>
    /// <param name="characters">Character enumerable used in <see cref="EnumerateCharacters"/>.</param>
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