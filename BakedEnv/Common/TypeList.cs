using System.Collections;

namespace BakedEnv.Common;

public class TypeList<T> : IEnumerable<Type>
{
    private HashSet<Type> Types { get; }

    public TypeList()
    {
        Types = new HashSet<Type>();
    }
    
    public void Add<T2>() where T2 : T
    {
        Types.Add(typeof(T2));
    }
    
    public void Add(Type type)
    {
        Types.Add(type);
    }

    public void AddFrom(TypeList<T> other)
    {
        foreach (var type in other.Types)
        {
            Add(type);
        }
    }

    public bool Remove<T2>() where T2 : T
    {
        return Remove(typeof(T2));
    }

    public bool Remove(Type type)
    {
        if (!Types.Contains(type))
            return false;
        
        return Types.Remove(type);
    }
    
    public IEnumerator<Type> GetEnumerator()
    {
        return Types.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}