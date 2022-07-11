namespace BakedEnv.Interpreter.ParserModules;

internal class ParserStack
{ 
    private LinkedList<ParserModule> List { get; }

    public ParserStack()
    {
        List = new LinkedList<ParserModule>();
    }

    public void Push(ParserModule module)
    {
        List.AddFirst(module);
    }

    public ParserModule? Peek(int levels = 0)
    {
        var item = List.First;

        if (item == null)
            return null;
        
        for (var i = 0; i < levels; i++)
        {
            item = item.Next;
            
            if (item == null)
                return null;
        }

        return item.Value;
    }

    public ParserModule? Pop()
    {
        var first = List.First;
        List.RemoveFirst();

        return first?.Value;
    }
}