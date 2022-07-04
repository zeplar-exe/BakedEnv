using BakedEnv.Interpreter.Instructions;

namespace BakedEnv.Interpreter.ProcessorStatementHandling;

/// <summary>
/// Helper methods for use in statement handling.
/// </summary>
public class ProcessorHandleHelper
{
    /// <summary>
    /// Create an error for an incorrect value type.
    /// </summary>
    /// <param name="instruction"><see cref="ProcessorStatementInstruction"/> which caused the error.</param>
    /// <typeparam name="TExpected">Expected value type.</typeparam>
    /// <returns></returns>
    public static BakedError CreateIncorrectValueTypeError<TExpected>(ProcessorStatementInstruction instruction)
    {
        return CreateIncorrectValueTypeError(instruction, typeof(TExpected).Name);
    }
    
    /// <summary>
    /// Create an error for an incorrect value type.
    /// </summary>
    /// <param name="instruction"><see cref="ProcessorStatementInstruction"/> which caused the error.</param>
    /// <param name="typeName">Name of the unexpected type.</param>
    /// <returns></returns>
    public static BakedError CreateIncorrectValueTypeError(ProcessorStatementInstruction instruction, string typeName)
    {
        return new BakedError(
            ErrorCodes.UnexpectedProcStatementValue,
            $"Expected '{typeName}' value for statement '{instruction.Name}', got '{instruction.Value.GetType().Name}'",
            instruction.SourceIndex);
    }
    
    /// <summary>
    /// Create an error for an out of range enum value.
    /// </summary>
    /// <param name="instruction"><see cref="ProcessorStatementInstruction"/> which caused the error.</param>
    /// <typeparam name="TExpected">Expected enum type.</typeparam>
    /// <returns></returns>
    public static BakedError CreateInvalidEnumValueError<TExpected>(ProcessorStatementInstruction instruction) 
        where TExpected : struct, Enum
    {
        var enumValues = string.Join(", ", Enum.GetValues<TExpected>());

        return new BakedError(
            ErrorCodes.ExpectedProcStatementEnum,
            $"Expected enum '{enumValues}' for statement '{instruction.Name}', got {instruction.Value.ToString()}",
            instruction.SourceIndex);
    }
}