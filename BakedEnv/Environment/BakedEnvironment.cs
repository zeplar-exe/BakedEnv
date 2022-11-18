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
public sealed class BakedEnvironment : IDisposable
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
    public List<LibraryEnvironment> Libraries { get; }
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
        Libraries = new List<LibraryEnvironment>();
        VariableReferenceOrder = VariableReferenceOrder.Default();
    }

    public void Dispose()
    {
        OutputWriter?.Dispose();
    }
}