namespace BakedEnv.Objects.Conversion;

public abstract class ConversionTable
{
    public abstract object? ToObject(BakedObject bakedObject);
    public abstract object? ToObject(BakedObject bakedObject, Type targetType);
    public abstract BakedObject ToBakedObject(object? o);
}