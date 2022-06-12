using System.Collections.ObjectModel;
using BakedEnv.Interpreter;
using BakedEnv.Interpreter.ProcessorStatementHandling;
using BakedEnv.Interpreter.Sources;
using BakedEnv.Objects;

namespace BakedEnv;

/// <summary>
/// An script environment to load and execute scripts.
/// </summary>
public class BakedEnvironment
{
    /// <summary>
    /// Global variables accessible anywhere within an executed script.
    /// </summary>
    public Dictionary<string, BakedObject> GlobalVariables { get; }
    public Dictionary<string, BakedObject> ReadOnlyGlobalVariables { get; }
    
    /// <summary>
    /// Processor statement handlers.
    /// </summary>
    public List<IProcessorStatementHandler> ProcessorStatementHandlers { get; }

    /// <summary>
    /// Instantiate a BakedEnvironment.
    /// </summary>
    public BakedEnvironment()
    {
        GlobalVariables = new Dictionary<string, BakedObject>();
        ReadOnlyGlobalVariables = new Dictionary<string, BakedObject>();
        ProcessorStatementHandlers = new List<IProcessorStatementHandler>();
    }

    public BakedEnvironment WithVariable(string key, BakedObject value)
    {
        GlobalVariables[key] = value;

        return this;
    }
    
    public BakedEnvironment WithReadOnlyVariable(string key, BakedObject value)
    {
        ReadOnlyGlobalVariables[key] = value;

        return this;
    }
    
    /// <summary>
    /// Add an array of <see cref="IProcessorStatementHandler"/> to the interpreter.
    /// </summary>
    /// <param name="handlers">Handlers to add.</param>
    public BakedEnvironment WithStatementHandlers(params IProcessorStatementHandler[] handlers)
    {
        ProcessorStatementHandlers.AddRange(handlers);

        return this;
    }

    public ScriptSession CreateSession()
    {
        return CreateSession(new BakedInterpreter());
    }
    
    public ScriptSession CreateSession(BakedInterpreter interpreter)
    {
        return new ScriptSession(interpreter.WithEnvironment(this));
    }
    
    public ScriptSession CreateSession(IBakedSource source)
    {
        return CreateSession(new BakedInterpreter(), source);
    }

    public ScriptSession CreateSession(BakedInterpreter interpreter, IBakedSource source)
    {
        return new ScriptSession(interpreter.WithEnvironment(this)).WithSource(source);
    }
}