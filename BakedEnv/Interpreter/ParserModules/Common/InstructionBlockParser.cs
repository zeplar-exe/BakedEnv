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
        var builder = new InstructionBlockParserResult.Builder();

        if (!Internals.Iterator.TryPeekNext(out var first))
        {
            return builder.Build(false);
        }

        if (first.Type != LexerTokenType.LeftCurlyBracket)
        {
            return builder.Build(false);
        }

        builder.WithOpening(first);

        var instructionParser = new InstructionParser(Internals);

        while (true)
        {
            if (!Internals.Iterator.TryMoveNext(out var token))
            {
                return builder.Build(false);
            }

            if (token.Type == LexerTokenType.RightCurlyBracket)
            {
                return builder.WithClosing(token).Build(true);
            }
            
            Internals.Iterator.PushCurrent();

            var result = instructionParser.Parse();

            builder.WithInstruction(result);
            
            if (!result.IsSuccess)
            {
                return builder.Build(false);
            }
        }
    }
}

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