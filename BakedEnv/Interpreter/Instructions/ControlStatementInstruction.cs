using BakedEnv.ControlStatements;
using BakedEnv.Interpreter.Expressions;
using BakedEnv.Interpreter.Scopes;
using BakedEnv.Objects;

namespace BakedEnv.Interpreter.Instructions;

public class ControlStatementInstruction : InterpreterInstruction
{
    public ControlStatementDefinition StatementDefinition { get; }
    public BakedExpression[] Parameters { get; }
    public IEnumerable<InterpreterInstruction> Instructions { get; }
    
    public ControlStatementInstruction(ControlStatementDefinition definition, BakedExpression[] parameters, IEnumerable<InterpreterInstruction> instructions, int sourceIndex) : base(sourceIndex)
    {
        StatementDefinition = definition;
        Parameters = parameters;
        Instructions = instructions;
    }

    public override void Execute(InvocationContext context)
    {
        StatementDefinition.Execute(context, Parameters, Instructions);
    }
}