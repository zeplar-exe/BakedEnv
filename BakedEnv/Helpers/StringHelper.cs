namespace BakedEnv.Helpers;

internal static class StringHelper
{
    public static string CreateTypeList(IEnumerable<object> objects)
    {
        return CreateStringList(objects.Select(o => o.GetType().Name));
    }
    
    public static string CreateEnumList<T>(IEnumerable<T> objects) where T : struct, Enum
    {
        return CreateStringList(objects.Select(Enum.GetName));
    }

    public static string CreateStringList(IEnumerable<string> strings)
    {
        return string.Join(", ", strings);
    }
}