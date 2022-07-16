using BakedEnv.Interpreter.Variables;
using TokenCs;

namespace BakedEnv.Interpreter.ParserModules.Identifiers;

internal class ChainIdentifierParserResult : ParserModuleResult
{
    public bool IsComplete { get; }
    public IEnumerable<SingleIdentifierParserResult> IdentifierNames { get; }
    public IEnumerable<LexerToken> SeparatorTokens { get; }
    
    private ChainIdentifierParserResult(
        bool completed, IEnumerable<LexerToken> tokens, 
        IEnumerable<SingleIdentifierParserResult> identifierNames, 
        IEnumerable<LexerToken> separatorTokens) : base(tokens)
    {
        IsComplete = completed;
        IdentifierNames = identifierNames;
        SeparatorTokens = separatorTokens;
    }

    public VariableReference CreateReference(BakedInterpreter interpreter)
    {
        return new VariableReference(IdentifierNames.Select(i => i.Identifier), interpreter);
    }

    public class Builder : ResultBuilder
    {
        private List<SingleIdentifierParserResult> IdentifierNames { get; }
        private List<LexerToken> SeparatorTokens { get; }

        public Builder()
        {
            IdentifierNames = new List<SingleIdentifierParserResult>();
            SeparatorTokens = new List<LexerToken>();
        }

        public Builder WithName(SingleIdentifierParserResult identifierParser)
        {
            IdentifierNames.Add(identifierParser);
            AddTokensFrom(identifierParser);

            return this;
        }

        public Builder WithSeparator(LexerToken token)
        {
            SeparatorTokens.Add(token);
            AddToken(token);

            return this;
        }

        public ChainIdentifierParserResult Build(bool completed)
        {
            return new ChainIdentifierParserResult(completed, AllTokens, IdentifierNames, SeparatorTokens);
        }
    }
}