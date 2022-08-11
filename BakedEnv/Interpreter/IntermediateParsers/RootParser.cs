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
        var root = new RootParser();

        var defaultParsers = AnyParser.Default().ContinueParsers;
        root.ContinueParsers.AddRange(defaultParsers);

        return root;
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

    public RootParser WithoutParser<T>() where T : MatchParser, new()
    {
        ContinueParsers.RemoveAll<T>();

        return this;
    }

    public IEnumerable<IntermediateToken> Parse(ParserIterator input)
    {
        var any = new AnyParser();
        
        any.ContinueParsers.AddRange(ContinueParsers);
        
        while (true)
        {
            if (any.TryParseOne(input, out var token))
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
}