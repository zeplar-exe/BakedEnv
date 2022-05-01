using BakedEnv.ExternalApi;
using BakedEnv.Interpreter;
using BakedEnv.Interpreter.Instructions;
using BakedEnv.Interpreter.Sources;
using BakedEnv.Objects;
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
    public Dictionary<string, BakedMethod> GlobalMethods { get; }
    /// <summary>
    /// Global variables accessible anywhere within an executed script.
    /// </summary>
    public Dictionary<string, BakedObject> GlobalVariables { get; }
    
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
        GlobalMethods = new Dictionary<string, BakedMethod>();
        GlobalVariables = new Dictionary<string, BakedObject>();
        ApiStructures = new List<ApiStructure>();
        DefaultBakeType = BakeType.Script;
    }

    /// <summary>
    /// Assign true and false to their respective variables.
    /// </summary>
    /// <returns></returns>
    public BakedEnvironment WithBooleanVariables()
    {
        GlobalVariables["true"] = new BakedBoolean(true);
        GlobalVariables["false"] = new BakedBoolean(false);

        return this;
    }
    
    /// <summary>
    /// Begin interpreting an <see cref="IBakedSource"/> .
    /// </summary>
    /// <param name="source">Implementation of an IBakedSource to interpret.</param>
    /// <returns>An enumeration of each instruction interpreted. Can be used for debugging purposes.</returns>
    public IEnumerable<InterpreterInstruction> Invoke(IBakedSource source)
    {
        var interpreter = new BakedInterpreter()
            .WithSource(source)
            .WithDefaultStatementHandler();

        interpreter.Init();
        
        // TODO: Global method to get api structure by name

        while (interpreter.TryGetNextInstruction(out var instruction))
            yield return instruction;
    }
}