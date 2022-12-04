using BakedEnv.Interpreter.IntermediateParsers.Common;
using BakedEnv.Interpreter.IntermediateTokens;
using BakedEnv.Interpreter.Lexer;

namespace BakedEnv.Interpreter.IntermediateParsers;

public class MappedTokenTypeParser : MatchParser
{
    public TokenTypeMap TypeMap { get; }

    public MappedTokenTypeParser()
    {
        TypeMap = new TokenTypeMap();
    }
    
    public override bool Match(TextualToken first)
    {
        return TypeMap.Contains(first.Type);
    }

    public override IntermediateToken Parse(TextualToken first, LexerIterator iterator)
    {
        return TypeMap.Get(first);
    }
}