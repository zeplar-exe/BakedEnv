namespace BakedEnv.Interpreter.Sources;

/// <summary>
/// Source used by the interpreter.
/// </summary>
public interface IBakedSource
{
    /// <returns>A "stream" of characters to be interpreted.</returns>
    public IEnumerable<char> EnumerateCharacters();
}