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

    public class Builder : ResultBuilder
    {
        private List<LexerToken> ContentTokens { get; }

        public Builder()
        {
            
            ContentTokens = new List<LexerToken>();
        }

        public Builder WithQuotation(LexerToken token)
        {
            AddToken(token);

            return this;
        }

        public Builder WithContentToken(LexerToken token)
        {
            AddToken(token);
            ContentTokens.Add(token);

            return this;
        }

        public StringParserResult Build(bool complete)
        {
            return new StringParserResult(complete, AllTokens, string.Join("", ContentTokens.Select(t => t.ToString())));
        }
    }
}