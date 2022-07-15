using TokenCs;

namespace BakedEnv.Interpreter.ParserModules.Common;

internal class InstructionBlockParserResult : ParserModuleResult
{
    public bool IsComplete { get; }
    public LexerToken OpenBracket { get; }
    public LexerToken CloseBracket { get; }
    public IEnumerable<InstructionParserResult> Instructions { get; }

    public InstructionBlockParserResult(
        bool complete, 
        IEnumerable<LexerToken> allTokens, 
        LexerToken openBracket,
        LexerToken closeBracket,
        IEnumerable<InstructionParserResult> instructions) : base(allTokens)
    {
        IsComplete = complete;
        OpenBracket = openBracket;
        CloseBracket = closeBracket;
        Instructions = instructions;
    }

    public class Builder
    {
        private List<LexerToken> Tokens { get; }
        private LexerToken? OpenBracket { get; set; }
        private LexerToken? CloseBracket { get; set; }
        private List<InstructionParserResult> Instructions {get;}

        public Builder()
        {
            Tokens = new List<LexerToken>();
            Instructions = new List<InstructionParserResult>();
        }

        public Builder WithOpening(LexerToken token)
        {
            OpenBracket = token;
            Tokens.Add(token);

            return this;
        }
        
        public Builder WithClosing(LexerToken token)
        {
            CloseBracket = token;
            Tokens.Add(token);

            return this;
        }

        public Builder WithInstruction(InstructionParserResult instruction)
        {
            Tokens.AddRange(instruction.AllTokens);
            Instructions.Add(instruction);

            return this;
        }

        public InstructionBlockParserResult Build(bool complete)
        {
            BuilderHelper.EnsureLexerToken(OpenBracket, LexerTokenType.LeftCurlyBracket);
            BuilderHelper.EnsureLexerToken(OpenBracket, LexerTokenType.RightCurlyBracket);
            
            return new InstructionBlockParserResult(complete, Tokens, OpenBracket, CloseBracket, Instructions);
        }
    }
}