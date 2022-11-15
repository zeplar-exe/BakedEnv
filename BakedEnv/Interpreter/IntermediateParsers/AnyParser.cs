using System.Diagnostics.CodeAnalysis;

using BakedEnv.Common;
using BakedEnv.Interpreter.IntermediateParsers.Common;
using BakedEnv.Interpreter.IntermediateTokens;

namespace BakedEnv.Interpreter.IntermediateParsers;

public class AnyParser : ParserBase
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
            .WithParser<NumericParser>()
            .WithParser<CommentParser>();
    }

    public AnyParser WithParser<T>() where T : MatchParser, new()
    {
        ContinueParsers.Add<T>();

        return this;
    }

    public AnyParser WithParser(Type parserType)
    {
        ContinueParsers.Add(parserType);

        return this;
    }

    public AnyParser WithoutParser<T>() where T : MatchParser, new()
    {
        ContinueParsers.Remove<T>();

        return this;
    }

    public IEnumerable<IntermediateToken> Parse(LexerIterator input)
    {
        while (TryParseOne(input, out var next))
        {
            yield return next;
        }
        
        yield return new EndOfFileToken(input.Current?.EndIndex ?? 0);
    }
    
    public bool TryParseOne(LexerIterator input, [NotNullWhen(true)] out IntermediateToken? token)
    {
        token = null;
        
        if (!input.SkipTrivia(out var next))
        {
            return false;
        }
        
        var parser = ContinueParsers.EnumerateInstances()
            .FirstOrDefault(p => p.Match(next));

        if (parser == null)
        {
            token = new UnexpectedToken(next);

            return true;
        }

        RegisterParser(parser);

        token = parser.Parse(next, input);

        return true;
    }
}