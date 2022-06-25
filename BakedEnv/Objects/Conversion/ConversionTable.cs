namespace BakedEnv.Objects.Conversion;

public abstract class ConversionTable
{
    public abstract object? ToObject(BakedObject bakedObject);
    public abstract BakedObject ToBakedObject(object? o);
}