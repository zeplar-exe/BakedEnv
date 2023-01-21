namespace BakedEnv.Common;

public class ErrorReporter
{
    private List<BakedError> Errors { get; }

    public delegate void ErrorReportedHandler(ErrorReporter reporter, BakedError error);
    public event ErrorReportedHandler? ErrorReported;

    public bool AnyError => Errors.Count > 0;

    public ErrorReporter()
    {
        Errors = new List<BakedError>();
    }

    public void Report(BakedError error)
    {
        Errors.Add(error);
        ErrorReported?.Invoke(this, error);
    }

    public IEnumerable<BakedError> Extract()
    {
        return Errors;
    }

    public void Clear()
    {
        Errors.Clear();
    }
}