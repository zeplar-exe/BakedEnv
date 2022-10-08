using BakedEnv.Helpers;
using BakedEnv.Interpreter.Lexer;



namespace BakedEnv.Interpreter;

internal class CommonErrorReporter
{
    private BakedInterpreter Interpreter { get; }

    public CommonErrorReporter(BakedInterpreter interpreter)
    {
        Interpreter = interpreter;
    }
    
    public bool TestUnexpectedTokenType(TextualToken token, out BakedError error, params TextualTokenType[] expected)
    {
        error = default;
        
        if (expected.All(t => token.Type != t))
        {
            error = ReportUnexpectedTokenType(token, expected);

            return true;
        }

        return false;
    }
    
    public BakedError ReportUnexpectedTokenType(TextualToken token, params TextualTokenType[] expected)
    {
        return Interpreter.ReportError(BakedError.EUnexpectedTokenType(
            StringHelper.CreateEnumList(expected), token.Type, 
            token.StartIndex));
    }
    
    public BakedError ReportEndOfFile(TextualToken token)
    {
        return Interpreter.ReportError(BakedError.EEarlyEndOfFile(token.StartIndex));
    }
}