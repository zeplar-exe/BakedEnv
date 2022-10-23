namespace BakedEnv.Interpreter.IntermediateParsers.Errors;

public class ErrorReporter
{
    private List<BakedError> Errors { get; }

    public ErrorReporter()
    {
        Errors = new List<BakedError>();
    }

    public void Report(BakedError error)
    {
        Errors.Add(error);
    }

    public IEnumerable<BakedError> Extract()
    {
        return Errors;
    }
}