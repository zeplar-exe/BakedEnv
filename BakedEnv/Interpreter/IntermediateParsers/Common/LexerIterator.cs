using System.Diagnostics.CodeAnalysis;

using BakedEnv.Common;
using BakedEnv.Interpreter.Lexer;



namespace BakedEnv.Interpreter.IntermediateParsers.Common;

public class LexerIterator
{
    private IEnumerator<TextualToken> Enumerator { get; }
    private bool ReserveCurrent { get; set; }
    
    public bool Ended { get; private set; }
    
    public TextualToken? Current => Enumerator.Current;
    
    public LexerIterator(IEnumerable<TextualToken> enumerable)
    {
        Enumerator = enumerable.GetEnumerator();
    }
    
    public virtual bool TryMoveNext(out TextualToken next)
    {
        next = default;

        if (ReserveCurrent)
        {
            next = Enumerator.Current;
            ReserveCurrent = false;
            
            return true;
        }

        if (Enumerator.MoveNext())
        {
            Ended = false;

            next = Enumerator.Current;
        }
        else
        {
            Ended = true;
        }

        return !Ended;
    }
    
    public bool NextIs(TextualTokenType type, [NotNullWhen(true)] out TextualToken? token)
    {
        token = null;

        if (!SkipTrivia(out token))
            return false;

        if (token.Type != type)
            return false;

        return true;
    }
    
    public bool NextIs(IEnumerable<TextualTokenType> types, [NotNullWhen(true)] out TextualToken? token)
    {
        token = null;

        if (!SkipTrivia(out token))
            return false;

        var type = token.Type;

        return types.Any(t => t == type);
    }
    
    public bool SkipTrivia([NotNullWhen(true)] out TextualToken? token)
    {
        token = null;

        while (true)
        {
            if (!TryMoveNext(out var next))
                return false;
            
            switch (next.Type)
            {
                case TextualTokenType.Space:
                case TextualTokenType.Tab:
                {
                    continue;
                }
                default:
                {
                    token = next;
                    break;
                }
            }
            
            break;
        }

        return true;
    }
    
    public void Reserve()
    {
        ReserveCurrent = true;
    }

    public void Dispose()
    {
        Enumerator.Dispose();
    }
}