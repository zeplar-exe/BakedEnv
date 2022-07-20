using BakedEnv.Interpreter.ParserModules.Common;
using TokenCs;

namespace BakedEnv.Interpreter.ParserModules.Misc;

internal class ProcessorParserResult : ParserModuleResult
{
    public bool IsComplete { get; }
    public LexerToken OpenBracket { get; }
    public LexerToken CloseBracket { get; }
    public KeyValueParserResult KeyValuePair { get; } 

    public ProcessorParserResult(
        bool complete, IEnumerable<LexerToken> allTokens, 
        LexerToken openBracket, LexerToken closeBracket, 
        KeyValueParserResult keyValuePair) : base(allTokens)
    {
        IsComplete = complete;
        OpenBracket = openBracket;
        CloseBracket = closeBracket;
        KeyValuePair = keyValuePair;
    }

    public class Builder : ResultBuilder
    {
        private LexerToken? OpenBracket { get; set; }
        private LexerToken? CloseBracket { get; set; }
        private KeyValueParserResult? KeyValuePair { get; set; }

        public Builder WithOpening(LexerToken token)
        {
            OpenBracket = token;

            return this;
        }

        public Builder WithClosing(LexerToken token)
        {
            CloseBracket = token;

            return this;
        }

        public Builder WithKeyValue(KeyValueParserResult keyValue)
        {
            KeyValuePair = keyValue;

            return this;
        }

        public ProcessorParserResult Build(bool complete)
        {
            BuilderHelper.EnsureLexerToken(OpenBracket, LexerTokenType.LeftBracket);
            BuilderHelper.EnsureLexerToken(CloseBracket, LexerTokenType.RightBracket);
            BuilderHelper.EnsurePropertyNotNull(KeyValuePair);
            
            return new ProcessorParserResult(complete, AllTokens, OpenBracket, CloseBracket, KeyValuePair);
        }
    }
}