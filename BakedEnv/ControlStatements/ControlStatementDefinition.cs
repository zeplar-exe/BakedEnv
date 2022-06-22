namespace BakedEnv.ControlStatements;

public class ControlStatementDefinition
{
    public string Name { get; }
    public int ParameterCount { get; }
    
    public ControlStatementExecution Execution { get; } 
    
    public ControlStatementDefinition(string name, int parameterCount, ControlStatementExecution execution)
    {
        Name = name;
        ParameterCount = parameterCount;
        Execution = execution;
    }
}