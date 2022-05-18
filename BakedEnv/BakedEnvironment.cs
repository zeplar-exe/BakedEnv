using BakedEnv.ExternalApi;
using BakedEnv.Interpreter;
using BakedEnv.Interpreter.Instructions;
using BakedEnv.Interpreter.Sources;
using BakedEnv.Objects;

namespace BakedEnv;

/// <summary>
/// An script environment to load and execute scripts.
/// </summary>
public class BakedEnvironment
{
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
    
    public List<IErrorHandler> ErrorHandlers { get; }

    /// <summary>
    /// Instantiate a BakedEnvironment.
    /// </summary>
    public BakedEnvironment()
    {
        DefaultBakeType = BakeType.Script;
        GlobalVariables = new Dictionary<string, BakedObject>();
        ApiStructures = new List<ApiStructure>();
        ErrorHandlers = new List<IErrorHandler>();
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

    public BakedEnvironment WithErrorHandlers(params IErrorHandler[] errorHandler)
    {
        ErrorHandlers.AddRange(errorHandler);

        return this;
    }

    /// <summary>
    /// Begin interpreting an <see cref="IBakedSource"/> .
    /// </summary>
    /// <param name="source">Implementation of an IBakedSource to interpret.</param>
    /// <param name="executionMode"><see cref="AutoExecutionMode"/> used during invocation.</param>
    /// <returns>An enumeration of each instruction interpreted. Can be used for debugging purposes.</returns>
    /// <remarks><see cref="BakedInterpreter.WithEnvironment"/> and
    /// <see cref="BakedInterpreter.WithDefaultStatementHandler()"/> are used during initiation.</remarks>
    public IEnumerable<InterpreterInstruction> Invoke(IBakedSource source, AutoExecutionMode executionMode)
    {
        var interpreter = new BakedInterpreter()
            .WithEnvironment(this)
            .WithDefaultStatementHandler()
            .WithSource(source);
        
        interpreter.ErrorReported += (sender, error) => ErrorHandlers.ForEach(e => e.HandleError(error, interpreter));
        
        interpreter.Init();

        // TODO: Global method to get api structure by name

        while (interpreter.TryGetNextInstruction(out var instruction))
        {
            if (executionMode == AutoExecutionMode.BeforeYield)
                instruction.Execute(interpreter);
            
            yield return instruction;
            
            if (executionMode == AutoExecutionMode.AfterYield)
                instruction.Execute(interpreter);
        }
    }
}