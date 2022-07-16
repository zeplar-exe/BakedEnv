using BakedEnv.Common;

namespace BakedEnv.Interpreter.Parsers;

internal class ParserEnvironment
{
    public BakedInterpreter Interpreter { get; }
    public InterpreterIterator Iterator { get; }
    public IteratorTools IteratorTools { get; }
    public CommonErrorReporter ErrorReporter { get; }
    public EnumNavigator<ParserState> State { get; }
    public Queue<BakedError> Errors { get; }

    public ParserEnvironment(
        BakedInterpreter interpreter, 
        InterpreterIterator iterator, 
        IteratorTools iteratorTools, 
        CommonErrorReporter errorReporter, 
        EnumNavigator<ParserState> state)
    {
        Interpreter = interpreter;
        Iterator = iterator;
        IteratorTools = iteratorTools;
        ErrorReporter = errorReporter;
        State = state;
        Errors = new Queue<BakedError>();
    }

    public void QueueError(BakedError error)
    {
        Errors.Enqueue(error);
    }
}