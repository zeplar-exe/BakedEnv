using BakedEnv.ControlStatements;
using BakedEnv.Interpreter.ProcessorStatementHandling;
using BakedEnv.Keywords;
using BakedEnv.Variables;

namespace BakedEnv.Environment;

public class LibraryEnvironment
{
    public string Identifier { get; }
    
    public VariableContainer Variables { get; }
    /// <summary>
    /// Processor statement handlers.
    /// </summary>
    public List<IProcessorStatementHandler> ProcessorStatementHandlers { get; }
    public List<KeywordDefinition> Keywords { get; }
    public List<ControlStatementDefinition> ControlStatements { get; }

    public LibraryEnvironment(string identifier)
    {
        Identifier = identifier;
        Variables = new VariableContainer();
        ProcessorStatementHandlers = new List<IProcessorStatementHandler>();
        Keywords = new List<KeywordDefinition>();
        ControlStatements = new List<ControlStatementDefinition>();
    }
}