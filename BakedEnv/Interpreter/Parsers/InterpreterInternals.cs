using System.Diagnostics.CodeAnalysis;
using BakedEnv.Interpreter.ParserModules;
using BakedEnv.Interpreter.Scopes;
using TokenCs;

namespace BakedEnv.Interpreter.Parsers;

internal class InterpreterInternals
{
    public ParserStack ParserStack { get; }
    public BakedInterpreter Interpreter { get; }
    public InterpreterIterator Iterator { get; }
    public IteratorTools IteratorTools { get; }
    public CommonErrorReporter ErrorReporter { get; }
    public StateMachine<ParserState> State { get; }
    public IBakedScope Scope { get; }
    
    public InterpreterInternals(
        ParserStack parserStack,
        BakedInterpreter interpreter, 
        InterpreterIterator iterator, 
        IteratorTools iteratorTools, 
        CommonErrorReporter errorReporter, 
        StateMachine<ParserState> state, 
        IBakedScope scope)
    {
        ParserStack = parserStack;
        Interpreter = interpreter;
        Iterator = iterator;
        IteratorTools = iteratorTools;
        ErrorReporter = errorReporter;
        State = state;
        Scope = scope;
    }

    public bool TestEndOfFile([NotNullWhen(false)] out LexerToken? token, out TryResult result)
    {
        result = default;
        
        var isTrue = false;
        
        if (!Iterator.TryMoveNext(out token))
        {
            result = ErrorReporter.EndOfFileResult(Iterator.Current);
            isTrue = true;
        }

        return isTrue;
    }
}