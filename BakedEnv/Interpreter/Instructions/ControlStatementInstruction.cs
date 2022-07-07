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

    public override void Execute(BakedInterpreter interpreter, IBakedScope scope)
    {
        var context = new InvocationContext(scope, SourceIndex);
        var parameters = Parameters.Select(p => p.Evaluate(interpreter, context)).ToArray();
        
        ControlStatement.Execute(interpreter, scope, parameters, Instructions);
    }
}