using BakedEnv.Interpreter.ParserModules.Common;
using TokenCs;

namespace BakedEnv.Interpreter.ParserModules.Values;

internal class TableParserResult : ParserModuleResult
{
    public bool IsComplete { get; }
    public LexerToken OpenBracket { get; }
    public LexerToken CloseBracket { get; }
    public KeyValueParserResult[] KeyValuePairs { get; }
    
    public TableParserResult(
        bool complete,
        IEnumerable<LexerToken> allTokens,
        LexerToken openBracket,
        LexerToken closeBracket,
        IEnumerable<KeyValueParserResult> keyValuePairs) : base(allTokens)
    {
        IsComplete = complete;
        OpenBracket = openBracket;
        CloseBracket = closeBracket;
        KeyValuePairs = keyValuePairs.ToArray();
    }

    public class Builder : ResultBuilder
    {
        private LexerToken? OpenBracket { get; set; }
        private LexerToken? CloseBracket { get; set; }
        private List<KeyValueParserResult> Pairs { get; set; }

        public Builder()
        {
            Pairs = new List<KeyValueParserResult>();
        }

        public Builder WithOpening(LexerToken token)
        {
            OpenBracket = token;
            AddToken(token);

            return this;
        }

        public Builder WithClosing(LexerToken token)
        {
            CloseBracket = token;
            AddToken(token);

            return this;
        }

        public Builder WithKeyValuePair(KeyValueParserResult pair)
        {
            Pairs.Add(pair);
            AddTokensFrom(pair);
            
            return this;
        }

        public TableParserResult Build(bool complete)
        {
            BuilderHelper.EnsureLexerToken(OpenBracket, LexerTokenType.LeftCurlyBracket);
            BuilderHelper.EnsureLexerToken(CloseBracket, LexerTokenType.RightCurlyBracket);
            
            return new TableParserResult(complete, AllTokens, OpenBracket, CloseBracket, Pairs);
        }
    }
}