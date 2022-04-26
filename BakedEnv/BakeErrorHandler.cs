namespace BakedEnv;

/// <summary>
/// Error handler used during script execution.
/// </summary>
public enum BakeErrorHandler
{
    /// <summary>
    /// The error is logged and execution restarts after the error-causing token.
    /// </summary>
    Continue,
    /// <summary>
    /// The error is logged and further execution is terminated.
    /// </summary>
    Terminate,
}