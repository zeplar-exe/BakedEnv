using BakedEnv.Interpreter.Parsers;
using TokenCs;

namespace BakedEnv.Interpreter.ParserModules.Common;

internal class InstructionBlockParser : ParserModule
{
    public InstructionBlockParser(InterpreterInternals internals) : base(internals)
    {
        
    }

    public InstructionBlockParserResult Parse()
    {
        var builder = new InstructionBlockParserResult.Builder();

        if (!Internals.Iterator.TryPeekNext(out var first))
        {
            return builder.Build(false);
        }

        if (first.Type != LexerTokenType.LeftCurlyBracket)
        {
            return builder.Build(false);
        }

        builder.WithOpening(first);

        var instructionParser = new InstructionParser(Internals);

        while (true)
        {
            if (!Internals.Iterator.TryMoveNext(out var token))
            {
                return builder.Build(false);
            }

            if (token.Type == LexerTokenType.RightCurlyBracket)
            {
                return builder.WithClosing(token).Build(true);
            }
            
            Internals.Iterator.PushCurrent();

            var result = instructionParser.Parse();

            builder.WithInstruction(result);
            
            if (!result.IsSuccess)
            {
                return builder.Build(false);
            }
        }
    }
}