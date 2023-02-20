using BakedEnv.ControlStatements;
using BakedEnv.Environment.Library;
using BakedEnv.Interpreter.ProcessorStatementHandling;
using BakedEnv.Keywords;
using BakedEnv.Variables;

namespace BakedEnv.Environment;

/// <summary>
/// An script environment to load and execute scripts.
/// </summary>
public sealed class BakedEnvironment : ILibraryEnvironment, IDisposable
{
    /// <summary>
    /// Global variables accessible anywhere within an executed script.
    /// </summary>
    public VariableContainer Variables { get; }
    public List<IProcessorStatementHandler> ProcessorStatementHandlers { get; }
    public List<KeywordDefinition> Keywords { get; }
    public List<ControlStatementDefinition> ControlStatements { get; }
    
    public LibraryContainer Libraries { get; }
    public VariableReferenceOrder VariableReferenceOrder { get; set; }
    
    public TextWriter? OutputWriter { get; set; }
    
    public ExpressionParserContainer ExpressionParsers { get; }

    /// <summary>
    /// Instantiate a BakedEnvironment.
    /// </summary>
    public BakedEnvironment()
    {
        Variables = new VariableContainer();
        ProcessorStatementHandlers = new List<IProcessorStatementHandler>();
        Keywords = new List<KeywordDefinition>();
        ControlStatements = new List<ControlStatementDefinition>();
        Libraries = new LibraryContainer();
        VariableReferenceOrder = VariableReferenceOrder.Default();
        ExpressionParsers = new ExpressionParserContainer();
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

    public void Dispose()
    {
        OutputWriter?.Dispose();
    }
}