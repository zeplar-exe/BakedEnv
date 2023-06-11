using System.Reflection;

using BakedEnv.Environment;
using BakedEnv.Environment.ProcessVariables;
using BakedEnv.Extensions;
using BakedEnv.Helpers;
using BakedEnv.Interpreter.Scopes;
using BakedEnv.Objects.Conversion;

namespace BakedEnv.Objects;

public class DelegateObject : BakedObject, IBakedCallable
{
    public static EnvironmentProcessVariable<IConversionTable> DefaultConversionProperty = new();

    public Delegate Delegate { get; }
    public IConversionTable ConversionTable { get; }

    public DelegateObject(Delegate d, BakedEnvironment environment)
    {
        Delegate = d;
        ConversionTable = environment.GetProcessVariable(DefaultConversionProperty) ?? throw new NullReferenceException(
            $"{nameof(DefaultConversionProperty)} is not set on the provided BakedEnvironment.");
    }
    
    public DelegateObject(Delegate d, IConversionTable conversionTable)
    {
        ArgumentNullException.ThrowIfNull(d);
        ArgumentNullException.ThrowIfNull(conversionTable);
        
        Delegate = d;
        ConversionTable = conversionTable;
    }

    public override object? GetValue()
    {
        return Delegate;
    }
    
    public BakedObject Invoke(BakedObject[] parameters, InvocationContext context)
    {
        ArgumentNullException.ThrowIfNull(parameters);
        ArgumentNullException.ThrowIfNull(context);
        
        var delegateParameters = Delegate.Method.GetParameters();
        
        try
        {
            var objectParameters = parameters.Select((p, i) =>
            {
                if (i >= delegateParameters.Length)
                    throw new TargetParameterCountException();

                return ConversionTable.ToObject(p, delegateParameters[i].ParameterType);;
            }).ToArray();
            var result = Delegate.Method.Invoke(Delegate.Target, objectParameters);

            return ConversionTable.ToBakedObject(result);
        }
        catch (Exception e)
        {
            switch (e)
            {
                case ArgumentException:
                    var delegateParametersString = string.Join(", ", 
                        delegateParameters.Select(p => p.ParameterType.Name));
                    
                    context.ReportError(BakedError.EInvocationArgumentMismatch(
                        StringHelper.CreateTypeList(parameters), delegateParametersString, 
                        context.SourceIndex));
                    break;
                case TargetParameterCountException:
                    context.ReportError(BakedError.EInvocationArgumentCountMismatch(
                        delegateParameters.Length,
                        parameters.Length, context.SourceIndex));
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