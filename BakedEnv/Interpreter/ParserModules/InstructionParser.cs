using BakedEnv.Interpreter.Expressions;
using BakedEnv.Interpreter.Instructions;
using BakedEnv.Interpreter.ParserModules.Expressions;
using BakedEnv.Interpreter.ParserModules.Functions;
using BakedEnv.Interpreter.ParserModules.Misc;
using BakedEnv.Interpreter.ParserModules.Values;
using BakedEnv.Interpreter.Parsers;
using TokenCs;
using ExpressionParser = BakedEnv.Interpreter.ParserModules.Expressions.ExpressionParser;

namespace BakedEnv.Interpreter.ParserModules;

internal class InstructionParser : ParserModule
{
    public InstructionParser(ParserEnvironment internals) : base(internals)
    {
        
    }

    public InstructionParserResult Parse()
    {
        var builder = new InstructionParserResult.Builder();
        
        if (!Internals.Iterator.TryPeekNext(out var first))
        {
            return builder.EndOfFile(Internals.Iterator.Current.EndIndex);
        }

        if (IsProcessorStatementStarter(first))
        {
            var processorParser = new ProcessorParser(Internals);
            var processorResult = processorParser.Parse();
            
            var instruction = new ProcessorStatementInstruction(
                processorResult.KeyValuePair.Key.Expression,
                processorResult.KeyValuePair.Value.Expression,
                processorResult.SourceIndex);

            return builder.Build(true, instruction);
        }

        var expressionParser = new ExpressionParser(Internals);
        var expressionResult = expressionParser.Parse();

        if (!expressionResult.IsComplete)
        {
            return Incomplete Instruction;
        }

        return ResolveExpression(builder, expressionResult);
    }

    private bool IsProcessorStatementStarter(LexerToken token)
    {
        return token.Type is LexerTokenType.LeftBracket;
    }

    private InstructionParserResult ResolveExpression(
        InstructionParserResult.Builder builder, 
        ExpressionParserResult expressionResult)
    {
        var expression = expressionResult.Expression;
        
        switch (expression)
        {
            case VariableExpression:
            case IndexExpression:
            {
                Internals.IteratorTools.SkipWhitespaceAndNewlines();

                if (!Internals.Iterator.TryPeekNext(out var next))
                {
                    return EOF;
                }

                if (next.Type == LexerTokenType.Equals)
                {
                    Internals.IteratorTools.SkipWhitespaceAndNewlines();
                    
                    var assignmentValueParser = new ExpressionParser(Internals);
                    var assignmentValue = assignmentValueParser.Parse();

                    if (!assignmentValue.IsComplete)
                    {
                        return Incomplete Instruction
                    }

                    switch (expression)
                    {
                        case VariableExpression variableExpression:
                        {
                            var assignment = new VariableAssignmentInstruction(
                                variableExpression.Reference, assignmentValue.Expression, 
                                expressionResult.SourceIndex);
                            
                            return builder.Build(true, assignment);
                        }
                        case IndexExpression indexExpression:
                        {
                            
                        }
                    }
                }
            }
        }
    }
}