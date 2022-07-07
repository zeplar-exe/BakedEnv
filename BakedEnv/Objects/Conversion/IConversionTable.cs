namespace BakedEnv.Objects.Conversion;

public interface IConversionTable
{
    public object? ToObject(BakedObject bakedObject);
    public object? ToObject(BakedObject bakedObject, Type targetType);
    public BakedObject ToBakedObject(object? o);
}