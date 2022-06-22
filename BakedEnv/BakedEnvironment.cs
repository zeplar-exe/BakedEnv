using BakedEnv.ControlStatements;
using BakedEnv.Interpreter;
using BakedEnv.Interpreter.ProcessorStatementHandling;
using BakedEnv.Interpreter.Sources;
using BakedEnv.Interpreter.Variables;
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
    public VariableContainer GlobalVariables { get; }
    
    /// <summary>
    /// Processor statement handlers.
    /// </summary>
    public List<IProcessorStatementHandler> ProcessorStatementHandlers { get; }
    
    public List<ControlStatementDefinition> ControlStatements { get; }

    public List<VariableReferenceType> VariableReferenceOrder { get; }
    
    public Stream? OutputStream { get; set; }

    /// <summary>
    /// Instantiate a BakedEnvironment.
    /// </summary>
    public BakedEnvironment()
    {
        GlobalVariables = new VariableContainer();
        ProcessorStatementHandlers = new List<IProcessorStatementHandler>();
        ControlStatements = new List<ControlStatementDefinition>();
        VariableReferenceOrder = new List<VariableReferenceType>();
    }

    /// <summary>
    /// Add a variable by key and value to the global variables.
    /// </summary>
    /// <param name="key">The variable name.</param>
    /// <param name="variable">The variable's value.</param>
    /// <returns></returns>
    public BakedEnvironment WithVariable(BakedVariable variable)
    {
        GlobalVariables.Add(variable);

        return this;
    }
    
    public BakedEnvironment WithVariable(string name, BakedObject value)
    {
        GlobalVariables.Add(new BakedVariable(name, value));

        return this;
    }

    public BakedEnvironment WithReadOnlyVariable(string name, BakedObject value)
    {
        GlobalVariables.Add(new BakedVariable(name, value) { IsReadOnly = true });

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

    public BakedEnvironment WithControlStatement(params ControlStatementDefinition[] definitions)
    {
        ControlStatements.AddRange(definitions);

        return this;
    }

    public BakedEnvironment WithOutputStream(Stream? stream)
    {
        OutputStream = stream;

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