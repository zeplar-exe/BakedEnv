using System.Diagnostics.CodeAnalysis;

using BakedEnv.Interpreter.IntermediateTokens;

namespace BakedEnv.Interpreter.InterpreterParsers;

public abstract class InterpreterParser
{
    public abstract DescendResult Descend(IntermediateToken token);

    public readonly struct DescendResult
    {
        [MemberNotNullWhen(true, nameof(Parser))] public bool Success { get; }
        public InterpreterParserNode? Parser { get; }

        private DescendResult(bool success, InterpreterParserNode? parser)
        {
            Success = success;
            Parser = parser;
        }

        [MemberNotNull(nameof(Parser))]
        public static DescendResult Successful(InterpreterParserNode parser) => new(true, parser);
        public static DescendResult Failure() => new(false, null);
    }
}