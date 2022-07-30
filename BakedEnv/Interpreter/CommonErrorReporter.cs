using BakedEnv.Helpers;

using TokenCs;

namespace BakedEnv.Interpreter;

internal class CommonErrorReporter
{
    private BakedInterpreter Interpreter { get; }

    public CommonErrorReporter(BakedInterpreter interpreter)
    {
        Interpreter = interpreter;
    }
    
    public bool TestUnexpectedTokenType(LexerToken token, out BakedError error, params LexerTokenType[] expected)
    {
        error = default;
        
        if (expected.All(t => token.Type != t))
        {
            error = ReportUnexpectedTokenType(token, expected);

            return true;
        }

        return false;
    }
    
    public BakedError ReportUnexpectedTokenType(LexerToken token, params LexerTokenType[] expected)
    {
        return Interpreter.ReportError(BakedError.TOKN.E1001(
            StringHelper.CreateEnumList(expected), token.Type, 
            token.StartIndex));
    }
    
    public BakedError ReportEndOfFile(LexerToken token)
    {
        return Interpreter.ReportError(BakedError.TOKN.E1000(token.StartIndex));
    }
}