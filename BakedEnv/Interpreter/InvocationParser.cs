using System.Diagnostics.CodeAnalysis;
using BakedEnv.Interpreter.Instructions;
using BakedEnv.Interpreter.Variables;
using BakedEnv.Objects;
using Jammo.ParserTools.Lexing;
using Jammo.ParserTools.Tools;

namespace BakedEnv.Interpreter;

internal class InvocationParser
{
    private BakedInterpreter Interpreter { get; }
    private EnumerableIterator<LexerToken> Iterator { get; }
    private IteratorTools IteratorTools { get; }
    private StateMachine<ParserState> State { get; }
    private VariableReference Reference { get; }
    
    public InvocationParser(BakedInterpreter interpreter, EnumerableIterator<LexerToken> iterator, IteratorTools iteratorTools, StateMachine<ParserState> state, VariableReference reference)
    {
        Interpreter = interpreter;
        Iterator = iterator;
        IteratorTools = iteratorTools;
        State = state;
        Reference = reference;
    }

    public InterpreterInstruction Parse()
    {
        var startToken = Iterator.Current;
        
        return ParseControlStatement(startToken) ?? ParseInvocation(startToken);
    }

    private InterpreterInstruction? ParseControlStatement(LexerToken startToken)
    {
        if (Reference.Path.Count != 0 || Interpreter.Environment == null)
            return null;

        foreach (var statement in Interpreter.Environment.ControlStatements)
        {
            if (statement.Name == Reference.Name)
            {
                var paramParser = Interpreter.CreateParameterParser();
                var controlParameterResult =
                    paramParser.TryParseParameterList(out var controlParameters);

                if (!controlParameterResult.Success)
                {
                    return new InvalidInstruction(controlParameterResult.Error);
                }

                if (controlParameters.Length != statement.ParameterCount)
                {
                    return new InvalidInstruction(new BakedError()); // TODO
                }

                IteratorTools.SkipWhitespaceAndNewlines();

                if (!Iterator.Current.Is(LexerTokenId.OpenCurlyBracket))
                {
                    return new InvalidInstruction(new BakedError()); // TODO
                }

                State.MoveTo(ParserState.ControlStatementBody);

                var instructions = new List<InterpreterInstruction>();

                while (true)
                    // TODO: volatile conditions here
                {
                    if (!Interpreter.TryGetNextInstruction(out var controlInstruction))
                    {
                        return new InvalidInstruction(new BakedError()); // TODO
                    }

                    if (State.Current != ParserState.ControlStatementBody)
                        break;

                    instructions.Add(controlInstruction);
                }
                
                return new ControlStatementInstruction(
                    statement.Execution,
                    controlParameters,
                    instructions,
                    startToken.Span.Start);
            }
        }

        return null;
    }

    private InterpreterInstruction ParseInvocation(LexerToken startToken)
    {
        if (!Reference.TryGetVariable(out var variable))
        {
            return new InvalidInstruction(new BakedError(
                null,
                "Cannot invoke a null value.",
                startToken.Span.Start));
        }

        if (variable.Value is not IBakedCallable callable)
        {
            return new InvalidInstruction(new BakedError(
                null,
                "Cannot invoke a non-callable object.",
                startToken.Span.Start));
        }

        var parameterParser = Interpreter.CreateParameterParser();
        var parameterResult = parameterParser.TryParseParameterList(out var parameters);

        if (!parameterResult.Success)
        {
            return new InvalidInstruction(parameterResult.Error);
        }

        return new ObjectInvocationInstruction(callable, parameters.ToArray(), Iterator.Current.Span.Start);
    }
}