using BakedEnv.Interpreter.Expressions;
using BakedEnv.Interpreter.ParserModules.Common;
using BakedEnv.Objects;
using TokenCs;

namespace BakedEnv.Interpreter.ParserModules.Expressions;

internal class FunctionExpressionParserResult : ParserModuleResult
{
    public bool IsDeclaration { get; }
    public bool IsComplete { get; }
    public LexerToken KeywordToken { get; }
    public ParameterListParserResult Parameters { get; }
    public InstructionBlockParserResult Block { get; }
    public ValueExpression Function { get; }
    
    public FunctionExpressionParserResult(
        bool declaration, bool complete, 
        IEnumerable<LexerToken> allTokens, 
        LexerToken keywordToken,
        ParameterListParserResult parameters,
        InstructionBlockParserResult block,
        BakedFunction function) : base(allTokens)
    {
        IsDeclaration = declaration;
        KeywordToken = keywordToken;
        IsComplete = complete;
        Parameters = parameters;
        Block = block;
        Function = new ValueExpression(function);
    }

    public class Builder : ResultBuilder
    {
        private LexerToken? KeywordToken { get; set; }
        private ParameterListParserResult? Parameters { get; set; }
        private InstructionBlockParserResult? Block { get; set; }

        public Builder WithKeyword(LexerToken token)
        {
            AddToken(token);
            KeywordToken = token;

            return this;
        }

        public Builder WithParameters(ParameterListParserResult parameters)
        {
            AddTokensFrom(parameters);
            Parameters = parameters;

            return this;
        }

        public Builder WithBlock(InstructionBlockParserResult block)
        {
            AddTokensFrom(block);
            Block = block;

            return this;
        }

        public FunctionExpressionParserResult BuildNonDeclaration()
        {
            BuilderHelper.EnsurePropertyNotNull(KeywordToken);
            BuilderHelper.EnsurePropertyNotNull(Parameters);
            BuilderHelper.EnsurePropertyNotNull(Block);
            
            return new FunctionExpressionParserResult(
                false, false,
                AllTokens, KeywordToken, 
                Parameters, Block, BakedFunction.Empty());
        }

        public FunctionExpressionParserResult BuildSuccess(BakedFunction function)
        {
            BuilderHelper.EnsurePropertyNotNull(KeywordToken);
            BuilderHelper.EnsurePropertyNotNull(Parameters);
            BuilderHelper.EnsurePropertyNotNull(Block);
            
            return new FunctionExpressionParserResult(
                true, true, 
                AllTokens, KeywordToken, 
                Parameters, Block, function);
        }

        public FunctionExpressionParserResult BuildFailure()
        {
            BuilderHelper.EnsurePropertyNotNull(KeywordToken);
            BuilderHelper.EnsurePropertyNotNull(Parameters);
            BuilderHelper.EnsurePropertyNotNull(Block);
            
            return new FunctionExpressionParserResult(
                true, false, 
                AllTokens, KeywordToken, 
                Parameters, Block, BakedFunction.Empty());
        }
    }
}