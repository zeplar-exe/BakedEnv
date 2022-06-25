using System.Reflection;
using BakedEnv.Interpreter;
using BakedEnv.Interpreter.Scopes;
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
    
    public BakedObject Invoke(BakedObject[] parameters, BakedInterpreter interpreter, InvocationContext context)
    {
        try
        {
            var objectParameters = parameters.Select(p => ConversionTable.ToObject(p)).ToArray();
            var result = Delegate.Method.Invoke(Delegate.Target, objectParameters);

            return ConversionTable.ToBakedObject(result);
        }
        catch (Exception e)
        {
            switch (e)
            {
                case ArgumentException args:
                    interpreter.ReportError(new BakedError(
                        null,
                        "Invalid arguments.",
                        context.SourceIndex));
                    break;
                case TargetParameterCountException paramCount:
                    interpreter.ReportError(new BakedError(
                        null,
                        "Expected {} parameters, got {}.",
                        context.SourceIndex));
                    break;
                default:
                    throw;
            }
        }

        return new BakedNull();
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