namespace BakedEnv.Interpreter;

public interface IBakedScope
{
    public IBakedScope? Parent { get; }
    public List<BakedVariable> Variables { get; }
    public List<BakedMethod> Methods { get; }

    protected IEnumerable<BakedVariable> EnumerateAllVariables()
    {
        foreach (var v in Variables)
            yield return v;

        var parent = Parent;

        while (parent != null)
        {
            foreach (var ancestorVar in parent.Variables)
                yield return ancestorVar;

            parent = parent.Parent;
        }
    }
}