namespace BakedEnv.Interpreter;

internal enum ParserState
{
    Any,
        
    MethodBody,
    ControlStatementBody,
}