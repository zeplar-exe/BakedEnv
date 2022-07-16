using BakedEnv.Interpreter.Expressions;
using BakedEnv.Interpreter.Instructions;
using BakedEnv.Objects;

namespace BakedEnv.Interpreter.Parsers;

internal class InvocationParser
{
    private ParserEnvironment Internals { get; }
    private BakedExpression Expression { get; }
    
    public InvocationParser(ParserEnvironment internals, BakedExpression expression)
    {
        Internals = internals;
        Expression = expression;
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

        var instruction = ParseControlStatement(startToken, controlParameters);

        if (instruction != null)
        {
            return instruction;
        }
        
        return ParseInvocation(startToken, controlParameters);
    }

    private InterpreterInstruction? ParseControlStatement(LexerToken startToken, BakedExpression[] parameters)
    {
        if (Expression is not VariableExpression variableExpression)
            return null;
        
        if (variableExpression.Reference.Path.Count != 0 || Internals.Interpreter.Environment == null)
            return null;

        foreach (var statement in Internals.Interpreter.Environment.ControlStatements)
        {
            if (statement.Name == variableExpression.Reference.Name)
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

                Internals.State.MoveTo(ParserState.StatementBody);

                var instructions = new List<InterpreterInstruction>();

                while (true)
                    // TODO: volatile conditions here
                {
                    if (!Internals.Interpreter.TryGetNextInstruction(out var controlInstruction))
                    {
                        return new InvalidInstruction(Internals.ErrorReporter.ReportEndOfFile(Internals.Iterator.Current));
                    }

                    if (Internals.State.Current != ParserState.StatementBody)
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
        Internals.IteratorTools.SkipWhitespaceAndNewlines();

        if (Internals.Iterator.TryMoveNext(out var bracket))
        {
            if (bracket.Is(LexerTokenId.OpenCurlyBracket))
            {
                if (Expression is not VariableExpression variableExpression)
                {
                    return new InvalidInstruction(new BakedError()); // TODO: Expected variable
                }
                
                Internals.State.MoveTo(ParserState.StatementBody);

                var instructions = new List<InterpreterInstruction>();
                
                while (true)
                    // TODO: volatile conditions here
                {
                    if (!Internals.Interpreter.TryGetNextInstruction(out var controlInstruction))
                    {
                        return new InvalidInstruction(Internals.ErrorReporter.ReportEndOfFile(Internals.Iterator.Current));
                    }

                    if (Internals.State.Current != ParserState.StatementBody)
                        break;

                    instructions.Add(controlInstruction);
                }

                var variables = parameters.OfType<VariableExpression>().ToArray();

                if (variables.Length != parameters.Length)
                {
                    return new InvalidInstruction(new BakedError()); // TODO: Expected alphanumeric arguments
                }

                var names = variables.Select(v => v.Reference.Name).ToArray();
            
                return new VariableAssignmentInstruction(
                    variableExpression.Reference,
                    new ValueExpression(new BakedFunction(names)),
                    startToken.Span.Start);
            }
        }

        return new ObjectInvocationInstruction(Expression, parameters, startToken.Span.Start);
    }
}