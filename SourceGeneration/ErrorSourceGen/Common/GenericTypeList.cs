using System.Collections;

namespace ErrorSourceGen.Common;

public class GenericTypeList<TInitial> : IEnumerable<TInitial>
{
    private HashSet<Type> Types { get; }
    private List<object> Objects { get; }

    public GenericTypeList()
    {
        Types = new HashSet<Type>();
        Objects = new List<object>();
    }

    public void Add<T>() where T : TInitial, new()
    {
        Types.Add(typeof(T));
        Objects.Add(new T());
    }

    public T Get<T>() where T : TInitial
    {
        if (!Types.Contains(typeof(T)))
        {
            throw new InvalidOperationException("This type does not exist within the list.");
        }

        return (T)Objects.Find(o => o.GetType() == typeof(T));
    }

    public IEnumerator<TInitial> GetEnumerator()
    {
        return Objects.Cast<TInitial>().GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}