using BakedEnv.Interpreter.Expressions;
using BakedEnv.Interpreter.ProcessorStatementHandling;
using BakedEnv.Interpreter.Scopes;
using BakedEnv.Objects;

namespace BakedEnv.Interpreter.Instructions;

/// <summary>
/// Instruction used in the handling of processor statements.
/// </summary>
/// <remarks>Usually used internally through <see cref="IProcessorStatementHandler"/>.</remarks>
public class ProcessorStatementInstruction : InterpreterInstruction
{
    /// <summary>
    /// The key/name of this processor statement.
    /// </summary>
    public string Name { get; set; }
    /// <summary>
    /// The value of this processor statement.
    /// </summary>
    public BakedExpression Expression { get; set; }
    
    /// <summary>
    /// Initialize a ProcessorStatementInstruction.
    /// </summary>
    /// <param name="name">The key/name of this processor statement.</param>
    /// <param name="expression">The expressional value of this processor statement.</param>
    /// <param name="sourceIndex">Source index used internally.</param>
    public ProcessorStatementInstruction(string name, BakedExpression expression, int sourceIndex) : base(sourceIndex)
    {
        Name = name;
        Expression = expression;
    }

    /// <inheritdoc />
    public override void Execute(InvocationContext context)
    {
        if (context.Interpreter.Environment == null)
            return;
        
        foreach (var handler in context.Interpreter.Environment.ProcessorStatementHandlers)
        {
            if (handler.TryHandle(this, context))
                return;
        }
        
        context.Interpreter.ReportError(CreateInvalidStatementError());
    }

    private BakedError CreateInvalidStatementError()
    {
        return new BakedError(
            ErrorCodes.UnregisteredProcStatement,
            $"A processor statement by the name of '{Name}' does not exist or has not been registered.",
            SourceIndex);
    }
}