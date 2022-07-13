using BakedEnv.Interpreter.Expressions;
using BakedEnv.Objects;
using TokenCs;

namespace BakedEnv.Interpreter.ParserModules.Expressions;

internal class FunctionExpressionParserResult : ParserModuleResult
{
    public bool IsDeclaration { get; }
    public bool IsComplete { get; }
    public ValueExpression Method { get; }
    
    public FunctionExpressionParserResult(
        bool declaration, bool complete, 
        IEnumerable<LexerToken> allTokens, 
        BakedFunction function) : base(allTokens)
    {
        IsDeclaration = declaration;
        IsComplete = complete;
        Method = new ValueExpression(function);
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
            return new FunctionExpressionParserResult(false, false, Tokens, BakedFunction.Empty());
        }

        public FunctionExpressionParserResult BuildSuccess(BakedFunction function)
        {
            return new FunctionExpressionParserResult(true, true, Tokens, function);
        }

        public FunctionExpressionParserResult BuildFailure()
        {
            return new FunctionExpressionParserResult(true, false, Tokens, BakedFunction.Empty());
        }
    }
}