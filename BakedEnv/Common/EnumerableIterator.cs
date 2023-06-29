using System.Diagnostics.CodeAnalysis;

namespace BakedEnv.Common;

public class EnumerableIterator<T> : IDisposable
{
    private IEnumerator<T> Enumerator { get; }
    private bool ReserveCurrent { get; set; }

    public bool Started { get; private set; }
    public bool Ended { get; private set; }
    
    public T? Current => Enumerator.Current;

    public EnumerableIterator(IEnumerable<T> enumerable)
    {
        Enumerator = enumerable.GetEnumerator();
    }

    public IEnumerable<T> Enumerate()
    {
        while (TryMoveNext(out var next))
        {
            yield return next;
        }
    }
    
    public bool TrySkip()
    {
        return TryMoveNext(out _);
    }

    public virtual bool TryMoveNext(out T next)
    {
        next = default;

        if (ReserveCurrent)
        {
            next = Enumerator.Current;
            ReserveCurrent = false;
            
            return true;
        }

        if (Enumerator.MoveNext())
        {
            Started = true;
            Ended = false;

            next = Enumerator.Current;
        }
        else
        {
            Ended = true;
        }

        return !Ended;
    }
    
    public void Reserve()
    {
        ReserveCurrent = true;
    }

    public void Dispose()
    {
        Enumerator.Dispose();
    }
}