using TokenCs;

namespace BakedEnv.Interpreter.ParserModules.Values;

internal class StringParserResult : ParserModuleResult
{
    public bool IsComplete { get; }
    public string String { get; }
    
    public StringParserResult(bool complete, IEnumerable<LexerToken> allTokens, string s) : base(allTokens)
    {
        IsComplete = complete;
        String = s;
    }

    public class Builder
    {
        private List<LexerToken> Tokens { get; }
        private List<LexerToken> ContentTokens { get; }

        public Builder()
        {
            Tokens = new List<LexerToken>();
            ContentTokens = new List<LexerToken>();
        }

        public Builder WithQuotation(LexerToken token)
        {
            Tokens.Add(token);

            return this;
        }

        public Builder WithContentToken(LexerToken token)
        {
            Tokens.Add(token);
            ContentTokens.Add(token);

            return this;
        }

        public StringParserResult Build(bool complete)
        {
            return new StringParserResult(complete, Tokens, string.Join("", ContentTokens.Select(t => t.ToString())));
        }
    }
}