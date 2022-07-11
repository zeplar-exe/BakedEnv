using BakedEnv.Interpreter.Variables;
using TokenCs;

namespace BakedEnv.Interpreter.ParserModules.Identifiers;

internal class ChainIdentifierResult
{
    public bool IsComplete { get; }
    public IEnumerable<LexerToken> AllTokens { get; }
    public IEnumerable<SingleIdentifierResult> IdentifierNames { get; }
    public IEnumerable<LexerToken> SeparatorTokens { get; }
    
    private ChainIdentifierResult(
        bool completed, List<LexerToken> tokens, 
        List<SingleIdentifierResult> identifierNames, 
        List<LexerToken> separatorTokens)
    {
        IsComplete = completed;
        AllTokens = tokens;
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
        private List<SingleIdentifierResult> IdentifierNames { get; }
        private List<LexerToken> SeparatorTokens { get; }

        public Builder WithName(SingleIdentifierResult identifier)
        {
            IdentifierNames.Add(identifier);
            Tokens.AddRange(identifier.AllTokens);

            return this;
        }

        public Builder WithSeparator(LexerToken token)
        {
            SeparatorTokens.Add(token);
            Tokens.Add(token);

            return this;
        }

        public ChainIdentifierResult Build(bool completed)
        {
            return new ChainIdentifierResult(completed, Tokens, IdentifierNames, SeparatorTokens);
        }
    }
}