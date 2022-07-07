namespace BakedEnv.Interpreter.Scopes;

public record InvocationContext(IBakedScope Scope, int SourceIndex = -1);