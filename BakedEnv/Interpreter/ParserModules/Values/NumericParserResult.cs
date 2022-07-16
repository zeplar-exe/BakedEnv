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
    
    public class Builder : ResultBuilder
    {

        public Builder WithToken(LexerToken token)
        {
            AddToken(token);

            return this;
        }

        public NumericParserResult BuildSuccess(BakedObject value)
        {
            return new NumericParserResult(true, AllTokens, value);
        }
        
        public NumericParserResult BuildFailure()
        {
            return new NumericParserResult(false, AllTokens, new BakedNull());
        }
    }
}