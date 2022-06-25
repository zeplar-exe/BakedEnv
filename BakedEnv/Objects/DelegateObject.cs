using BakedEnv.Interpreter;
using BakedEnv.Objects.Conversion;

namespace BakedEnv.Objects;

public class DelegateObject : BakedObject, IBakedCallable
{
    public Delegate Delegate { get; }
    public ConversionTable ConversionTable { get; }

    public DelegateObject(Delegate d) : this(d, new PrimitiveConversionTable()) { }
    
    public DelegateObject(Delegate d, ConversionTable conversionTable)
    {
        Delegate = d;
        ConversionTable = conversionTable;
    }

    public override object? GetValue()
    {
        return Delegate;
    }
    
    public BakedObject Invoke(BakedObject[] parameters, BakedInterpreter interpreter, IBakedScope scope)
    {
        var objectParameters = parameters.Select(p => ConversionTable.ToObject(p)).ToArray();
        var result = Delegate.Method.Invoke(Delegate.Target, objectParameters);

        return ConversionTable.ToBakedObject(result);
    }

    public override int GetHashCode()
    {
        return Delegate.GetHashCode();
    }

    public override string? ToString()
    {
        return Delegate.ToString();
    }
}