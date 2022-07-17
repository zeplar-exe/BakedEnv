namespace BakedEnv.Objects.Conversion;

public interface IConversionTable
{
    public bool TryToObject(BakedObject bakedObject, Type targetType, out object? result);
    public bool TryToBakedObject(object? o, out BakedObject result);
}