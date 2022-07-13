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
        
    }
}

internal class ParameterListParserResult : ParserModuleResult
{
    public bool IsComplete { get; }
    public IEnumerable<SingleIdentifierResult> Identifiers { get; }
    public IEnumerable<LexerToken> Separators { get; }
    public IEnumerable<string> Names => Identifiers.Select(i => i.Identifier);

    public ParameterListParserResult(
        bool complete,
        IEnumerable<LexerToken> allTokens,
        IEnumerable<SingleIdentifierResult> identifiers,
        IEnumerable<LexerToken> separators) : base(allTokens)
    {
        IsComplete = complete;
        Identifiers = identifiers;
        Separators = separators;
    }

    public class Builder
    {
        private List<LexerToken> Tokens { get; }
        private List<SingleIdentifierResult> Identifiers { get; }
        private List<LexerToken> Separators { get; }
        
        public Builder()
        {
            Tokens = new List<LexerToken>();
            Identifiers = new List<SingleIdentifierResult>();
            Separators = new List<LexerToken>();
        }

        public Builder WithIdentifier(SingleIdentifierResult identifier)
        {
            Tokens.AddRange(identifier.AllTokens);
            Identifiers.Add(identifier);

            return this;
        }

        public Builder WithSeparator(LexerToken token)
        {
            Tokens.Add(token);
            Separators.Add(token);

            return this;
        }

        public ParameterListParserResult BuildSuccess()
        {
            return new ParameterListParserResult(true, Tokens, Identifiers, Separators);
        }

        public ParameterListParserResult BuildFailure()
        {
            return new ParameterListParserResult(false, Tokens, Identifiers, Separators);
        }
    }
}