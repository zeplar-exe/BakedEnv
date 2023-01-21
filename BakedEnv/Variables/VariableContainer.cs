using System.Collections;
using System.Diagnostics.CodeAnalysis;

using BakedEnv.Objects;

namespace BakedEnv.Variables;

public class VariableContainer : IEnumerable<IBakedVariable>
{
    private Dictionary<string, IBakedVariable> Variables { get; }
    
    public int Count => Variables.Count;
    public bool IsReadOnly => false;

    public event EventHandler<IBakedVariable>? VariableAdded;
    public event EventHandler<IBakedVariable>? VariableRemoved;
    public event EventHandler<IBakedVariable>? VariableAccessed;
    public event EventHandler<IBakedVariable>? VariableChanged;

    public VariableContainer()
    {
        Variables = new Dictionary<string, IBakedVariable>();
    }

    public IBakedVariable this[string key] => Variables[key];

    public void Add(string name, BakedObject value)
    {
        Add(new BakedVariable(name, value));
    }
    
    public void Add(IBakedVariable item)
    {
        Variables[item.Name] = item;
        VariableAdded?.Invoke(this, item);

        item.ValueChanged += (variable, _) => VariableChanged?.Invoke(this, variable);
    }
    
    public bool Contains(string name)
    {
        return Variables.ContainsKey(name);
    }

    public bool Remove(string name)
    {
        if (Variables.TryGetValue(name, out var variable))
        {
            Variables.Remove(name);
            VariableRemoved?.Invoke(this, variable);
            
            return true;
        }

        return false;
    }

    public bool TryGetValue(string key, [NotNullWhen(true)] out IBakedVariable? variable)
    {
        if (Variables.TryGetValue(key, out variable))
        {
            VariableAccessed?.Invoke(this, variable);
            
            return true;
        }

        variable = null;

        return false;
    }

    public void Clear()
    {
        foreach (var key in Variables.Keys.ToArray())
        {
            Remove(key);
        }
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