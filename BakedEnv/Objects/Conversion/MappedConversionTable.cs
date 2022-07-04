using System.Numerics;

namespace BakedEnv.Objects.Conversion;

public class MappedConversionTable : ConversionTable
{
    private Dictionary<(Type, Type), Func<BakedObject, object>> BakedConversion { get; }
    private Dictionary<Type, Func<object, BakedObject>> ObjectConversion { get; }

    public MappedConversionTable()
    {
        BakedConversion = new Dictionary<(Type, Type), Func<BakedObject, object>>();
        ObjectConversion = new Dictionary<Type, Func<object, BakedObject>>();

        #region BakedObject Mappings

        // Types smaller than int require double conversion in order to avoid arithmetic overflow
        MapBakedObject<BakedInteger, sbyte>(i => (sbyte)(int)i.Value);
        MapBakedObject<BakedInteger, byte>(i => (byte)(int)i.Value);
        MapBakedObject<BakedInteger, short>(i => (short)(int)i.Value);
        MapBakedObject<BakedInteger, ushort>(i => (ushort)(int)i.Value);
        
        MapBakedObject<BakedInteger, int>(i => (int)i.Value);
        MapBakedObject<BakedInteger, uint>(i => (uint)i.Value);
        MapBakedObject<BakedInteger, long>(i => (long)i.Value);
        MapBakedObject<BakedInteger, ulong>(i => (ulong)i.Value);
        MapBakedObject<BakedInteger, float>(i => (float)i.Value);
        MapBakedObject<BakedInteger, double>(i => (double)i.Value);
        MapBakedObject<BakedInteger, decimal>(i => (decimal)i.Value);
        MapBakedObject<BakedInteger, BigInteger>(i => i.Value);

        MapBakedObject<BakedString, string>(s => s.Value);
        
        MapBakedObject<BakedBoolean, bool>(b => b.Value);

        #endregion

        #region Object Mappings

        MapObject<sbyte>(s => new BakedInteger(s));
        MapObject<byte>(s => new BakedInteger(s));
        MapObject<short>(s => new BakedInteger(s));
        MapObject<ushort>(s => new BakedInteger(s));
        MapObject<int>(s => new BakedInteger(s));
        MapObject<uint>(s => new BakedInteger(s));
        MapObject<long>(s => new BakedInteger(s));
        MapObject<ulong>(s => new BakedInteger(s));
        MapObject<float>(s => new BakedInteger(s));
        MapObject<double>(s => new BakedInteger(s));
        MapObject<decimal>(s => new BakedInteger(s));
        MapObject<BigInteger>(s => new BakedInteger(s));
        
        MapObject<string>(s => new BakedString(s));
        
        MapObject<bool>(s => new BakedBoolean(s));

        #endregion
    }
    
    public void MapBakedObject<TBaked, T>(Func<TBaked, T> converter) where TBaked : BakedObject
    {
        BakedConversion[(typeof(TBaked), typeof(T))] = o => converter.Invoke((TBaked)o)!;
    }
    
    public void MapObject<T>(Func<T, BakedObject> converter)
    {
        ObjectConversion[typeof(T)] = o => converter.Invoke((T)o);
    }
    
    public override object? ToObject(BakedObject bakedObject)
    {
        return bakedObject.GetValue();
    }

    public override object? ToObject(BakedObject bakedObject, Type targetType)
    {
        if (BakedConversion.TryGetValue((bakedObject.GetType(), targetType), out var converter))
        {
            return converter.Invoke(bakedObject);
        }

        return null;
    }

    public override BakedObject ToBakedObject(object? o)
    {
        if (o == null)
            return new BakedNull();
        
        if (ObjectConversion.TryGetValue(o.GetType(), out var converter))
        {
            return converter.Invoke(o);
        }

        return new BakedNull();
    }
}