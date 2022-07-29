using BakedEnv.Interpreter.Variables;

namespace BakedEnv;

public class VariableReferenceOrder
{
    private readonly HashSet<VariableReferenceType> b_order;
    public IEnumerable<VariableReferenceType> Order => b_order;

    public VariableReferenceOrder()
    {
        b_order = new HashSet<VariableReferenceType>();
    }
    
    public VariableReferenceOrder(params VariableReferenceType[] order) : this(order.AsEnumerable())
    {
        
    }

    public VariableReferenceOrder(IEnumerable<VariableReferenceType> order)
    {
        b_order = new HashSet<VariableReferenceType>();
        
        foreach (var type in order)
        {
            Then(type);
        }
    }

    public VariableReferenceOrder Then(VariableReferenceType type)
    {
        b_order.Add(type);

        return this;
    }
}