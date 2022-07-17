using BakedEnv.Interpreter.Scopes;
using BakedEnv.Objects;

namespace BakedEnv.Interpreter.Expressions;

public class TableExpression : BakedExpression
{
    private BakedExpression[] Keys { get; }
    private BakedExpression[] Values { get; }

    public TableExpression(IEnumerable<BakedExpression> keys, IEnumerable<BakedExpression> values)
    {
        Keys = keys.ToArray();
        Values = values.ToArray();
    }

    public override BakedObject Evaluate(InvocationContext context)
    {
        var table = new BakedTable();

        for (var keyIndex = 0; keyIndex < Keys.Length; keyIndex++)
        {
            var key = Keys[keyIndex].Evaluate(context);
            var value = Values.Length - 1 > keyIndex ? Values[keyIndex].Evaluate(context) : new BakedNull();

            table[key] = value;
        }

        return table;
    }
}