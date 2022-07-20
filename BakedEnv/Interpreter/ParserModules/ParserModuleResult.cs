using TokenCs;

namespace BakedEnv.Interpreter.ParserModules;

internal class ParserModuleResult
{
    public LexerToken[] AllTokens { get; }
    public int SourceIndex => AllTokens.FirstOrDefault()?.StartIndex ?? -1;

    public ParserModuleResult(IEnumerable<LexerToken> allTokens)
    {
        AllTokens = allTokens.ToArray();
    }

    internal abstract class ResultBuilder
    {
        protected List<LexerToken> AllTokens { get; }
        protected List<BakedError> Errors { get; }

        public ResultBuilder()
        {
            AllTokens = new List<LexerToken>();
            Errors = new List<BakedError>();
        }

        public void AddToken(LexerToken token)
        {
            AllTokens.Add(token);
        }
        
        public void AddTokensFrom(ParserModuleResult result)
        {
            AddTokens(result.AllTokens);
        }

        public void AddTokens(IEnumerable<LexerToken> tokens)
        {
            AllTokens.AddRange(tokens);
        }

        public void AddError(string id, string message, int index)
        {
            AddError(new BakedError(id, message, index));
        }

        public void AddError(BakedError error)
        {
            Errors.Add(error);
        }
    }
}