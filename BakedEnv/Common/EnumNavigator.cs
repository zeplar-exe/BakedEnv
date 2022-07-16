namespace BakedEnv.Common;

public class EnumNavigator<T> where T : Enum
{
    private Stack<T> Stack { get; }

    public T? Current => Stack.TryPeek(out var current) ? current : default;
    
    public EnumNavigator()
    {
        Stack = new Stack<T>();
    }
    
    public EnumNavigator(T start)
    {
        Stack = new Stack<T>();
        Stack.Push(start);
    }

    public void MoveTo(T item)
    {
        Stack.Push(item);
    }

    public void MoveLast()
    {
        Stack.TryPop(out _);
    }
}