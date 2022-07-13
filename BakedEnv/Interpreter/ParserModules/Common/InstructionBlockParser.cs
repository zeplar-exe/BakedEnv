using BakedEnv.Interpreter.Instructions;
using BakedEnv.Interpreter.Parsers;
using TokenCs;

namespace BakedEnv.Interpreter.ParserModules.Common;

internal class InstructionBlockParser : ParserModule
{
    public InstructionBlockParser(InterpreterInternals internals) : base(internals)
    {
        
    }

    public InstructionBlockParserResult Parse()
    {
        
    }
}

internal class InstructionBlockParserResult : ParserModuleResult
{
    public bool IsComplete { get; }
    public IEnumerable<InterpreterInstruction> Instructions { get; }

    public InstructionBlockParserResult(
        bool complete, 
        IEnumerable<LexerToken> allTokens, 
        IEnumerable<InterpreterInstruction> instructions) : base(allTokens)
    {
        IsComplete = complete;
        Instructions = instructions;
    }

    public class Builder
    {
        private List<LexerToken> Tokens { get; }

        public Builder()
        {
            Tokens = new List<LexerToken>();
        }
    }
}