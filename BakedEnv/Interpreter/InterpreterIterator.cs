using System.Diagnostics.CodeAnalysis;
using BakedEnv.Common;
using BakedEnv.Interpreter.Instructions;
using BakedEnv.Interpreter.IntermediateTokens;



namespace BakedEnv.Interpreter;

public class InterpreterIterator : EnumerableIterator<IntermediateToken>
{
    private BacklogEnumerable<IntermediateToken> Backlog { get; }
    private TypeList<IntermediateToken> IgnoreTokens { get; }

    public InterpreterIterator(IEnumerable<IntermediateToken> enumerable) :
        this(new BacklogEnumerable<IntermediateToken>(enumerable))
    {
        
    }

    public InterpreterIterator(BacklogEnumerable<IntermediateToken> backlog) : base(backlog)
    {
        Backlog = backlog;
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

            return true;
        }

        return false;
    }

    public bool TryTakeNextOfType<T>(
        [NotNullWhen(true)] out T? token, 
        [NotNullWhen(false)] out BakedError? error) where T : IntermediateToken
    {
        token = null;
        error = null;
        
        if (!TryPeekNext(out var peekToken))
        {
            error = BakedError.EEarlyEndOfFile(Current?.EndIndex ?? 0);
        } 
        else if (!peekToken.IsComplete)
        {
            error = BakedError.EIncompleteIntermediateToken(peekToken.GetType().Name, peekToken.StartIndex);
        }
        else if (peekToken is not T)
        {
            error = BakedError.EUnexpectedTokenType(
                typeof(T).Name, 
                peekToken.GetType().Name,
                peekToken.StartIndex);
        }

        if (error != null)
            return false;

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