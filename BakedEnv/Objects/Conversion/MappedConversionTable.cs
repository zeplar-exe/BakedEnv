using System.Numerics;

namespace BakedEnv.Objects.Conversion;

public class MappedConversionTable : IConversionTable
{
    private Dictionary<(Type, Type), Func<BakedObject, object>> BakedConversion { get; }
    private Dictionary<Type, Func<object, BakedObject>> ObjectConversion { get; }

    public MappedConversionTable()
    {
        BakedConversion = new Dictionary<(Type, Type), Func<BakedObject, object>>();
        ObjectConversion = new Dictionary<Type, Func<object, BakedObject>>();
    }

    public static MappedConversionTable Primitive()
    {
        var table = new MappedConversionTable();
        
        #region BakedObject Mappings

        // Types smaller than int require double conversion in order to avoid arithmetic overflow
        table.MapBakedObject<BakedInteger, sbyte>(i => (sbyte)(int)i.Value);
        table.MapBakedObject<BakedInteger, byte>(i => (byte)(int)i.Value);
        table.MapBakedObject<BakedInteger, short>(i => (short)(int)i.Value);
        table.MapBakedObject<BakedInteger, ushort>(i => (ushort)(int)i.Value);
        
        table.MapBakedObject<BakedInteger, int>(i => (int)i.Value);
        table.MapBakedObject<BakedInteger, uint>(i => (uint)i.Value);
        table.MapBakedObject<BakedInteger, long>(i => (long)i.Value);
        table.MapBakedObject<BakedInteger, ulong>(i => (ulong)i.Value);
        table.MapBakedObject<BakedInteger, float>(i => (float)i.Value);
        table.MapBakedObject<BakedInteger, double>(i => (double)i.Value);
        table.MapBakedObject<BakedInteger, decimal>(i => (decimal)i.Value);
        table.MapBakedObject<BakedInteger, BigInteger>(i => i.Value);

        table.MapBakedObject<BakedString, string>(s => s.Value);
        
        table.MapBakedObject<BakedBoolean, bool>(b => b.Value);

        #endregion

        #region Object Mappings

        table.MapObject<sbyte>(s => new BakedInteger(s));
        table.MapObject<byte>(s => new BakedInteger(s));
        table.MapObject<short>(s => new BakedInteger(s));
        table.MapObject<ushort>(s => new BakedInteger(s));
        table.MapObject<int>(s => new BakedInteger(s));
        table.MapObject<uint>(s => new BakedInteger(s));
        table.MapObject<long>(s => new BakedInteger(s));
        table.MapObject<ulong>(s => new BakedInteger(s));
        table.MapObject<float>(s => new BakedInteger(s));
        table.MapObject<double>(s => new BakedInteger(s));
        table.MapObject<decimal>(s => new BakedInteger(s));
        table.MapObject<BigInteger>(s => new BakedInteger(s));
        
        table.MapObject<string>(s => new BakedString(s));
        
        table.MapObject<bool>(s => new BakedBoolean(s));

        #endregion

        return table;
    }
    
    public void MapBakedObject<TBaked, T>(Func<TBaked, T> converter) where TBaked : BakedObject
    {
        BakedConversion[(typeof(TBaked), typeof(T))] = o => converter.Invoke((TBaked)o)!;
    }
    
    public void MapObject<T>(Func<T, BakedObject> converter)
    {
        ObjectConversion[typeof(T)] = o => converter.Invoke((T)o);
    }

    public bool TryToObject(BakedObject bakedObject, Type targetType, out object? result)
    {
        result = null;
        
        if (BakedConversion.TryGetValue((bakedObject.GetType(), targetType), out var converter))
        {
            result = converter.Invoke(bakedObject);

            return true;
        }

        return false;
    }

    public bool TryToBakedObject(object? o, out BakedObject result)
    {
        result = new BakedNull();
        
        if (o == null)
            return true;
        
        if (ObjectConversion.TryGetValue(o.GetType(), out var converter))
        {
            result = converter.Invoke(o);

            return true;
        }

        return false;
    }
}