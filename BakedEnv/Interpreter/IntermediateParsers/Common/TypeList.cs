namespace BakedEnv.Interpreter.IntermediateParsers.Common;

internal class TypeList<T> : List<T>
{
    public T2 Add<T2>() where T2 : T, new()
    {
        var item = new T2();
        
        base.Add(item);

        return item;
    }

    public int RemoveAll<T2>() where T2 : T, new()
    {
        return base.RemoveAll(i => i is T2);
    }
}