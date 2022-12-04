using BakedEnv.Interpreter.IntermediateTokens;
using BakedEnv.Interpreter.IntermediateTokens.Raw;
using BakedEnv.Interpreter.Lexer;

namespace BakedEnv.Interpreter.IntermediateParsers;

public class TokenTypeMap
{
    private Dictionary<TextualTokenType, Func<TextualToken, RawIntermediateToken>> Dictionary { get; }

    public TokenTypeMap()
    {
        Dictionary = new Dictionary<TextualTokenType, Func<TextualToken, RawIntermediateToken>>();
    }

    public void Map<T>(TextualTokenType type, Func<TextualToken, T> creator) where T : RawIntermediateToken
    {
        Dictionary[type] = creator;
    }

    public bool Contains(TextualTokenType type)
    {
        return Dictionary.ContainsKey(type);
    }

    public bool Unmap(TextualTokenType type)
    {
        return Dictionary.Remove(type);
    }

    public void Clear()
    {
        Dictionary.Clear();
    }

    public IntermediateToken Get(TextualToken token)
    {
        if (!Dictionary.TryGetValue(token.Type, out var creator))
            return new UnexpectedToken(token);

        return creator.Invoke(token);
    }
}