using BakedEnv.Interpreter.Instructions;
using BakedEnv.Interpreter.Parsers;
using TokenCs;

namespace BakedEnv.Interpreter.ParserModules;

internal class InstructionParser : ParserModule
{
    public InstructionParser(InterpreterInternals internals) : base(internals)
    {
        
    }
}

internal class InstructionParserResult : ParserModuleResult
{
    public InterpreterInstruction Instruction { get; }
    
    public InstructionParserResult(IEnumerable<LexerToken> allTokens, InterpreterInstruction instruction) : base(allTokens)
    {
        
    }
}