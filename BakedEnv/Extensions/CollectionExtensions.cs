[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("BakedEnv.GeneralTests")] 
namespace BakedEnv.Extensions;

internal static class CollectionExtensions
{
    public static TDistinct GetOrAddByType<TRoot, TDistinct>(this ICollection<TRoot> collection, TDistinct distinct) 
        where TDistinct : TRoot
    {
        return GetOrAddByType(collection, () => distinct);
    }
    
    public static TDistinct GetOrAddByType<TRoot, TDistinct>(this ICollection<TRoot> collection, Func<TDistinct> creator) 
        where TDistinct : TRoot
    {
        var result = collection.OfType<TDistinct>().FirstOrDefault();

        if (result == null)
        {
            result = creator.Invoke();
            
            collection.Add(result);
        }

        return result;
    }
}