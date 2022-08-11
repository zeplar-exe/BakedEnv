using System.Diagnostics.CodeAnalysis;

using BakedEnv.Interpreter.IntermediateParsers.Common;
using BakedEnv.Interpreter.IntermediateTokens;

namespace BakedEnv.Interpreter.IntermediateParsers;

internal class AnyParser
{
    public TypeList<MatchParser> ContinueParsers { get; }

    public AnyParser()
    {
        ContinueParsers = new TypeList<MatchParser>();
    }

    public static AnyParser Default()
    {
        return new AnyParser()
            .WithParser<ProcessorStatementParser>()
            .WithParser<StringParser>()
            .WithParser<NumericParser>();
    }

    public AnyParser WithParser<T>() where T : MatchParser, new()
    {
        ContinueParsers.Add<T>();

        return this;
    }

    public AnyParser WithParser(MatchParser parser)
    {
        ContinueParsers.Add(parser);

        return this;
    }

    public AnyParser WithoutParser<T>() where T : MatchParser, new()
    {
        ContinueParsers.RemoveAll<T>();

        return this;
    }

    public bool TryParseOne(ParserIterator input, [NotNullWhen(true)] out IntermediateToken? token)
    {
        token = null;
        
        if (!input.SkipTrivia(out var next))
        {
            return false;
        }

        foreach (var parser in ContinueParsers)
        {
            if (!parser.Match(next))
                continue;
            
            token = parser.Parse(next, input);
        }

        token ??= new UnexpectedToken(next);

        return true;
    }
}