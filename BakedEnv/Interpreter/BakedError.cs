using Jammo.ParserTools;

namespace BakedEnv.Interpreter;

/// <summary>
/// An error found during parsing or execution.
/// </summary>
/// <param name="Id">Optional error ID for use in external applications.</param>
/// <param name="Message">Error message.</param>
/// <param name="SourceIndex">Index, if possible, of the error from the source.</param>
public readonly record struct BakedError(string? Id, string Message, int SourceIndex);