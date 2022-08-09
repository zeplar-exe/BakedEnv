using System.Diagnostics.CodeAnalysis;

using BakedEnv.Interpreter.IntermediateParsers.Common;
using BakedEnv.Interpreter.IntermediateTokens;

namespace BakedEnv.Interpreter.IntermediateParsers;

internal class RootParser
{
    public TypeList<MatchParser> ContinueParsers { get; }

    public RootParser()
    {
        ContinueParsers = new TypeList<MatchParser>();
    }

    public static RootParser Default()
    {
        return new RootParser()
            .WithParser<ProcessorStatementParser>()
            .WithParser<StringParser>();
    }

    public RootParser WithParser<T>() where T : MatchParser, new()
    {
        ContinueParsers.Add<T>();

        return this;
    }
    
    public RootParser WithParser(MatchParser parser)
    {
        ContinueParsers.Add(parser);

        return this;
    }

    public IEnumerable<IntermediateToken> Parse(ParserIterator input)
    {
        while (true)
        {
            if (TryParseOne(input, out var token))
            {
                yield return token;
            }
            else
            {
                break;
            }
        }

        yield return new EndOfFileToken(input.Current?.EndIndex ?? 0);
    }

    private bool TryParseOne(ParserIterator input, [NotNullWhen(true)] out IntermediateToken? token)
    {
        token = null;
        
        if (!input.SkipTrivia(out var next))
        {
            return false;
        }

        foreach (var parser in ContinueParsers)
        {
            var result = parser.TryParse(next, input);

            if (result.IsMatch)
            {
                token = result.Token;
                
                break;
            }
        }

        token ??= new UnexpectedToken(next);

        return true;
    }
}