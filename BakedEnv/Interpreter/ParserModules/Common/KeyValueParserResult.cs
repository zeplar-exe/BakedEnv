using BakedEnv.Interpreter.ParserModules.Expressions;
using TokenCs;

namespace BakedEnv.Interpreter.ParserModules.Common;

internal class KeyValueParserResult : ParserModuleResult
{
    public bool IsComplete { get; }
    public ExpressionParserResult Key { get; }
    public ExpressionParserResult Value { get; }
    public LexerToken Separator { get; }
    
    public KeyValueParserResult(
        bool complete, 
        IEnumerable<LexerToken> allTokens, 
        ExpressionParserResult key, 
        ExpressionParserResult value, 
        LexerToken separator) : base(allTokens)
    {
        IsComplete = complete;
        Key = key;
        Value = value;
        Separator = separator;
    }
    
    public class Builder : ResultBuilder
    {
        private ExpressionParserResult? Key { get; set; }
        private ExpressionParserResult? Value { get; set; }
        private LexerToken? Separator { get; set; }

        public Builder WithKey(ExpressionParserResult key)
        {
            Key = key;
            AddTokensFrom(key);

            return this;
        }

        public Builder WithValue(ExpressionParserResult value)
        {
            Value = value;
            AddTokensFrom(value);

            return this;
        }

        public Builder WithSeparator(LexerToken token)
        {
            Separator = token;
            AddToken(token);

            return this;
        }

        public KeyValueParserResult Build(bool complete)
        {
            BuilderHelper.EnsurePropertyNotNull(Key);
            BuilderHelper.EnsurePropertyNotNull(Value);
            BuilderHelper.EnsureLexerToken(Separator, LexerTokenType.Comma);

            return new KeyValueParserResult(complete, AllTokens, Key, Value, Separator);
        }
    }
}