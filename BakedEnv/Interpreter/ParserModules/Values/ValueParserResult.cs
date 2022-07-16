using BakedEnv.Objects;
using TokenCs;

namespace BakedEnv.Interpreter.ParserModules.Values;

internal class ValueParserResult : ParserModuleResult
{
    public bool IsSuccess { get; }
    public BakedObject Value { get; }

    public ValueParserResult(
        bool isSuccess, 
        IEnumerable<LexerToken> allTokens, 
        BakedObject value) : base(allTokens)
    {
        IsSuccess = isSuccess;
        Value = value;
    }

    public class Builder : ResultBuilder
    {
        public ValueParserResult BuildSuccess(BakedObject value)
        {
            return new ValueParserResult(true, AllTokens, value);
        }

        public ValueParserResult BuildFailure()
        {
            return new ValueParserResult(false, AllTokens, new BakedNull());
        }
    }
}