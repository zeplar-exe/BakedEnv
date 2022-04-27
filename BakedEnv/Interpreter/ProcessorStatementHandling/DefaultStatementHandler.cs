using BakedEnv.Extensions;
using BakedEnv.Interpreter.Instructions;

namespace BakedEnv.Interpreter.ProcessorStatementHandling;

/// <summary>
/// Default implementation of <see cref="IProcessorStatementHandler"/>.
/// Handles <see cref="BakeType"/> and <see cref="BakeErrorHandler"/>.
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
                if (!instruction.Value.TryGetAs<string>(out var stringValue))
                {
                    interpreter.ReportError(ProcessorHandleHelper.CreateIncorrectValueTypeError<string>(instruction));
                    
                    return false;
                }

                if (!Enum.TryParse<BakeType>(stringValue, out var bakeType))
                {
                    interpreter.ReportError(ProcessorHandleHelper.CreateInvalidEnumValueError<BakeErrorHandler>(instruction));
                    
                    return false;
                }

                interpreter.Context.BakeType = bakeType;
                
                break;
            }
            case "NullReferenceError":
            {
                if (!instruction.Value.IsWhole())
                {
                    interpreter.ReportError(ProcessorHandleHelper.CreateIncorrectValueTypeError(instruction, "integer"));

                    return false;
                }

                interpreter.Context.NullReferenceErrorEnabled = (int)instruction.Value > 0;
                
                break;
            }
        }

        return false;
    }
}