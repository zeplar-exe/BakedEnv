using BakedEnv.Extensions;
using BakedEnv.Interpreter.Instructions;
using BakedEnv.Objects;
using BakedEnv.Objects.Interfaces;

namespace BakedEnv.Interpreter.ProcessorStatementHandling;

/// <summary>
/// Default implementation of <see cref="IProcessorStatementHandler"/>.
/// Handles <see cref="BakeType"/> and <see cref="InterpreterContext.NullReferenceErrorEnabled"/>.
/// <br></br>
/// Allows for disabling of said items via <see cref="DefaultStatementHandler.DisableStatement"/>.
/// </summary>
public class DefaultStatementHandler : IProcessorStatementHandler
{
    private HashSet<string> Disabled { get; }

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
    public bool TryHandle(ProcessorStatementInstruction instruction, BakedInterpreter interpreter)
    {
        if (Disabled.Contains(instruction.Name))
            return false;
        
        switch (instruction.Name)
        {
            case "BakeType":
            {
                if (instruction.Value is not IBakedString stringValue)
                {
                    interpreter.ReportError(ProcessorHandleHelper.CreateIncorrectValueTypeError<string>(instruction));
                    
                    return false;
                }

                if (!Enum.TryParse<BakeType>(stringValue.ToString(), out var bakeType))
                {
                    interpreter.ReportError(ProcessorHandleHelper.CreateInvalidEnumValueError<BakeType>(instruction));
                    
                    return false;
                }

                interpreter.Context.BakeType = bakeType;
                
                break;
            }
        }

        return false;
    }
}