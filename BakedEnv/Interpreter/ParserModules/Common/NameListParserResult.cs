using BakedEnv.Interpreter.ParserModules.Identifiers;
using TokenCs;

namespace BakedEnv.Interpreter.ParserModules.Common;

internal class NameListParserResult : ParserModuleResult
{
    public bool IsComplete {get;}
    public SingleIdentifierParserResult[] Identifiers { get; }
    public LexerToken[] Separators { get; }

    public NameListParserResult(
        bool complete, 
        IEnumerable<LexerToken> allTokens,
        IEnumerable<SingleIdentifierParserResult> identifiers, 
        IEnumerable<LexerToken> separators) : base(allTokens)
    {
        IsComplete = complete;
        Identifiers = identifiers.ToArray();
        Separators = separators.ToArray();
    }

    public class Builder : ResultBuilder
    {
        private List<SingleIdentifierParserResult> Identifiers { get; }
        private List<LexerToken> Separators { get; }

        public Builder()
        {
            
            Identifiers = new List<SingleIdentifierParserResult>();
            Separators = new List<LexerToken>();
        }

        public Builder WithIdentifier(SingleIdentifierParserResult identifierParser)
        {
            AddTokensFrom(identifierParser);
            Identifiers.Add(identifierParser);

            return this;
        }

        public Builder WithSeparator(LexerToken token)
        {
            AddToken(token);
            Separators.Add(token);

            return this;
        }

        public NameListParserResult Build(bool complete)
        {
            return new NameListParserResult(complete, AllTokens, Identifiers, Separators);
        }
    }
}