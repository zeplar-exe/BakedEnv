using TokenCs;

namespace BakedEnv.Interpreter.IntermediateParsers.Common;

public class LexerTokenList : List<LexerToken>
{
    private LexerTokenType[] Constraint { get; }
    public LexerTokenList(params LexerTokenType[] constraint)
    {
        Constraint = constraint;
    }

    public new void Add(LexerToken token)
    {
        AssertToken(token, nameof(token));

        base.Add(token);
    }
    
    public new void AddRange(IEnumerable<LexerToken> tokens)
    {
        foreach (var token in tokens)
        {
            AssertToken(token, nameof(token));
            
            base.Add(token);
        }
    }

    public new void Insert(int index, LexerToken token)
    {
        AssertToken(token, nameof(token));
        
        base.Insert(index, token);
    }
    
    public new void InsertRange(int index, IEnumerable<LexerToken> tokens)
    {
        foreach (var token in tokens)
        {
            AssertToken(token, nameof(token));
            
            base.Insert(index, token);
        }
    }

    private void AssertToken(LexerToken token, string argumentName)
    {
        if (Constraint.All(c => c != token.Type))
            throw new ArgumentException(
                $"Expected {{argumentName}}to be a LexerToken of type {Constraint}, got {token.Type}.");
    }
}