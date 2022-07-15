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

    public class Builder
    {
        private List<LexerToken> Tokens { get; }
        private List<SingleIdentifierParserResult> IdentifierNames { get; }
        private List<LexerToken> SeparatorTokens { get; }

        public Builder()
        {
            Tokens = new List<LexerToken>();
            IdentifierNames = new List<SingleIdentifierParserResult>();
            SeparatorTokens = new List<LexerToken>();
        }

        public Builder WithName(SingleIdentifierParserResult identifierParser)
        {
            IdentifierNames.Add(identifierParser);
            Tokens.AddRange(identifierParser.AllTokens);

            return this;
        }

        public Builder WithSeparator(LexerToken token)
        {
            SeparatorTokens.Add(token);
            Tokens.Add(token);

            return this;
        }

        public ChainIdentifierParserResult Build(bool completed)
        {
            return new ChainIdentifierParserResult(completed, Tokens, IdentifierNames, SeparatorTokens);
        }
    }
}