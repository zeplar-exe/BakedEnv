using BakedEnv.Objects;
using TokenCs;

namespace BakedEnv.Interpreter.ParserModules.Values;

internal class NumericParserResult : ParserModuleResult
{
    public bool IsComplete { get; }
    public BakedObject Value { get; }
    
    public NumericParserResult(bool complete, IEnumerable<LexerToken> allTokens, BakedObject value) : base(allTokens)
    {
        IsComplete = complete;
        Value = value;
    }
    
    public class Builder
    {
        private List<LexerToken> Tokens { get; }

        public Builder()
        {
            Tokens = new List<LexerToken>();
        }

        public Builder WithToken(LexerToken token)
        {
            Tokens.Add(token);

            return this;
        }

        public NumericParserResult BuildSuccess(BakedObject value)
        {
            return new NumericParserResult(true, Tokens, value);
        }
        
        public NumericParserResult BuildFailure()
        {
            return new NumericParserResult(false, Tokens, new BakedNull());
        }
    }
}