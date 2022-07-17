using BakedEnv.Interpreter.Expressions;
using BakedEnv.Interpreter.Instructions;
using BakedEnv.Interpreter.ParserModules.Functions;
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

        switch (first.Type)
        {
            case LexerTokenType.AlphaNumeric:
            case LexerTokenType.Underscore:
            {
                var functionParser = new FunctionDeclarationParser(Internals);
                var functionResult = functionParser.Parse();

                if (functionResult.IsDeclaration)
                {
                    if (!functionResult.IsComplete)
                    {
                        return builder.BuildInvalid(new BakedError()); // TODO: ModuleResults return errors
                    }
                }
                
                break;
            }
            case LexerTokenType.LeftParenthesis:
            {
                var expressionParser = new ExpressionParser(Internals);
                var expressionResult = expressionParser.Parse();

                if (!expressionResult.IsComplete)
                {
                    return builder.BuildInvalid(new BakedError());
                }

                if (expressionResult.Expression is InvocationExpression invocation)
                {// tf is this why is it locked to parenthesis?
                    var instruction = new ObjectInvocationInstruction(
                        invocation.Expression, 
                        invocation.Parameters, 
                        0);

                    return builder.Build(true, instruction);
                }

                break;
            }
        }
    }
}