using BakedEnv.Common;
using BakedEnv.Interpreter.Scopes;

namespace BakedEnv.Interpreter.Parsers;

internal class ParserEnvironment
{
    public BakedInterpreter Interpreter { get; }
    public InterpreterIterator Iterator { get; }
    public IteratorTools IteratorTools { get; }
    public CommonErrorReporter ErrorReporter { get; }
    public EnumNavigator<ParserState> State { get; }
    public IBakedScope Scope { get; }
    
    public ParserEnvironment(
        BakedInterpreter interpreter, 
        InterpreterIterator iterator, 
        IteratorTools iteratorTools, 
        CommonErrorReporter errorReporter, 
        EnumNavigator<ParserState> state, 
        IBakedScope scope)
    {
        Interpreter = interpreter;
        Iterator = iterator;
        IteratorTools = iteratorTools;
        ErrorReporter = errorReporter;
        State = state;
        Scope = scope;
    }
}