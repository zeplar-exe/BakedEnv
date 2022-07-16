using TokenCs;

namespace BakedEnv.Interpreter.ParserModules.Common;

internal class ParameterListParserResult : ParserModuleResult
{
    public bool IsComplete { get; }
    public LexerToken OpenParenthesis { get; }
    public LexerToken CloseParenthesis { get; }
    public NameListParserResult NameList { get; }
    public IEnumerable<string> Names => NameList.Identifiers.Select(i => i.Identifier);

    public ParameterListParserResult(
        bool complete,
        IEnumerable<LexerToken> allTokens,
        LexerToken openParenthesis,
        LexerToken closeParenthesis,
        NameListParserResult nameList) : base(allTokens)
    {
        IsComplete = complete;
        OpenParenthesis = openParenthesis;
        CloseParenthesis = closeParenthesis;
        NameList = nameList;
    }

    public class Builder : ResultBuilder
    {
        private LexerToken? OpenParenthesis { get; set; }
        private LexerToken? CloseParenthesis { get; set; }
        private NameListParserResult? NameList { get; set; }

        public Builder WithOpening(LexerToken token)
        {
            OpenParenthesis = token;
            AddToken(token);

            return this;
        }

        public Builder WithClosing(LexerToken token)
        {
            CloseParenthesis = token;
            AddToken(token);

            return this;
        }

        public Builder WithNameList(NameListParserResult names)
        {
            NameList = names;
            AddTokensFrom(names);

            return this;
        }

        public ParameterListParserResult Build(bool complete)
        {
            BuilderHelper.EnsureLexerToken(OpenParenthesis, LexerTokenType.LeftParenthesis);
            BuilderHelper.EnsureLexerToken(CloseParenthesis, LexerTokenType.RightParenthesis);
            BuilderHelper.EnsurePropertyNotNull(NameList);
            
            return new ParameterListParserResult(complete, AllTokens, OpenParenthesis, CloseParenthesis, NameList);
        }
    }
}