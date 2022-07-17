namespace BakedEnv.Objects;

public class BakedTable : BakedObject
{
    private Dictionary<BakedObject, BakedObject> Dictionary { get; }

    public int Length => Dictionary.Count;
    public IEnumerable<BakedObject> Keys => Dictionary.Keys;
    public IEnumerable<BakedObject> Values => Dictionary.Values;

    public BakedTable()
    {
        Dictionary = new Dictionary<BakedObject, BakedObject>();
    }

    public BakedObject this[BakedObject bakedObject]
    {
        get
        {
            if (Dictionary.TryGetValue(bakedObject, out var value))
                return value!;

            return new BakedNull();
        }
        set => Dictionary[bakedObject] = value;
    }

    public override object GetValue()
    {
        return Dictionary;
    }

    public override bool TryGetContainedObject(string name, out BakedObject bakedObject)
    {
        bakedObject = new BakedNull();
        
        foreach (var entry in Dictionary)
        {
            if (entry.Key.Equals(name))
            {
                bakedObject = entry.Value;

                return true;
            }
        }

        return false;
    }

    public override bool TrySetContainedObject(string name, BakedObject bakedObject)
    {
        Dictionary[new BakedString(name)] = bakedObject;

        return true;
    }

    public override bool TryGetIndex(BakedObject[] key, out BakedObject bakedObject)
    {
        bakedObject = new BakedNull();
        
        if (key.Length > 0)
        {
            return false;
        }
        
        bakedObject = this[key[0]];

        return true;
    }
    
    public override bool TrySetIndex(BakedObject[] key, BakedObject value)
    {
        if (key.Length > 0)
        {
            return false;
        }
        
        this[key[0]] = value;

        return true;
    }

    public override int GetHashCode()
    {
        return Dictionary.GetHashCode();
    }

    public override string? ToString()
    {
        return Dictionary.ToString();
    }
}