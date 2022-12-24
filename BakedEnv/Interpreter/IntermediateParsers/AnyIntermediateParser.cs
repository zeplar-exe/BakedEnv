using System.Diagnostics.CodeAnalysis;

using BakedEnv.Interpreter.IntermediateParsers.Common;
using BakedEnv.Interpreter.IntermediateTokens;
using BakedEnv.Interpreter.IntermediateTokens.Raw;
using BakedEnv.Interpreter.Lexer;

namespace BakedEnv.Interpreter.IntermediateParsers;

public class AnyIntermediateParser : IntermediateParser
{
    private List<MatchIntermediateParser> ContinueParsers { get; }

    public AnyIntermediateParser()
    {
        ContinueParsers = new List<MatchIntermediateParser>();
        
        var mappedCharacters = new MappedTokenTypeIntermediateParser();
        
        mappedCharacters.TypeMap.Map(TextualTokenType.Period, token => new PeriodToken(token));
        mappedCharacters.TypeMap.Map(TextualTokenType.Hashtag, token => new HashToken(token));
        mappedCharacters.TypeMap.Map(TextualTokenType.LeftBracket, token => new LeftBracketToken(token));
        mappedCharacters.TypeMap.Map(TextualTokenType.RightBracket, token => new RightBracketToken(token));
        mappedCharacters.TypeMap.Map(TextualTokenType.LeftParenthesis, token => new LeftParenthesisToken(token));
        mappedCharacters.TypeMap.Map(TextualTokenType.RightParenthesis, token => new RightParenthesisToken(token));
        mappedCharacters.TypeMap.Map(TextualTokenType.Equals, token => new EqualsToken(token));
        
        this.WithParser(mappedCharacters)
            .WithParser<StringIntermediateParser>()
            .WithParser<NumericIntermediateParser>() 
            .WithParser<IdentifierIntermediateParser>()
            .WithParser<CommentIntermediateParser>();
    }

    public AnyIntermediateParser WithParser<T>() where T : MatchIntermediateParser, new()
    {
        ContinueParsers.Add(new T());

        return this;
    }
    
    public AnyIntermediateParser WithParser<T>(T parser) where T : MatchIntermediateParser, new()
    {
        ContinueParsers.Add(parser);

        return this;
    }

    public AnyIntermediateParser WithoutParser<T>() where T : MatchIntermediateParser, new()
    {
        ContinueParsers.Remove(new T());

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
        
        var parser = ContinueParsers.FirstOrDefault(p => p.Match(next));

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