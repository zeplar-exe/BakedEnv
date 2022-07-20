using BakedEnv.Interpreter.Expressions;
using BakedEnv.Interpreter.Instructions;
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
    }

    private bool IsProcessorStatementStarter(LexerToken token)
    {
        return token.Type is LexerTokenType.LeftBracket;
    }
}