using BakedEnv.Interpreter.Parsers;
using BakedEnv.Objects;
using TokenCs;

namespace BakedEnv.Interpreter.ParserModules.Values;

internal class ValueParser : ParserModule
{
    public ValueParser(InterpreterInternals internals) : base(internals)
    {
        
    }

    public ValueParserResult Parse()
    {
        var builder = new ValueParserResult.Builder();
        
        if (Internals.TestEndOfFile(out var first, out var eofResult))
        {
            return builder.BuildFailure();
        }

        switch (first.Type)
        {
            case LexerTokenType.Numeric:
            {
                break;
            }
            case LexerTokenType.DoubleQuotation:
            {
                break;
            }
        }
        
        return builder.BuildFailure();
    }
}

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

        public Builder WithTokens(IEnumerable<LexerToken> tokens)
        {
            Tokens.AddRange(tokens);

            return this;
        }

        public ValueParserResult BuildSuccess(BakedObject value)
        {
            return new ValueParserResult(true, Tokens, value);
        }

        public ValueParserResult BuildFailure()
        {
            return new ValueParserResult(false, Tokens, new BakedNull());
        }
    }
}