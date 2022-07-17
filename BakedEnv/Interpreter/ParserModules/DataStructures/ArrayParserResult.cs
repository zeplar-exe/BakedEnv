using BakedEnv.Interpreter.Expressions;
using BakedEnv.Interpreter.ParserModules.Common;
using BakedEnv.Interpreter.ParserModules.Expressions;
using TokenCs;

namespace BakedEnv.Interpreter.ParserModules.DataStructures;

internal class ArrayParserResult : ParserModuleResult
{
    public bool IsComplete { get; }
    public LexerToken OpenBracket { get; }
    public LexerToken CloseBracket { get; }
    public ExpressionListParserResult ExpressionList { get; }

    public ArrayParserResult(bool complete, IEnumerable<LexerToken> allTokens, 
        LexerToken openBracket,
        LexerToken closeBracket, 
        ExpressionListParserResult expressionList) : base(allTokens)
    {
        IsComplete = complete;
        OpenBracket = openBracket;
        CloseBracket = closeBracket;
        ExpressionList = expressionList;
    }

    public class Builder : ResultBuilder
    {
        private LexerToken? OpenBracket { get; set; }
        private LexerToken? CloseBracket { get; set; }
        private ExpressionListParserResult? Expressions { get; set; }

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

        public Builder WithExpressions(ExpressionListParserResult expressions)
        {
            Expressions = expressions;
            AddTokensFrom(expressions);
            
            return this;
        }

        public ArrayParserResult Build(bool complete)
        {
            BuilderHelper.EnsureLexerToken(OpenBracket, LexerTokenType.LeftBracket);
            BuilderHelper.EnsureLexerToken(CloseBracket, LexerTokenType.RightBracket);
            BuilderHelper.EnsurePropertyNotNull(Expressions);
            
            return new ArrayParserResult(complete, AllTokens, OpenBracket, CloseBracket, Expressions);
        }
    }
}