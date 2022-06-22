using BakedEnv.Interpreter;
using BakedEnv.Interpreter.Instructions;
using BakedEnv.Objects;

namespace BakedEnv.ControlStatements;

public class IfStatementExecution : ControlStatementExecution
{
    public override BakedObject Execute(BakedInterpreter interpreter, IBakedScope scope, BakedObject[] parameters,
        IEnumerable<InterpreterInstruction> instructions)
    {
        if (parameters[0].Equals(true))
        {
            foreach (var instruction in instructions)
            {
                instruction.Execute(interpreter, scope);

                if (instruction is IScriptTermination termination)
                    return termination.ReturnValue;
            }
        }

        return new BakedVoid();
    }
}