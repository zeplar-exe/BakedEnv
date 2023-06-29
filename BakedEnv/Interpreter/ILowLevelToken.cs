namespace BakedEnv.Interpreter;

public interface ILowLevelToken
{
    public long StartIndex { get; }
    public long Length { get; }
    public long EndIndex { get; }
}