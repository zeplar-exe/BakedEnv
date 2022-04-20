namespace BakedEnv.ExternalApi;

[Flags]
public enum StructureDiffHandler
{
    None = 0,
    
    CreateMissingFromStructure = 1<<0,
    DeleteMissingFromType = 1<<1
}