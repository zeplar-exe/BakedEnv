using BakedEnv.Interpreter.Scopes;

namespace BakedEnv.Interpreter.InterpreterParsers;

public record ParserContext(BakedInterpreter Interpreter, IBakedScope Scope)
{
    public ContextErrorReporter CreateReporter()
    {
        return new ContextErrorReporter();
    }

    public class ContextErrorReporter : IDisposable
    {
        private List<BakedError> Errors { get; }

        public ContextErrorReporter()
        {
            Errors = new List<BakedError>();
        }

        public void Report(BakedError error)
        {
            Errors.Add(error);
        }

        public void Report(IEnumerable<BakedError> errors)
        {
            Errors.AddRange(errors);
        }
        
        internal void Report(InterpreterInternalException internalException)
        {
            Errors.AddRange(internalException.Errors);
        }
        
        public void Dispose()
        {
            if (Errors.Count > 0)
                throw new InterpreterInternalException(Errors.ToArray());
        }
    }
}