namespace BakedEnv.Interpreter.IntermediateTokens;

public class EndOfFileToken : IntermediateToken
{
    public override int StartIndex { get; }
    public override int Length { get; }
    public override int EndIndex { get; }

    public EndOfFileToken(int index)
    {
        StartIndex = index;
        Length = 0;
        EndIndex = index;
    }
}