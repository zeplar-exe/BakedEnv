using BakedEnv.ExternalApi;
using BakedEnv.Interpreter;

namespace BakedEnv;

/// <summary>
/// An script environment to load and execute scripts.
/// </summary>
public class BakedEnvironment
{
    /// <summary>
    /// Global methods accessible anywhere within an executed script.
    /// </summary>
    public List<ExternalMethod> GlobalMethods { get; }
    /// <summary>
    /// Global variables accessible anywhere within an executed script.
    /// </summary>
    public List<BakedVariable> GlobalVariables { get; }
    
    /// <summary>
    /// <see cref="BakeType"/> to assume when it is not specified during execution.
    /// Default value (during construction) is <see cref="BakeType.Script">BakeType.Script</see>.
    /// </summary>
    public BakeType DefaultBakeType { get; set; }
    
    /// <summary>
    /// <see cref="ApiStructure">ApiStructures</see> accessible during execution.
    /// </summary>
    public List<ApiStructure> ApiStructures { get; }

    /// <summary>
    /// Instantiate a BakedEnvironment.
    /// </summary>
    public BakedEnvironment()
    {
        GlobalMethods = new List<ExternalMethod>();
        GlobalVariables = new List<BakedVariable>();
        ApiStructures = new List<ApiStructure>();
        DefaultBakeType = BakeType.Script;
    }
    
    /// <summary>
    /// Begin interpreting an <see cref="IBakedSource"/> .
    /// </summary>
    /// <param name="source">Implementation of an IBakedSource to interpret.</param>
    /// <returns>An enumeration of each instruction interpreted. Can be used for debugging purposes.</returns>
    /// <exception cref="NotImplementedException">This method is not implemented.</exception>
    public IEnumerable<InterpreterInstruction> Invoke(IBakedSource source)
    {
        throw new NotImplementedException();
    }
}