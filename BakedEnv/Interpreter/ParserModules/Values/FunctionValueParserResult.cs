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
    public ChainIdentifierResult Identifier { get; }
    public ParameterListParserResult Parameters { get; }
    public InstructionBlockParserResult Block { get; }
    public ValueExpression Function { get; }
    
    public FunctionValueParserResult(
        bool declaration,
        bool complete, 
        IEnumerable<LexerToken> allTokens,
        LexerToken keywordToken, 
        ChainIdentifierResult identifier, 
        ParameterListParserResult parameters, 
        InstructionBlockParserResult block, 
        BakedFunction function) : base(allTokens)
    {
        IsDeclaration = declaration;
        IsComplete = complete;
        KeywordToken = keywordToken;
        Identifier = identifier;
        Parameters = parameters;
        Block = block;
        Function = new ValueExpression(function);
    }

    public class Builder
    {
        private List<LexerToken> Tokens { get; }
        private LexerToken KeywordToken { get; set; }
        private ChainIdentifierResult Identifier { get; set; }
        private ParameterListParserResult Parameters { get; set; }
        private InstructionBlockParserResult Block { get; set; }

        public Builder()
        {
            Tokens = new List<LexerToken>();
        }

        public Builder WithKeyword(LexerToken token)
        {
            Tokens.Add(token);
            KeywordToken = token;

            return this;
        }

        public Builder WithParameters(ParameterListParserResult parameters)
        {
            Tokens.AddRange(parameters.AllTokens);
            Parameters = parameters;

            return this;
        }

        public Builder WithIdentifier(ChainIdentifierResult identifier)
        {
            Tokens.AddRange(identifier.AllTokens);
            Identifier = identifier;

            return this;
        }

        public Builder WithBlock(InstructionBlockParserResult block)
        {
            Tokens.AddRange(block.AllTokens);
            Block = block;

            return this;
        }

        public FunctionValueParserResult BuildNonDeclaration()
        {
            return new FunctionValueParserResult(
                false, false,
                Tokens, KeywordToken,
                Identifier, Parameters, Block, BakedFunction.Empty());
        }

        public FunctionValueParserResult BuildSuccess(BakedFunction function)
        {
            return new FunctionValueParserResult(
                true, true,
                Tokens, KeywordToken,
                Identifier, Parameters, Block, function);
        }

        public FunctionValueParserResult BuildFailure()
        {
            return new FunctionValueParserResult(
                true, false,
                Tokens, KeywordToken,
                Identifier, Parameters, Block, BakedFunction.Empty());
        }
    }
}