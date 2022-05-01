using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using BakedEnv.Objects;

namespace BakedEnv.ObjectCreation;

public static class BakedObjectHelper
{
    public static List<IObjectCreator> RegisteredObjectCreators { get; }

    static BakedObjectHelper()
    {
        RegisteredObjectCreators = new List<IObjectCreator> { new DefaultObjectCreator() };
    }

    public static bool TryGetObject(object? o, [NotNullWhen(true)] out BakedObject? bakedObject)
    {
        bakedObject = null;
        
        foreach (var creator in RegisteredObjectCreators)
        {
            if (creator.TryCreate(o, out bakedObject))
                return true;
        }

        return false;
    }
}