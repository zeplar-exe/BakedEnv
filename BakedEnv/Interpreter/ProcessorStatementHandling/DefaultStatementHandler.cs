using BakedEnv.Interpreter.Instructions;
using BakedEnv.Interpreter.Scopes;

namespace BakedEnv.Interpreter.ProcessorStatementHandling;

/// <summary>
/// Default implementation of <see cref="IProcessorStatementHandler"/>.
/// <br></br>
/// Allows for disabling of said items via <see cref="DefaultStatementHandler.DisableStatement"/>.
/// </summary>
public class DefaultStatementHandler : IProcessorStatementHandler
{
    private HashSet<string> Disabled { get; }

    /// <summary>
    /// Initialize a DefaultStatementHandler.
    /// </summary>
    public DefaultStatementHandler()
    {
        Disabled = new HashSet<string>();
    }

    /// <summary>
    /// Enable a statement that may have been disabled.
    /// </summary>
    /// <param name="statement">Statement to enable.</param>
    public void EnableStatement(string statement) => Disabled.Remove(statement);
    /// <summary>
    /// Disable a statement by its name. Useful for overriding certain default statements.
    /// </summary>
    /// <param name="statement">Statement to disable</param>
    public void DisableStatement(string statement) => Disabled.Add(statement);

    /// <inheritdoc />
    public bool TryHandle(ProcessorStatementInstruction instruction, InvocationContext context)
    {
        if (Disabled.Contains(instruction.Name))
            return false;
        
        switch (instruction.Name)
        {
            
        }

        return false;
    }
}