namespace BakedEnv.Interpreter.Parsers;

internal readonly record struct TryResult(bool Success, BakedError Error = default);