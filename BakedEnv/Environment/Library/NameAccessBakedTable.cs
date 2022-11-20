using BakedEnv.Objects;

namespace BakedEnv.Environment.Library;

public class NameAccessBakedTable : BakedTable
{
    public bool Mutable { get; set; }
    
    public override bool TryGetContainedObject(string name, out BakedObject bakedObject)
    {
        return TryGetIndex(new BakedObject[] { new BakedString(name) }, out bakedObject);
    }

    public override bool TrySetContainedObject(string name, BakedObject bakedObject)
    {
        return TrySetIndex(new BakedObject[] { new BakedString(name) }, bakedObject);
    }

    public override bool TrySetIndex(BakedObject[] key, BakedObject value)
    {
        return Mutable && base.TrySetIndex(key, value);
    }
}