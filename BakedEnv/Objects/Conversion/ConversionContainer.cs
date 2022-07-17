namespace BakedEnv.Objects.Conversion;

public class ConversionContainer : List<IConversionTable>, IConversionTable
{
    public bool TryToObject(BakedObject bakedObject, Type targetType, out object? result)
    {
        result = null;

        foreach (var table in this)
        {
            if (table.TryToObject(bakedObject, targetType, out result))
                return true;
        }

        return false;
    }

    public bool TryToBakedObject(object? o, out BakedObject result)
    {
        result = new BakedNull();
        
        foreach (var table in this)
        {
            if (table.TryToBakedObject(o, out result))
                return true;
        }

        return false;
    }
}