using BakedEnv.Interpreter;

namespace BakedEnv.Objects;

public class BakedVoid : BakedObject
{
    public override object? GetValue()
    {
        return null;
    }

    public override bool TryInvoke(BakedInterpreter interpreter, IBakedScope scope, out BakedObject? returnValue)
    {
        returnValue = null;
        
        return false;
    }

    public override bool TryGetContainedObject(string name, out BakedObject? bakedObject)
    {
        bakedObject = null;
        
        return false;
    }

    public override bool TrySetContainedObject(string name, BakedObject? bakedObject)
    {
        bakedObject = null;
        
        return false;
    }

    public override bool Equals(object? obj)
    {
        return obj is BakedVoid;
    }

    public override bool Equals(BakedObject? other)
    {
        return other is BakedVoid;
    }

    public override string ToString()
    {
        return "void";
    }
}