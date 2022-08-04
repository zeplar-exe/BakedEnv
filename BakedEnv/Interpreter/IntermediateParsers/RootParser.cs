using System.Diagnostics.CodeAnalysis;

using BakedEnv.Interpreter.IntermediateTokens;
using BakedEnv.Interpreter.IntermediateTokens.Raw;

using TokenCs;

namespace BakedEnv.Interpreter.IntermediateParsers;

internal class RootParser
{
    
    
    public IEnumerable<IntermediateToken> Parse(ParserIterator input)
    {
        while (true)
        {
            if (TryParseOne(input, out var token))
            {
                yield return token;
            }
            else
            {
                break;
            }
        }

        yield return new EndOfFileToken(input.Current?.EndIndex ?? 0);
    }

    private bool TryParseOne(ParserIterator input, [NotNullWhen(true)] out IntermediateToken? token)
    {
        token = null;
        
        if (!input.SkipTrivia(out var next))
        {
            return false;
        }
            
        switch (next.Type)
        {
            case LexerTokenType.LeftBracket:
            {
                var bracketToken = new LeftBracketToken(next);
                var processorParser = new ProcessorStatementParser();

                token = processorParser.Parse(bracketToken, input);
                
                break;
            }
            case LexerTokenType.SingleQuotation:
            case LexerTokenType.DoubleQuotation:
            {
                var quotationToken = new QuotationToken(next);
                var stringParser = new StringParser();

                token = stringParser.Parse(quotationToken, input);
                
                break;
            }
            default:
            {
                token = new UnexpectedToken(next);
                
                break;
            }
        }

        return true;
    }
}