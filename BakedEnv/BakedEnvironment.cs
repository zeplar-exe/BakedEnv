using BakedEnv.ExternalApi;
using BakedEnv.Interpreter;
using BakedEnv.Interpreter.Sources;
using Jammo.ParserTools;
using Jammo.ParserTools.Lexing;
using Jammo.ParserTools.Tokenization;

namespace BakedEnv;

/// <summary>
/// An script environment to load and execute scripts.
/// </summary>
public class BakedEnvironment
{
    /// <summary>
    /// Global methods accessible anywhere within an executed script.
    /// </summary>
    public List<BakedMethod> GlobalMethods { get; }
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
        GlobalMethods = new List<BakedMethod>();
        GlobalVariables = new List<BakedVariable>();
        ApiStructures = new List<ApiStructure>();
        DefaultBakeType = BakeType.Script;
    }
    
    /// <summary>
    /// Begin interpreting an <see cref="IBakedSource"/> .
    /// </summary>
    /// <param name="source">Implementation of an IBakedSource to interpret.</param>
    /// <returns>An enumeration of each instruction interpreted. Can be used for debugging purposes.</returns>
    public IEnumerable<InterpreterInstruction> Invoke(IBakedSource source)
    {
        var interpreter = new BakedInterpreter();
        interpreter.InitWith(source);
        
        interpreter.Context.BakeType = DefaultBakeType;
        interpreter.Context.Variables.AddRange(GlobalVariables);
        interpreter.Context.Methods.AddRange(GlobalMethods);
        
        // TODO: Global method to get api structure

        while (interpreter.TryGetNextInstruction(out var instruction))
            yield return instruction;
    }
}