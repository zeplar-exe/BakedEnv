using BakedEnv.Interpreter.IntermediateTokens;
using BakedEnv.Interpreter.IntermediateTokens.Raw;

using TokenCs;

namespace BakedEnv.Interpreter.IntermediateParsers;

internal class RootParser
{
    
    
    public IEnumerable<IntermediateToken> Parse(ParserIterator input)
    {
        while (input.TryMoveNext(out var next))
        {
            switch (next.Type)
            {
                case LexerTokenType.LeftBracket:
                {
                    var bracketToken = new LeftBracketToken(next);
                    var processorParser = new ProcessorStatementParser();

                    var processorToken = processorParser.Parse(bracketToken, input);

                    yield return processorToken;
                    break;
                }
            }
        }

        yield return new EndOfFileToken(input.Current?.EndIndex ?? 0);
    }
}