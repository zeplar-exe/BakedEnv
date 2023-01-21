using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace BakedEnv.Environment.Library;

public class LibraryContainer : IEnumerable<ILibraryEnvironment>
{
    private List<ILibraryEnvironment> Libraries { get; }
    
    public event EventHandler<ILibraryEnvironment>? LibraryAdded;
    public event EventHandler<ILibraryEnvironment>? LibraryRemoved;
        
    public int Count => Libraries.Count;

    public LibraryContainer()
    {
        Libraries = new List<ILibraryEnvironment>();
    }

    public void Add(ILibraryEnvironment library)
    {
        Libraries.Add(library);
        LibraryAdded?.Invoke(this, library);
    }

    public bool Remove(ILibraryEnvironment library)
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

    public IEnumerator<ILibraryEnvironment> GetEnumerator()
    {
        return Libraries.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}