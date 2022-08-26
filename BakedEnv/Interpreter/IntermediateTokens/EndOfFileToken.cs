namespace BakedEnv.Interpreter.IntermediateTokens;

public class EndOfFileToken : IntermediateToken
{
    public override ulong StartIndex { get; }
    public override int Length { get; }
    public override ulong EndIndex { get; }

    public EndOfFileToken(ulong index)
    {
        StartIndex = index;
        Length = 0;
        EndIndex = index;
    }
}