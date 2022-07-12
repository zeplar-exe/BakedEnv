using BakedEnv.Interpreter.Parsers;
using TokenCs;

namespace BakedEnv.Interpreter.ParserModules.Common;

internal class ExpressionListParser : ParserModule
{
    public ExpressionListParser(InterpreterInternals internals) : base(internals)
    {
        
    }
}

internal class ExpressionListParserResult : ParserModuleResult
{
    public ExpressionListParserResult(IEnumerable<LexerToken> allTokens) : base(allTokens)
    {
        
    }
}