using System.Diagnostics.CodeAnalysis;
using BakedEnv.Common;
using BakedEnv.Interpreter.IntermediateTokens;
using BakedEnv.Interpreter.Lexer;


namespace BakedEnv.Interpreter;

public class InterpreterIterator : EnumerableIterator<IntermediateToken>
{
    private BacklogEnumerable<IntermediateToken> Backlog { get; }
    private TypeList<IntermediateToken> IgnoreTokens { get; }

    public InterpreterIterator(IEnumerable<IntermediateToken> enumerable) : base(enumerable)
    {
        Backlog = enumerable as BacklogEnumerable<IntermediateToken> ?? new BacklogEnumerable<IntermediateToken>(enumerable);
        IgnoreTokens = new TypeList<IntermediateToken>();
    }

    public void Ignore<T>() where T : IntermediateToken
    {
        IgnoreTokens.Add<T>();
    }

    public IntermediateToken MoveNextOrThrow()
    {
        if (!TryMoveNext(out var next))
            BakedError.EEarlyEndOfFile(Current!.EndIndex).Throw();

        return next!;
    }

    public override bool TryMoveNext([NotNullWhen(true)] out IntermediateToken? next)
    {
        next = null;
        
        while (base.TryMoveNext(out var nextTemp))
        {
            if (IgnoreTokens.Any(t => t == nextTemp.GetType()))
                continue;

            next = nextTemp;
            
            if (!next.IsComplete)
            {
                BakedError.EIncompleteIntermediateToken(next.GetType().Name, next.StartIndex).Throw();
            }

            return true;
        }

        return false;
    }

    public bool TryTakeNextRaw(
        TextualTokenType type,
        [NotNullWhen(true)] out RawIntermediateToken? token, 
        out BakedError error)
    {
        token = null;
        error = default;
        
        if (!TryPeekNext(out var peekToken))
        {
            error = BakedError.EEarlyEndOfFile(Current?.EndIndex ?? 0);
            
            return false;
        } 
        else if (!peekToken.IsComplete)
        {
            error = BakedError.EIncompleteIntermediateToken(peekToken.GetType().Name, peekToken.StartIndex);
            
            return false;
        }
        else if (!peekToken.IsRawType(type))
        {
            error = BakedError.EUnexpectedTokenType(
                type.ToString(), 
                peekToken.GetType().Name,
                peekToken.StartIndex);

            return false;
        }

        TryMoveNext(out _);
        
        token = (RawIntermediateToken)peekToken;

        return true;
    }
    
    public bool TryPeekNext([NotNullWhen(true)] out IntermediateToken? token)
    {
        if (!TryMoveNext(out token))
            return false;
        
        Backlog.Push(token);
        
        return true;
    }
    
    public bool TryPeekNextOfType<T>([NotNullWhen(true)] out IntermediateToken? token) 
        where T : IntermediateToken
    {
        if (!TryPeekNext(out token))
            return false;

        if (token is not T)
            return false;
        
        return true;
    }

    public IEnumerable<IntermediateToken> PeekTakeWhile(Func<IntermediateToken, bool> predicate)
    {
        while (TryMoveNext(out var token))
        {
            if (!predicate.Invoke(token))
            {
                Backlog.Push(token);
                
                yield break;
            }

            yield return token;
        }
    }
}