using BakedEnv.ControlStatements;
using BakedEnv.Objects;

namespace BakedEnv.Interpreter.Instructions;

public class ControlStatementInstruction : InterpreterInstruction
{
    public ControlStatementExecution ControlStatement { get; }
    public BakedObject[] Parameters { get; }
    public IEnumerable<InterpreterInstruction> Instructions { get; }
    
    public ControlStatementInstruction(int sourceIndex, ControlStatementExecution controlStatement, BakedObject[] parameters, IEnumerable<InterpreterInstruction> instructions) : base(sourceIndex)
    {
        ControlStatement = controlStatement;
        Parameters = parameters;
        Instructions = instructions;
    }

    public override void Execute(BakedInterpreter interpreter, IBakedScope scope)
    {
        ControlStatement.Execute(interpreter, scope, Parameters, Instructions);
    }
}