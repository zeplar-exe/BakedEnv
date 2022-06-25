namespace BakedEnv.Objects.Conversion;

public class PrimitiveConversionTable : ConversionTable
{
    public override object? ToObject(BakedObject bakedObject)
    {
        return bakedObject.GetValue();
    }

    public override BakedObject ToBakedObject(object? o)
    {
        switch (o)
        {
            case int i:
                return new BakedInteger(i);
            case byte b:
                return new BakedInteger(b);
            case uint ui:
                return new BakedInteger(ui);
            case float f:
                return new BakedInteger(f);
            case double d:
                return new BakedInteger(d);
            case decimal e:
                return new BakedInteger(e);
            case short sh:
                return new BakedInteger(sh);
            case ushort us:
                return new BakedInteger(us);
            case long l:
                return new BakedInteger(l);
            case ulong ul:
                return new BakedInteger(ul);
            case string s:
                return new BakedString(s);
            case bool b:
                return new BakedBoolean(b);
            case null:
                return new BakedNull();
        }

        return new BakedNull();
    }
}