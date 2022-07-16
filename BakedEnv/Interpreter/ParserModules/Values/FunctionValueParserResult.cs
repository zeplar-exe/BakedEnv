using BakedEnv.Interpreter.Expressions;
using BakedEnv.Interpreter.ParserModules.Common;
using BakedEnv.Interpreter.ParserModules.Identifiers;
using BakedEnv.Objects;
using TokenCs;

namespace BakedEnv.Interpreter.ParserModules.Values;

internal class FunctionValueParserResult : ParserModuleResult
{
    public bool IsDeclaration { get; }
    public bool IsComplete { get; }
    public LexerToken KeywordToken { get; }
    public ChainIdentifierParserResult Identifier { get; }
    public ParameterListParserResult Parameters { get; }
    public InstructionBlockParserResult Block { get; }
    public ValueExpression Function { get; }
    
    public FunctionValueParserResult(
        bool declaration,
        bool complete, 
        IEnumerable<LexerToken> allTokens,
        LexerToken keywordToken, 
        ChainIdentifierParserResult identifierParser, 
        ParameterListParserResult parameters, 
        InstructionBlockParserResult block, 
        BakedFunction function) : base(allTokens)
    {
        IsDeclaration = declaration;
        IsComplete = complete;
        KeywordToken = keywordToken;
        Identifier = identifierParser;
        Parameters = parameters;
        Block = block;
        Function = new ValueExpression(function);
    }

    public class Builder : ResultBuilder
    {
        private LexerToken? KeywordToken { get; set; }
        private ChainIdentifierParserResult? Identifier { get; set; }
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

        public Builder WithIdentifier(ChainIdentifierParserResult identifierParser)
        {
            AddTokensFrom(identifierParser);
            Identifier = identifierParser;

            return this;
        }

        public Builder WithBlock(InstructionBlockParserResult block)
        {
            AddTokensFrom(block);
            Block = block;

            return this;
        }

        public FunctionValueParserResult BuildNonDeclaration()
        {
            BuilderHelper.EnsureLexerToken(KeywordToken, LexerTokenType.AlphaNumeric);
            BuilderHelper.EnsurePropertyNotNull(Identifier);
            BuilderHelper.EnsurePropertyNotNull(Parameters);
            BuilderHelper.EnsurePropertyNotNull(Block);
            
            return new FunctionValueParserResult(
                false, false,
                AllTokens, KeywordToken,
                Identifier, Parameters, Block, BakedFunction.Empty());
        }

        public FunctionValueParserResult BuildSuccess(BakedFunction function)
        {
            BuilderHelper.EnsureLexerToken(KeywordToken, LexerTokenType.AlphaNumeric);
            BuilderHelper.EnsurePropertyNotNull(Identifier);
            BuilderHelper.EnsurePropertyNotNull(Parameters);
            BuilderHelper.EnsurePropertyNotNull(Block);
            
            return new FunctionValueParserResult(
                true, true,
                AllTokens, KeywordToken,
                Identifier, Parameters, Block, function);
        }

        public FunctionValueParserResult BuildFailure()
        {
            BuilderHelper.EnsureLexerToken(KeywordToken, LexerTokenType.AlphaNumeric);
            BuilderHelper.EnsurePropertyNotNull(Identifier);
            BuilderHelper.EnsurePropertyNotNull(Parameters);
            BuilderHelper.EnsurePropertyNotNull(Block);
            
            return new FunctionValueParserResult(
                true, false,
                AllTokens, KeywordToken,
                Identifier, Parameters, Block, BakedFunction.Empty());
        }
    }
}