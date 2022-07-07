using BakedEnv.ControlStatements;
using BakedEnv.Interpreter.Expressions;
using BakedEnv.Interpreter.Scopes;
using BakedEnv.Objects;

namespace BakedEnv.Interpreter.Instructions;

public class ControlStatementInstruction : InterpreterInstruction
{
    public ControlStatementExecution ControlStatement { get; }
    public BakedExpression[] Parameters { get; }
    public IEnumerable<InterpreterInstruction> Instructions { get; }
    
    public ControlStatementInstruction(ControlStatementExecution controlStatement, BakedExpression[] parameters, IEnumerable<InterpreterInstruction> instructions, int sourceIndex) : base(sourceIndex)
    {
        ControlStatement = controlStatement;
        Parameters = parameters;
        Instructions = instructions;
    }

    public override void Execute(InvocationContext context)
    {
        var parameters = Parameters.Select(p => p.Evaluate(context)).ToArray();
        
        ControlStatement.Execute(context, parameters, Instructions);
    }
}