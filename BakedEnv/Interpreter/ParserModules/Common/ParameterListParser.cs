using BakedEnv.Interpreter.ParserModules.Identifiers;
using BakedEnv.Interpreter.Parsers;
using TokenCs;

namespace BakedEnv.Interpreter.ParserModules.Common;

internal class ParameterListParser : ParserModule
{
    public ParameterListParser(InterpreterInternals internals) : base(internals)
    {
        
    }

    public ParameterListParserResult Parse()
    {
        var builder = new ParameterListParserResult.Builder();
        
        if (!Internals.Iterator.TryMoveNext(out var first))
        {
            return builder.Build(false);
        }

        if (first.Type != LexerTokenType.LeftParenthesis)
        {
            return builder.Build(false);
        }

        builder.WithOpening(first);
        
        Internals.IteratorTools.SkipWhitespaceAndNewlines();
        
        var nameParser = new NameListParser(Internals);
        var result = nameParser.Parse();
                    
        builder.WithNameList(result);

        if (!result.IsComplete)
        {
            return builder.Build(false);
        }
        
        Internals.IteratorTools.SkipWhitespaceAndNewlines();
        
        if (!Internals.Iterator.TryPeekNext(out var last))
        {
            return builder.Build(false);
        }

        if (last.Type != LexerTokenType.RightParenthesis)
        {
            return builder.Build(false);
        }

        return builder.WithClosing(last).Build(true);
    }
}

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
        private LexerToken OpenParenthesis { get; set; }
        private LexerToken CloseParenthesis { get; set; }
        private NameListParserResult NameList { get; set; }
        
        public Builder()
        {
            Tokens = new List<LexerToken>();
        }

        public Builder WithOpening(LexerToken token)
        {
            OpenParenthesis = token;

            return this;
        }

        public Builder WithClosing(LexerToken token)
        {
            CloseParenthesis = token;

            return this;
        }

        public Builder WithNameList(NameListParserResult names)
        {
            NameList = names;

            return this;
        }

        public ParameterListParserResult Build(bool complete)
        {
            return new ParameterListParserResult(complete, OpenParenthesis, CloseParenthesis, Tokens, NameList);
        }
    }
}