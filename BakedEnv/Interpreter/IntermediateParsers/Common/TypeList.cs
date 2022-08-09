namespace BakedEnv.Interpreter.IntermediateParsers.Common;

public class TypeList<T> : List<T>
{
    public T2 Add<T2>() where T2 : T, new()
    {
        var item = new T2();
        
        base.Add(item);

        return item;
    }
}