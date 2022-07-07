using BakedEnv.Interpreter.Expressions;
using BakedEnv.Interpreter.Instructions;
using BakedEnv.Interpreter.Variables;
using BakedEnv.Objects;
using Jammo.ParserTools.Lexing;
using Jammo.ParserTools.Tools;

namespace BakedEnv.Interpreter.Parsers;

internal class InvocationParser
{
    private InterpreterInternals Internals { get; }
    private VariableReference Reference { get; }
    
    public InvocationParser(InterpreterInternals internals, VariableReference reference)
    {
        Internals = internals;
        Reference = reference;
    }

    public InterpreterInstruction Parse()
    {
        var startToken = Internals.Iterator.Current;
        var paramParser = Internals.Interpreter.CreateParameterParser();
        var controlParameterResult =
            paramParser.TryParseParameterList(out var controlParameters);
        
        if (!controlParameterResult.Success)
        {
            return new InvalidInstruction(controlParameterResult.Error);
        }
        
        return ParseControlStatement(startToken, controlParameters) ?? ParseInvocation(startToken, controlParameters);
    }

    private InterpreterInstruction? ParseControlStatement(LexerToken startToken, BakedExpression[] parameters)
    {
        if (Reference.Path.Count != 0 || Internals.Interpreter.Environment == null)
            return null;

        foreach (var statement in Internals.Interpreter.Environment.ControlStatements)
        {
            if (statement.Name == Reference.Name)
            {
                if (parameters.Length != statement.ParameterCount)
                {
                    return new InvalidInstruction(new BakedError()); // TODO
                }

                Internals.IteratorTools.SkipWhitespaceAndNewlines();

                if (!Internals.Iterator.TryMoveNext(out var next))
                {
                    return new InvalidInstruction(Internals.ErrorReporter.ReportEndOfFile(Internals.Iterator.Current));
                }
                
                if (Internals.ErrorReporter.TestUnexpectedTokenType(next, out var error, LexerTokenId.OpenCurlyBracket))
                {
                    return new InvalidInstruction(error);
                }

                Internals.State.MoveTo(ParserState.ControlStatementBody);

                var instructions = new List<InterpreterInstruction>();

                while (true)
                    // TODO: volatile conditions here
                {
                    if (!Internals.Interpreter.TryGetNextInstruction(out var controlInstruction))
                    {
                        return new InvalidInstruction(Internals.ErrorReporter.ReportEndOfFile(Internals.Iterator.Current));
                    }

                    if (Internals.State.Current != ParserState.ControlStatementBody)
                        break;

                    instructions.Add(controlInstruction);
                }
                
                return new ControlStatementInstruction(
                    statement.Execution,
                    parameters,
                    instructions,
                    startToken.Span.Start);
            }
        }

        return null;
    }

    private InterpreterInstruction ParseInvocation(LexerToken startToken, BakedExpression[] parameters)
    {
        if (!Reference.TryGetVariable(out var variable))
        {
            return new InvalidInstruction(new BakedError(
                ErrorCodes.InvokeNull,
                "Cannot invoke a null value.",
                startToken.Span.Start));
        }

        if (variable.Value is not IBakedCallable callable)
        {
            return new InvalidInstruction(new BakedError(
                ErrorCodes.InvokeNonCallable,
                "Cannot invoke a non-callable object.",
                startToken.Span.Start));
        }

        return new ObjectInvocationInstruction(callable, parameters, startToken.Span.Start);
    }
}