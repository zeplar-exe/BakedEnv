namespace BakedEnv.Interpreter.Sources;

/// <summary>
/// Source used by the interpreter.
/// </summary>
public interface IBakedSource
{
    /// <returns>Characters to be interpreted.</returns>
    public IEnumerable<char> EnumerateCharacters();
}