using BakedEnv.Interpreter.ParserModules.Common;
using BakedEnv.Interpreter.Parsers;
using BakedEnv.Objects;
using TokenCs;

namespace BakedEnv.Interpreter.ParserModules.Expressions;

internal class FunctionExpressionParser : ParserModule
{
    public const string Keyword = "function";
    
    public FunctionExpressionParser(InterpreterInternals internals) : base(internals)
    {
        
    }

    public FunctionExpressionParserResult Parse()
    {
        var builder = new FunctionExpressionParserResult.Builder();
        
        if (!Internals.Iterator.TryMoveNext(out var token))
        {
            return builder.BuildFailure();
        }

        if (token.Type != LexerTokenType.AlphaNumeric || token.ToString() != Keyword)
        {
            Internals.Iterator.PushCurrent();
            
            return builder.BuildFailure();
        }
        
        builder.WithToken(token);

        Internals.IteratorTools.SkipWhitespaceAndNewlines();
        
        if (!Internals.Iterator.TryPeekNext(out var next))
        {
            return builder.BuildFailure();
        }

        if (next.Type != LexerTokenType.LeftParenthesis)
        {
            return builder.BuildNonDeclaration();
        }

        var paramsParser = new ParameterListParser(Internals);
        var paramsResult = paramsParser.Parse();
        
        builder.WithTokens(paramsResult.AllTokens);

        if (!paramsResult.IsComplete)
        {
            return builder.BuildFailure();
        }
        
        Internals.IteratorTools.SkipWhitespaceAndNewlines();

        var blockParser = new InstructionBlockParser(Internals);
        var blockResult = blockParser.Parse();
        
        builder.WithTokens(blockResult.AllTokens);

        if (!blockResult.IsComplete)
        {
            return builder.BuildFailure();
        }

        return builder.BuildSuccess(new BakedFunction(paramsResult.Names));
    }
}