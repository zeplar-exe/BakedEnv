using System.Collections;

using BakedEnv.Variables;

namespace BakedEnv.Environment;

public class VariableReferenceOrder : IEnumerable<VariableReferenceType>
{
    private HashSet<VariableReferenceType> Order { get; }

    public VariableReferenceOrder()
    {
        Order = new HashSet<VariableReferenceType>();
    }

    public static VariableReferenceOrder Empty() => new();

    public static VariableReferenceOrder Default()
    {
        return new VariableReferenceOrder()
            .Then(VariableReferenceType.Globals)
            .Then(VariableReferenceType.ScopeVariables);
    }
    
    public VariableReferenceOrder(params VariableReferenceType[] order) : this(order.AsEnumerable())
    {
        
    }

    public VariableReferenceOrder(IEnumerable<VariableReferenceType> order)
    {
        Order = new HashSet<VariableReferenceType>();
        
        foreach (var type in order)
        {
            Then(type);
        }
    }

    public VariableReferenceOrder Then(VariableReferenceType type)
    {
        Order.Add(type);

        return this;
    }

    public IEnumerator<VariableReferenceType> GetEnumerator()
    {
        return Order.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}