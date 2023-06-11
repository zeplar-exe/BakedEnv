using System.Diagnostics.CodeAnalysis;

namespace BakedEnv.Interpreter.InterpreterParsers;

public readonly struct DescendResult
{
    [MemberNotNullWhen(true, nameof(Parser))] public bool IsSuccess { get; }
    public InterpreterParserNode? Parser { get; }

    private DescendResult(bool isSuccess, InterpreterParserNode? item)
    {
        IsSuccess = isSuccess;
        Parser = item;
    }

    [MemberNotNull(nameof(Parser))]
    public static DescendResult SuccessIf(InterpreterParserNode? item, Func<bool> predicate) => new(predicate.Invoke(), item);
    public static DescendResult Success(InterpreterParserNode item) => new(true, item);
    public static DescendResult Failure() => new(false, default);
}