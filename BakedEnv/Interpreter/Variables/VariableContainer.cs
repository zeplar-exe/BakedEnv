using System.Collections;
using System.Diagnostics.CodeAnalysis;
using BakedEnv.Objects;

namespace BakedEnv.Interpreter.Variables;

public class VariableContainer : ICollection<BakedVariable>
{
    private Dictionary<string, BakedVariable> Variables { get; }
    
    public int Count => Variables.Count;
    public bool IsReadOnly => false;

    public VariableContainer()
    {
        Variables = new Dictionary<string, BakedVariable>();
    }

    public BakedVariable this[string key] => Variables[key];

    public void Add(string name, BakedObject value)
    {
        Variables[name] = new BakedVariable(name, value);
    }
    
    public void Add(BakedVariable item)
    {
        Variables[item.Name] = item;
    }

    public bool TryGetValue(string key, [NotNullWhen(true)] out BakedVariable? variable)
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

    public bool Contains(BakedVariable item)
    {
        return Variables.ContainsKey(item.Name);
    }

    public void CopyTo(BakedVariable[] array, int arrayIndex)
    {
        Variables.Values.CopyTo(array, arrayIndex);
    }

    public bool Remove(BakedVariable item)
    {
        return Variables.Remove(item.Name);
    }
    
    public IEnumerator<BakedVariable> GetEnumerator()
    {
        return Variables.Values.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}