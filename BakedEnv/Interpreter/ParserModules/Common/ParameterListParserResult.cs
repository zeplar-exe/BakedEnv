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
        LexerToken openParenthesis,
        LexerToken closeParenthesis,
        IEnumerable<LexerToken> allTokens,
        NameListParserResult nameList) : base(allTokens)
    {
        IsComplete = complete;
        OpenParenthesis = openParenthesis;
        CloseParenthesis = closeParenthesis;
        NameList = nameList;
    }

    public class Builder
    {
        private List<LexerToken> Tokens { get; }
        private LexerToken? OpenParenthesis { get; set; }
        private LexerToken? CloseParenthesis { get; set; }
        private NameListParserResult? NameList { get; set; }
        
        public Builder()
        {
            Tokens = new List<LexerToken>();
        }

        public Builder WithOpening(LexerToken token)
        {
            OpenParenthesis = token;
            Tokens.Add(token);

            return this;
        }

        public Builder WithClosing(LexerToken token)
        {
            CloseParenthesis = token;
            Tokens.Add(token);

            return this;
        }

        public Builder WithNameList(NameListParserResult names)
        {
            NameList = names;
            Tokens.AddRange(names.AllTokens);

            return this;
        }

        public ParameterListParserResult Build(bool complete)
        {
            BuilderHelper.EnsurePropertyNotNull(OpenParenthesis);
            BuilderHelper.EnsurePropertyNotNull(CloseParenthesis);
            BuilderHelper.EnsurePropertyNotNull(NameList);
            
            return new ParameterListParserResult(complete, OpenParenthesis, CloseParenthesis, Tokens, NameList);
        }
    }
}