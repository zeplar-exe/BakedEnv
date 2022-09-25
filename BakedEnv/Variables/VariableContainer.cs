using System.Collections;
using System.Diagnostics.CodeAnalysis;

using BakedEnv.Objects;

namespace BakedEnv.Variables;

public class VariableContainer : ICollection<IBakedVariable>
{
    private Dictionary<string, IBakedVariable> Variables { get; }
    
    public int Count => Variables.Count;
    public bool IsReadOnly => false;

    public VariableContainer()
    {
        Variables = new Dictionary<string, IBakedVariable>();
    }

    public IBakedVariable this[string key] => Variables[key];

    public void Add(string name, BakedObject value)
    {
        Variables[name] = new BakedVariable(name, value);
    }
    
    public void Add(IBakedVariable item)
    {
        Variables[item.Name] = item;
    }

    public bool TryGetValue(string key, [NotNullWhen(true)] out IBakedVariable? variable)
    {
        if (Variables.TryGetValue(key, out variable))
            return true;

        variable = null;

        return false;
    }

    public void Clear()
    {
        Variables.Clear();
    }

    public bool Contains(IBakedVariable item)
    {
        return Variables.ContainsKey(item.Name);
    }

    public void CopyTo(IBakedVariable[] array, int arrayIndex)
    {
        Variables.Values.CopyTo(array, arrayIndex);
    }

    public bool Remove(IBakedVariable item)
    {
        return Variables.Remove(item.Name);
    }
    
    public IEnumerator<IBakedVariable> GetEnumerator()
    {
        return Variables.Values.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}