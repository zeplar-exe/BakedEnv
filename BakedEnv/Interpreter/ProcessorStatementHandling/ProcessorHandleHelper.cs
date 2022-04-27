using BakedEnv.Interpreter.Instructions;

namespace BakedEnv.Interpreter.ProcessorStatementHandling;

public class ProcessorHandleHelper
{
    public static BakedError CreateIncorrectValueTypeError<TExpected>(ProcessorStatementInstruction instruction)
    {
        return CreateIncorrectValueTypeError(instruction, typeof(TExpected).Name);
    }
    
    public static BakedError CreateIncorrectValueTypeError(ProcessorStatementInstruction instruction, string typeName)
    {
        return new BakedError(
            null,
            $"Expected '{typeName}' value for statement '{instruction.Name}', got '{instruction.Value.GetType().Name}'",
            instruction.SourceIndex);
    }
    
    public static BakedError CreateInvalidEnumValueError<TExpected>(ProcessorStatementInstruction instruction) 
        where TExpected : struct, Enum
    {
        var enumValues = string.Join(", ", Enum.GetValues<TExpected>());

        return new BakedError(
            null,
            $"Expected enum '{enumValues}' for statement '{instruction.Name}', got {instruction.Value.ToString()}",
            instruction.SourceIndex);
    }
}