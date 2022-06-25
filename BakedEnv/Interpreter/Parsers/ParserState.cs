namespace BakedEnv.Interpreter.Parsers;

internal enum ParserState
{
    Any,
        
    MethodBody,
    ControlStatementBody,
}