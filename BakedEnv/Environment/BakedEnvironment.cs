using BakedEnv.ControlStatements;
using BakedEnv.Environment.Library;
using BakedEnv.Environment.ProcessVariables;
using BakedEnv.Interpreter.ProcessorStatementHandling;
using BakedEnv.Keywords;
using BakedEnv.Variables;

namespace BakedEnv.Environment;

/// <summary>
/// An script environment to load and execute scripts.
/// </summary>
public sealed class BakedEnvironment : ILibraryEnvironment
{
    private Dictionary<int, object?> EnvironmentProcessVariables { get; }

    /// <summary>
    /// Global variables accessible anywhere within an executed script.
    /// </summary>
    public VariableContainer Variables { get; }
    public List<IProcessorStatementHandler> ProcessorStatementHandlers { get; }
    public List<KeywordDefinition> Keywords { get; }
    public List<ControlStatementDefinition> ControlStatements { get; }
    
    public LibraryContainer Libraries { get; }
    public VariableReferenceOrder VariableReferenceOrder { get; set; }
    
    public ExpressionParserContainer ExpressionParsers { get; }

    /// <summary>
    /// Instantiate a BakedEnvironment.
    /// </summary>
    public BakedEnvironment()
    {
        EnvironmentProcessVariables = new Dictionary<int, object?>();
        Variables = new VariableContainer();
        ProcessorStatementHandlers = new List<IProcessorStatementHandler>();
        Keywords = new List<KeywordDefinition>();
        ControlStatements = new List<ControlStatementDefinition>();
        Libraries = new LibraryContainer();
        VariableReferenceOrder = VariableReferenceOrder.Default();
        ExpressionParsers = new ExpressionParserContainer();
    }
    
    public bool TryGetProcessVariable<T>(EnvironmentProcessVariable<T> variable, out T? value)
    {
        value = default;
        
        if (EnvironmentProcessVariables.TryGetValue(variable.GetHashCode(), out var existing))
        {
            value = (T?)existing;

            return true;
        }

        return false;
    }

    public T? GetProcessVariable<T>(EnvironmentProcessVariable<T> variable)
    {
        EnvironmentProcessVariables.TryGetValue(variable.GetHashCode(), out var value);
            
        return (T?)value;
    }
    
    public void SetProcessVariable<T>(EnvironmentProcessVariable<T> variable, T? value)
    {
        EnvironmentProcessVariables[variable.GetHashCode()] = variable;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns>This methods yields the current <see cref="BakedEnvironment"/>
    /// before items in <see cref="Libraries"/>.</returns>
    /// <remarks></remarks>
    public IEnumerable<ILibraryEnvironment> EnumerateAllLibraries()
    {
        yield return this;

        foreach (var library in Libraries)
        {
            yield return library;
        }
    }
}