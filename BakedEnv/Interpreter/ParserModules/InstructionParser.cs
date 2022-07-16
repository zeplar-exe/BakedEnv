using BakedEnv.Interpreter.Expressions;
using BakedEnv.Interpreter.Instructions;
using BakedEnv.Interpreter.ParserModules.Expressions;
using BakedEnv.Interpreter.ParserModules.Values;
using BakedEnv.Interpreter.Parsers;
using TokenCs;

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
                var functionParser = new FunctionValueParser(Internals);
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
                var expressionParser = new TailExpressionParser(Internals);
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

internal class InstructionParserResult : ParserModuleResult
{
    public bool IsSuccess { get; }
    public InterpreterInstruction Instruction { get; }
    
    public InstructionParserResult(bool success, IEnumerable<LexerToken> allTokens, InterpreterInstruction instruction) : base(allTokens)
    {
        IsSuccess = success;
        Instruction = instruction;
    }

    public class Builder : ResultBuilder
    {
        public Builder WithModuleResult(ParserModuleResult result)
        {
            AddTokensFrom(result);

            return this;
        }

        public InstructionParserResult EndOfFile(int index)
        {
            return BuildInvalid(new BakedError(
                ErrorCodes.EndOfFile,
                ErrorMessages.EndOfFile,
                index));
        }

        public InstructionParserResult BuildInvalid(BakedError error)
        {
            return Build(false, new InvalidInstruction(error));
        }

        public InstructionParserResult Build(bool success, InterpreterInstruction instruction)
        {
            return new InstructionParserResult(success, AllTokens, instruction);
        }
    }
}