using System.Diagnostics.CodeAnalysis;

namespace BakedEnv.Interpreter.InterpreterParsers;

public readonly struct DescendResult
{
    [MemberNotNullWhen(true, nameof(Parser))] public bool Success { get; }
    public InterpreterParserNode? Parser { get; }

    private DescendResult(bool success, InterpreterParserNode? item)
    {
        Success = success;
        Parser = item;
    }

    [MemberNotNull(nameof(Parser))]
    public static DescendResult SuccessfulIf(InterpreterParserNode? item, Func<bool> predicate) => new(predicate.Invoke(), item);
    public static DescendResult Successful(InterpreterParserNode item) => new(true, item);
    public static DescendResult Failure() => new(false, default);
}