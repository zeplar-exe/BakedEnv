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
    /// <summary>
    /// Read-only variables accessible anywhere within an executed script.
    /// </summary>
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

    /// <summary>
    /// Add a variable by key and value to the global variables.
    /// </summary>
    /// <param name="key">The variable name.</param>
    /// <param name="value">The variable's value.</param>
    /// <returns></returns>
    public BakedEnvironment WithVariable(string key, BakedObject value)
    {
        GlobalVariables[key] = value;

        return this;
    }
    
    /// <summary>
    /// Add a read-only variable by key and value to the global read-only variables.
    /// </summary>
    /// <param name="key">The variable name.</param>
    /// <param name="value">The variable's value.</param>
    /// <returns></returns>
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

    /// <summary>
    /// Create a <see cref="ScriptSession"/> with a blank <see cref="BakedInterpreter"/>.
    /// </summary>
    /// <returns>The resulting <see cref="ScriptSession"/>.</returns>
    /// <remarks><see cref="BakedInterpreter.WithEnvironment"/> is called.</remarks>
    public ScriptSession CreateSession()
    {
        return CreateSession(new BakedInterpreter().WithEnvironment(this));
    }
    
    /// <summary>
    /// Create a <see cref="ScriptSession"/> with an interpreter.
    /// </summary>
    /// <param name="interpreter">Interpreter to use.</param>
    /// <returns>The resulting <see cref="ScriptSession"/>.</returns>
    /// <remarks><see cref="BakedInterpreter.WithEnvironment"/> is called.</remarks>
    public ScriptSession CreateSession(BakedInterpreter interpreter)
    {
        return new ScriptSession(interpreter.WithEnvironment(this));
    }
    
    /// <summary>
    /// Create a <see cref="ScriptSession"/> with an IBakedSource.
    /// </summary>
    /// <param name="source">The source to use.</param>
    /// <returns>The resulting <see cref="ScriptSession"/>.</returns>
    /// <remarks>Calls <see cref="BakedInterpreter.WithEnvironment"/>.</remarks>
    public ScriptSession CreateSession(IBakedSource source)
    {
        return CreateSession(new BakedInterpreter().WithEnvironment(this), source);
    }

    /// <summary>
    /// Create a <see cref="ScriptSession"/> with an interpreter and IBakedSource.
    /// </summary>
    /// <param name="interpreter">The interpreter to use.</param>
    /// <param name="source">The source to use.</param>
    /// <returns>The resulting ScriptSesion.</returns>
    public ScriptSession CreateSession(BakedInterpreter interpreter, IBakedSource source)
    {
        return new ScriptSession(interpreter.WithEnvironment(this)).WithSource(source);
    }
}