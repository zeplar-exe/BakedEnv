using BakedEnv.Interpreter.Scopes;

namespace BakedEnv.Interpreter.InterpreterParsers;

public record ParserContext(BakedInterpreter Interpreter, IBakedScope Scope);