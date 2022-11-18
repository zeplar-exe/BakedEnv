using BakedEnv.Objects;

namespace BakedEnv.Extensions;

internal static class BakedObjectExtensions
{
    public static string TypeName(this BakedObject bakedObject)
    {
        return bakedObject.GetType().Name;
    }
}