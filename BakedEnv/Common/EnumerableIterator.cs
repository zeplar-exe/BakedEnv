using System.Diagnostics.CodeAnalysis;

namespace BakedEnv.Common;

internal class EnumerableIterator<T> : IDisposable
{
    private IEnumerator<T> Enumerator { get; }

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

    public bool TryMoveNext([NotNullWhen(true)] out T? next)
    {
        next = default;

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

    public void Dispose()
    {
        Enumerator.Dispose();
    }
}