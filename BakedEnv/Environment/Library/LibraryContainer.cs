using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace BakedEnv.Environment.Library;

public class LibraryContainer : IEnumerable<LibraryEnvironment>
{
    private List<LibraryEnvironment> Libraries { get; }
    
    public event EventHandler<LibraryEnvironment>? LibraryAdded;
    public event EventHandler<LibraryEnvironment>? LibraryRemoved;
        
    public int Count => Libraries.Count;

    public LibraryContainer()
    {
        Libraries = new List<LibraryEnvironment>();
    }

    public void Add(LibraryEnvironment library)
    {
        Libraries.Add(library);
        LibraryAdded?.Invoke(this, library);
    }

    public bool Remove(LibraryEnvironment library)
    {
        if (Libraries.Remove(library))
        {
            LibraryRemoved?.Invoke(this, library);

            return true;
        }

        return false;
    }

    public void Clear()
    {
        foreach (var library in Libraries.ToArray())
        {
            Remove(library);
        }
    }

    public IEnumerator<LibraryEnvironment> GetEnumerator()
    {
        return Libraries.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}