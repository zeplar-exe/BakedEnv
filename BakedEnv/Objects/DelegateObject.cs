using System.Reflection;
using BakedEnv.Interpreter;
using BakedEnv.Interpreter.Scopes;
using BakedEnv.Objects.Conversion;

namespace BakedEnv.Objects;

public class DelegateObject : BakedObject, IBakedCallable
{
    public Delegate Delegate { get; }
    public ConversionTable ConversionTable { get; }

    public DelegateObject(Delegate d) : this(d, new MappedConversionTable()) { }
    
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
            var delegateParameters = Delegate.Method.GetParameters();

            if (delegateParameters.Length > parameters.Length)
            {
                interpreter.ReportError(new BakedError(
                    null,
                    $"Expected {delegateParameters.Length} parameters, got {parameters.Length}.",
                    context.SourceIndex));

                return new BakedNull();
            }

            var trimmedParams = parameters.Take(delegateParameters.Length).ToArray();
            var objectParams = new List<object?>();

            for (var i = 0; i < trimmedParams.Length; i++)
            {
                var param = trimmedParams[i];
                var delegateParam = delegateParameters[i];
                
                objectParams.Add(ConversionTable.ToObject(param, delegateParam.ParameterType));
            }
            
            var result = Delegate.Method.Invoke(Delegate.Target, objectParams.ToArray());

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