using BakedEnv.Interpreter.Instructions;
using TokenCs;

namespace BakedEnv.Interpreter.ParserModules;

internal class InstructionParserResult : ParserModuleResult
{
    public bool IsSuccess { get; }
    public InterpreterInstruction Instruction { get; }
    
    public InstructionParserResult(bool success, IEnumerable<LexerToken> allTokens, InterpreterInstruction instruction) : base(allTokens)
    {
        IsSuccess = success;
        Instruction = instruction;
    }

    public class Builder : ResultBuilder
    {
        public Builder WithModuleResult(ParserModuleResult result)
        {
            AddTokensFrom(result);

            return this;
        }

        public InstructionParserResult EndOfFile(int index)
        {
            return BuildInvalid(new BakedError(
                ErrorCodes.EndOfFile,
                ErrorMessages.EndOfFile,
                index));
        }

        public InstructionParserResult BuildInvalid(BakedError error)
        {
            return Build(false, new InvalidInstruction(error));
        }

        public InstructionParserResult Build(bool success, InterpreterInstruction instruction)
        {
            return new InstructionParserResult(success, AllTokens, instruction);
        }
    }
}