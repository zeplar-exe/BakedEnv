using BakedEnv.Interpreter.Expressions;
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
        
        if (Internals.TestEndOfFile(out var token, out var eofResult))
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
        
        if (Internals.TestEndOfFile(out var next, out eofResult))
        {
            return builder.BuildFailure();
        }
        
        Internals.Iterator.PushCurrent();

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

        return builder.BuildSuccess(new BakedMethod(paramsResult.Names));
    }
}

internal class FunctionExpressionParserResult : ParserModuleResult
{
    public bool IsDeclaration { get; }
    public bool IsComplete { get; }
    public ValueExpression Method { get; }
    
    public FunctionExpressionParserResult(
        bool declaration, bool complete, 
        IEnumerable<LexerToken> allTokens, 
        BakedMethod method) : base(allTokens)
    {
        IsDeclaration = declaration;
        IsComplete = complete;
        Method = new ValueExpression(method);
    }

    public class Builder
    {
        private List<LexerToken> Tokens { get; }

        public Builder()
        {
            Tokens = new List<LexerToken>();
        }

        public Builder WithToken(LexerToken token)
        {
            Tokens.Add(token);

            return this;
        }

        public Builder WithTokens(IEnumerable<LexerToken> tokens)
        {
            Tokens.AddRange(tokens);

            return this;
        }

        public FunctionExpressionParserResult BuildNonDeclaration()
        {
            return new FunctionExpressionParserResult(false, false, Tokens, BakedMethod.Empty());
        }

        public FunctionExpressionParserResult BuildSuccess(BakedMethod method)
        {
            return new FunctionExpressionParserResult(true, true, Tokens, method);
        }

        public FunctionExpressionParserResult BuildFailure()
        {
            return new FunctionExpressionParserResult(true, false, Tokens, BakedMethod.Empty());
        }
    }
}