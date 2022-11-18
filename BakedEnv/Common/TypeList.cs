using System.Collections;

namespace BakedEnv.Common;

public class TypeList<T> : IEnumerable<Type>
{
    private Dictionary<int, TypePair> Types { get; }

    public delegate T TypeCreator(Type type);

    public TypeList()
    {
        Types = new Dictionary<int, TypePair>();
    }
    
    public bool Add<T2>() where T2 : T, new()
    {
        return Add(typeof(T2), _ => new T2());
    }
    
    public bool Add<T2>(TypeCreator creator) where T2 : T
    {
        return Add(typeof(T2), creator);
    }
    
    public bool Add(Type type)
    {
        return Add(type, t => (T)Activator.CreateInstance(t));
    }

    public bool Add(Type type, TypeCreator creator)
    {
        if (!type.IsAssignableTo(typeof(T)))
            throw new ArgumentException($"Type ({type.Name}) must be assignable to {typeof(T)}.");

        if (Types.ContainsKey(type.GetHashCode()))
            return false;

        Types[type.GetHashCode()] = new TypePair(type, creator);

        return true;
    }

    public bool Remove<T2>() where T2 : T
    {
        return Remove(typeof(T2));
    }

    public bool Remove(Type type)
    {
        if (!Types.ContainsKey(type.GetHashCode()))
            return false;
        
        return Types.Remove(type.GetHashCode());
    }

    public IEnumerable<T> EnumerateInstances()
    {
        foreach (var (hash, pair) in Types)
        {
            yield return pair.Creator.Invoke(pair.Type);
        }
    }
    
    public IEnumerator<Type> GetEnumerator()
    {
        return Types.Select(item => item.Value.Type).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    private readonly record struct TypePair(Type Type, TypeCreator Creator);
}