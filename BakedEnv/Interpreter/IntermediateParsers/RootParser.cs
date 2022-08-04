using BakedEnv.Common;
using BakedEnv.Interpreter.IntermediateTokens;

using TokenCs;

namespace BakedEnv.Interpreter.IntermediateParsers;

internal class RootParser
{
    
    
    public IEnumerable<IntermediateToken> Parse(EnumerableIterator<LexerToken> input)
    {
        if (!input.TryMoveNext(out var first))
        {
            var index = input.Current?.StartIndex ?? 0;
            
            yield return new EndOfFileToken(index); 
            yield break;
        }

        yield return new EndOfFileToken(input.Current!.StartIndex);
    }
}