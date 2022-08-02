using BakedEnv.Common;
using BakedEnv.Interpreter.IntermediateTokens;

using TokenCs;

namespace BakedEnv.Interpreter.IntermediateParsers;

internal class RootParser
{
    
    
    public IEnumerable<IntermediateToken> Parse(EnumerableIterator<LexerToken> iterator)
    {
        yield break;
    }
}