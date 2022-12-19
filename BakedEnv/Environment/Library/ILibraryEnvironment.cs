using BakedEnv.ControlStatements;
using BakedEnv.Interpreter.ProcessorStatementHandling;
using BakedEnv.Keywords;
using BakedEnv.Variables;

namespace BakedEnv.Environment.Library;

public interface ILibraryEnvironment
{
    public VariableContainer Variables { get; }
    public List<IProcessorStatementHandler> ProcessorStatementHandlers { get; }
    public List<KeywordDefinition> Keywords { get; }
    public List<ControlStatementDefinition> ControlStatements { get; }
}