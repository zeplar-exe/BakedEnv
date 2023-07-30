using System.Diagnostics.CodeAnalysis;
using BakedEnv.Common;
using BakedEnv.Interpreter.IntermediateTokens;
using BakedEnv.Interpreter.Lexer;


namespace BakedEnv.Interpreter;

public class InterpreterIterator : IDisposable
{
    private IEnumerator<IntermediateToken> Enumerator { get; }
    private TypeList<IntermediateToken> IgnoreTokens { get; }
    private IteratorLinkedListNode? TokensLinkedList { get; set; }
    
    private bool ReserveCurrent { get; set; }

    public InterpreterIterator(IEnumerable<IntermediateToken> enumerable)
    {
        Enumerator = enumerable.GetEnumerator();
        IgnoreTokens = new TypeList<IntermediateToken>();
    }

    public Marker CreateMarker()
    {
        return new Marker(this, TokensLinkedList);
    }

    public void Ignore<T>() where T : IntermediateToken
    {
        IgnoreTokens.Add<T>();
    }

    public IntermediateToken MoveNextOrThrow()
    {
        if (!TryMoveNext(out var next))
            BakedError.EEarlyEndOfFile(Enumerator.Current.EndIndex).Throw();

        return next!;
    }

    public bool TryMoveNext([NotNullWhen(true)] out IntermediateToken? next)
    {
        next = null;

        if (ReserveCurrent)
        {
            ReserveCurrent = false;
            next = Enumerator.Current;
            
            return true;
        }
        
        if (TokensLinkedList?.Next is {} node)
        {
            next = node.Value;
            
            return true;
        }
        
        while (Enumerator.MoveNext())
        {
            var nextTemp = Enumerator.Current;
            
            if (IgnoreTokens.Any(t => t == nextTemp.GetType()))
                continue;

            if (TokensLinkedList == null)
            {
                TokensLinkedList = new IteratorLinkedListNode(nextTemp);
            }
            else
            {
                TokensLinkedList = TokensLinkedList.Next = new IteratorLinkedListNode(nextTemp);
            }
            
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
            error = BakedError.EEarlyEndOfFile(Enumerator.Current?.EndIndex ?? 0);
            
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

        ReserveCurrent = true;
        
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

    public class Marker
    {
        private InterpreterIterator Iterator { get; }
        
        /// <summary>
        /// The node to revert to upon calling <see cref="Restore"/>.
        /// </summary>
        /// <remarks>This value is permitted to be null, wherein the iterator
        /// will be restored to a state which only pulls from the normal
        /// iteration procedure. It is expected that this is only null when
        /// being created at the beginning of an <see cref="InterpreterIterator"/>'s lifetime.</remarks>
        internal IteratorLinkedListNode? Node { get; }

        internal Marker(InterpreterIterator iterator, IteratorLinkedListNode? node)
        {
            Iterator = iterator;
            Node = node;
        }

        public bool Restore()
        {
            Iterator.TokensLinkedList = Node;

            return true;
        }
    }

    internal class IteratorLinkedListNode
    {
        public IntermediateToken Value { get; }
        public IteratorLinkedListNode? Next { get; set; }
        public IteratorLinkedListNode? Previous { get; set; }

        public IteratorLinkedListNode(IntermediateToken value)
        {
            Value = value;
        }
    }

    public void Dispose()
    {
        Enumerator.Dispose();
    }
}