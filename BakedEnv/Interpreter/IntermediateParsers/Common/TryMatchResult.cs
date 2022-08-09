using System.Diagnostics.CodeAnalysis;

using BakedEnv.Interpreter.IntermediateTokens;

namespace BakedEnv.Interpreter.IntermediateParsers.Common;

public readonly struct TryMatchResult
{
    [MemberNotNullWhen(true, nameof(Token))]
    public bool IsMatch { get; }
    public IntermediateToken? Token { get; }

    public TryMatchResult(bool isMatch, IntermediateToken? token)
    {
        IsMatch = isMatch;
        Token = token;
    }

    public static TryMatchResult MatchSuccess(IntermediateToken token)
    {
        return new TryMatchResult(true, token);
    }

    public static TryMatchResult NotMatch()
    {
        return new TryMatchResult(false, null);
    }

    public static implicit operator bool(TryMatchResult result) => result.IsMatch;
}