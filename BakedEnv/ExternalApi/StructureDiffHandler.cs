namespace BakedEnv.ExternalApi;

/// <summary>
/// Structural difference flags used in <see cref="ApiStructure.SyncObject">ApiStructure.SyncObject</see>
/// </summary>
[Flags]
public enum StructureDiffHandler
{
    /// <summary>
    /// None, nada, nunca.
    /// </summary>
    None = 0,
    
    /// <summary>
    /// Create nodes present in the target type, but not the <see cref="ApiStructure"/>.
    /// </summary>
    CreateMissingFromStructure = 1<<0,
    /// <summary>
    /// Delete nodes in the <see cref="ApiStructure"/> missing from the target type.
    /// </summary>
    DeleteMissingFromType = 1<<1
}