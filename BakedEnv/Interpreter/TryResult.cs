namespace BakedEnv.Interpreter;

internal readonly record struct TryResult(bool Success, BakedError Error = default);