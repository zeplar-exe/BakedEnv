using BakedEnv.ControlStatements;
using BakedEnv.Interpreter;
using BakedEnv.Interpreter.ProcessorStatementHandling;
using BakedEnv.Keywords;
using BakedEnv.Sources;
using BakedEnv.Variables;

namespace BakedEnv.Environment;

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

    public List<KeywordDefinition> Keywords { get; }
    
    public List<ControlStatementDefinition> ControlStatements { get; }

    public VariableReferenceOrder VariableReferenceOrder { get; set; }
    
    public TextWriter? OutputWriter { get; set; }

    /// <summary>
    /// Instantiate a BakedEnvironment.
    /// </summary>
    public BakedEnvironment()
    {
        GlobalVariables = new VariableContainer();
        ProcessorStatementHandlers = new List<IProcessorStatementHandler>();
        Keywords = new List<KeywordDefinition>();
        ControlStatements = new List<ControlStatementDefinition>();
        VariableReferenceOrder = VariableReferenceOrder.Default();
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