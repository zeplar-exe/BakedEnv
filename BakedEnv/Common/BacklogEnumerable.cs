using System.Collections;

namespace BakedEnv.Common;

public class BacklogEnumerable<T> : IEnumerable<T>
{
    private IEnumerable<T> Source { get; }
    private Stack<T> Backlog { get; }

    public BacklogEnumerable(IEnumerable<T> source)
    {
        Source = source;
        Backlog = new Stack<T>();
    }

    public void Push(T item)
    {
        Backlog.Push(item);
    }

    public IEnumerator<T> GetEnumerator()
    {
        while (Backlog.TryPop(out var backlogItem))
        {
            yield return backlogItem;
        }
        
        foreach (var item in Source)
        {
            while (Backlog.TryPop(out var backlogItem))
            {
                yield return backlogItem;
            }

            yield return item;
        }
        
        while (Backlog.TryPop(out var backlogItem))
        {
            yield return backlogItem;
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}