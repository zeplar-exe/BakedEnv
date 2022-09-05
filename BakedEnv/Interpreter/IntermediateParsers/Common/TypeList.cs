using System.Collections;

namespace BakedEnv.Interpreter.IntermediateParsers.Common;

public class TypeList<T> : IEnumerable<Type>
{
    private HashSet<TypePair> Types { get; }

    public delegate T TypeCreator(Type type);

    public TypeList()
    {
        Types = new HashSet<TypePair>();
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
        {
            throw new ArgumentException($"Type ({type.Name}) must be assignable to {typeof(T)}.");
        }
        
        return Types.Add(new TypePair(type, creator));
    }

    public bool Remove<T2>() where T2 : T
    {
        return Remove(typeof(T2));
    }

    public bool Remove(Type type)
    {
        var pair = new TypePair(type, _ => throw new Exception("This delegate should never be invoked."));

        return Types.Remove(pair);
    }

    public IEnumerable<T> EnumerateInstances()
    {
        foreach (var pair in Types)
        {
            yield return pair.Creator.Invoke(pair.Type);
        }
    }
    
    public IEnumerator<Type> GetEnumerator()
    {
        return Types.Select(t => t.Type).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    private readonly struct TypePair
    {
        public Type Type { get; }
        public TypeCreator Creator { get; }

        public TypePair(Type type, TypeCreator creator)
        {
            Type = type;
            Creator = creator;
        }

        public override int GetHashCode()
        {
            return Type.GetHashCode();
        }
    }
}