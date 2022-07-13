using BakedEnv.Interpreter.ParserModules.Expressions;
using BakedEnv.Interpreter.ParserModules.Identifiers;
using BakedEnv.Interpreter.Parsers;
using TokenCs;

namespace BakedEnv.Interpreter.ParserModules.Common;

internal class NameListParser : ParserModule
{
    public NameListParser(InterpreterInternals internals) : base(internals)
    {
        
    }

    public NameListParserResult Parse()
    {
        var builder = new NameListParserResult.Builder();

        var expectValue = true;

        while (Internals.Iterator.TryMoveNext(out var token))
        {
            switch (token.Type)
            {
                case LexerTokenType.Comma:
                {
                    if (expectValue)
                    {
                        return builder.Build(false);
                    }

                    builder.WithSeparator(token);
                    expectValue = true;
                    
                    break;
                }
                default:
                {
                    Internals.Iterator.PushCurrent();

                    if (!expectValue)
                    {
                        return builder.Build(true);
                    }

                    var identifierParser = new SingleIdentifierParser(Internals);
                    var result = identifierParser.Parse();
                    
                    builder.WithIdentifier(result);

                    if (result.IsEmpty)
                    {
                        return builder.Build(false);
                    }

                    expectValue = false;
                    
                    break;
                }
            }
            
            Internals.IteratorTools.SkipWhitespaceAndNewlines();
        }

        return builder.Build(false);
    }
}

internal class NameListParserResult : ParserModuleResult
{
    public bool IsComplete {get;}
    public IEnumerable<SingleIdentifierResult> Identifiers { get; }
    public IEnumerable<LexerToken> Separators { get; }

    public NameListParserResult(
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

        public NameListParserResult Build(bool complete)
        {
            return new NameListParserResult(complete, Tokens, Identifiers, Separators);
        }
    }
}