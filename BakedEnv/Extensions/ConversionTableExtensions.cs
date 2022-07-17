using BakedEnv.Objects;
using BakedEnv.Objects.Conversion;

namespace BakedEnv.Extensions;

public static class ConversionTableExtensions
{
    public static object? ToObject(this IConversionTable table, BakedObject bakedObject, Type targetType)
    {
        table.TryToObject(bakedObject, targetType, out var result);

        return result;
    }

    public static BakedObject ToBakedObject(this IConversionTable table, object? o)
    {
        table.TryToBakedObject(o, out var result);

        return result;
    }
}