using BakedEnv.Interpreter.ParserModules.Identifiers;
using TokenCs;

namespace BakedEnv.Interpreter.ParserModules.Common;

internal class NameListParserResult : ParserModuleResult
{
    public bool IsComplete {get;}
    public IEnumerable<SingleIdentifierParserResult> Identifiers { get; }
    public IEnumerable<LexerToken> Separators { get; }

    public NameListParserResult(
        bool complete, 
        IEnumerable<LexerToken> allTokens,
        IEnumerable<SingleIdentifierParserResult> identifiers, 
        IEnumerable<LexerToken> separators) : base(allTokens)
    {
        IsComplete = complete;
        Identifiers = identifiers;
        Separators = separators;
    }

    public class Builder
    {
        private List<LexerToken> Tokens { get; }
        private List<SingleIdentifierParserResult> Identifiers { get; }
        private List<LexerToken> Separators { get; }

        public Builder()
        {
            Tokens = new List<LexerToken>();
            Identifiers = new List<SingleIdentifierParserResult>();
            Separators = new List<LexerToken>();
        }

        public Builder WithIdentifier(SingleIdentifierParserResult identifierParser)
        {
            Tokens.AddRange(identifierParser.AllTokens);
            Identifiers.Add(identifierParser);

            return this;
        }

        public Builder WithSeparator(LexerToken token)
        {
            Tokens.Add(token);
            Separators.Add(token);

            return this;
        }

        public NameListParserResult Build(bool complete)
        {
            return new NameListParserResult(complete, Tokens, Identifiers, Separators);
        }
    }
}