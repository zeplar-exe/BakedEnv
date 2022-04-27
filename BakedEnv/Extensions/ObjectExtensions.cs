namespace BakedEnv.Extensions;

internal static class ObjectExtensions
{
    public static bool TryGetAs<T>(this object o, out T? t, T? defaultValue = default)
    {
        t = defaultValue;

        if (o is T value)
            t = value;
        else
            return false;

        return true;
    }

    public static bool IsNumeric(this object o)
    {
        return o is sbyte or byte or ushort or short or uint or int or ulong or long or float or double or decimal or nint or nuint;
    }
    
    public static bool IsWhole(this object o)
    {
        return o is sbyte or byte or ushort or short or uint or int or ulong or long or nint or nuint;
    }
    
    public static bool IsFloatingPoint(this object o)
    {
        return o is float or double or decimal;
    }
}