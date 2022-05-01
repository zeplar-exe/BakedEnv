using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using BakedEnv.Objects;

namespace BakedEnv.ObjectCreation;

/// <inheritdoc />
public class DefaultObjectCreator : IObjectCreator
{
    /// <inheritdoc />
    public bool TryCreate(object? o, [NotNullWhen(true)] out BakedObject? bakedObject)
    {
        bakedObject = null;
        
        switch (o)
        {
            case byte value: bakedObject = new BakedInteger(value); break;
            case sbyte value: bakedObject = new BakedInteger(value); break;
            case ushort value: bakedObject = new BakedInteger(value); break;
            case short value: bakedObject = new BakedInteger(value); break;
            case uint value: bakedObject = new BakedInteger(value); break;
            case int value: bakedObject = new BakedInteger(value); break;
            case ulong value: bakedObject = new BakedInteger(value); break;
            case long value: bakedObject = new BakedInteger(value); break;
            case float value: bakedObject = new BakedInteger(value); break;
            case double value: bakedObject = new BakedInteger(value); break;
            case decimal value: bakedObject = new BakedInteger(value); break;
            case BigInteger value: bakedObject = new BakedInteger(value); break;
            case string value: bakedObject = new BakedString(value); break;
            case bool value: bakedObject = new BakedBoolean(value); break;
            default: return false;
        }

        return true;
    }
}