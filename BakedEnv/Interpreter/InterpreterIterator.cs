using System.Diagnostics.CodeAnalysis;
using BakedEnv.Common;
using BakedEnv.Interpreter.IntermediateTokens;



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

    public bool TryTakeNextOfType<T>(
        [NotNullWhen(true)] out T? token, 
        out BakedError error) where T : IntermediateToken
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
        else if (peekToken is not T)
        {
            error = BakedError.EUnexpectedTokenType(
                typeof(T).Name, 
                peekToken.GetType().Name,
                peekToken.StartIndex);

            return false;
        }

        TryMoveNext(out _);
        
        token = (T)peekToken!;

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