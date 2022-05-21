using BakedEnv.Interpreter;
using BakedEnv.Interpreter.Sources;
using BakedEnv.Objects;

namespace BakedEnv;

/// <summary>
/// An script environment to load and execute scripts.
/// </summary>
public class BakedEnvironment
{
    public BakedInterpreter Interpreter { get; private set; }

    /// <summary>
    /// Global variables accessible anywhere within an executed script.
    /// </summary>
    public Dictionary<string, BakedObject> GlobalVariables { get; }
    
    /// <summary>
    /// <see cref="BakeType"/> to assume when it is not specified during execution.
    /// Default value (during construction) is <see cref="BakeType.Script">BakeType.Script</see>.
    /// </summary>
    public BakeType DefaultBakeType { get; set; }

    /// <summary>
    /// Instantiate a BakedEnvironment.
    /// </summary>
    public BakedEnvironment()
    {
        DefaultBakeType = BakeType.Script;
        GlobalVariables = new Dictionary<string, BakedObject>();
    }

    /// <summary>
    /// Assign true and false to their respective variables.
    /// </summary>
    /// <returns></returns>
    public BakedEnvironment WithBooleanVariables()
    {
        GlobalVariables["true"] = new BakedBoolean(true);
        GlobalVariables["false"] = new BakedBoolean(false);

        return this;
    }

    public ScriptSession CreateSession()
    {
        return CreateSession(new BakedInterpreter().WithDefaultStatementHandler());
    }
    
    public ScriptSession CreateSession(BakedInterpreter interpreter)
    {
        return new ScriptSession(interpreter.WithEnvironment(this));
    }
    
    public ScriptSession CreateSession(IBakedSource source)
    {
        return CreateSession(new BakedInterpreter().WithDefaultStatementHandler(), source);
    }

    public ScriptSession CreateSession(BakedInterpreter interpreter, IBakedSource source)
    {
        return new ScriptSession(interpreter.WithEnvironment(this)).WithSource(source);
    }
}