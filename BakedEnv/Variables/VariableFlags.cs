namespace BakedEnv.Variables;

[Flags]
public enum VariableFlags
{
    None = 1<<0,
    ReadOnly = 1<<1
}