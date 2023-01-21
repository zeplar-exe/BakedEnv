using BakedEnv.Common;
using BakedEnv.Interpreter.IntermediateTokens;

namespace BakedEnv.Interpreter;

public static class ErrorReporterExtensions
{
    public static void ReportEndOfFile(this ErrorReporter reporter, IntermediateToken token)
    {
        reporter.Report(BakedError.EEarlyEndOfFile(token.StartIndex));
    }
}