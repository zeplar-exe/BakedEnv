namespace BakedEnv.Interpreter.Sources;

/// <summary>
/// A baked source which allows for insertion mid-parse. Useful when debugging.
/// </summary>
public class StackSource : IBakedSource
{
    private Stack<string> StringStack { get; }

    public StackSource(params string[] init)
    {
        StringStack = new Stack<string>();
        
        foreach (var item in init)
        {
            StringStack.Push(item);
        }
    }

    /// <summary>
    /// Push an item to the internal stack.
    /// </summary>
    /// <param name="item">Item to push.</param>
    public void Push(string item) => StringStack.Push(item);

    /// <inheritdoc />
    public IEnumerable<char> EnumerateCharacters()
    {
        while (StringStack.TryPop(out var item))
        {
            foreach (var character in item)
            {
                yield return character;
            }
        }
    }
}