using System.Diagnostics.CodeAnalysis;
using BakedEnv.Interpreter;
using BakedEnv.Interpreter.Instructions;
using BakedEnv.Interpreter.Sources;
using BakedEnv.Objects;

namespace BakedEnv;

/// <summary>
/// Singular unit of a BakedEnv script session. 
/// </summary>
public class ScriptSession : IDisposable
{
    private bool Disposed { get; set; }
    
    /// <summary>
    /// Interpreter used by the session.
    /// </summary>
    public BakedInterpreter Interpreter { get; }
    
    /// <summary>
    /// Event which fires before the ScriptSession is disposed. Useful for last-minute cleanup.
    /// </summary>
    public event EventHandler? OnDisposing;

    /// <summary>
    /// Initialize a ScriptSession with an interpreter.
    /// </summary>
    /// <param name="interpreter">The interpreter to use.</param>
    public ScriptSession(BakedInterpreter interpreter)
    {
        Interpreter = interpreter;
    }
    
    /// <summary>
    /// Init the ScriptSession (and Interpreter)
    /// </summary>
    public ScriptSession Init()
    {
        Interpreter.Init();

        return this;
    }

    /// <summary>
    /// Update the interpreter's IBakedSource.
    /// </summary>
    /// <param name="source">The new source.</param>
    public ScriptSession WithSource(IBakedSource source)
    {
        Interpreter.WithSource(source);

        return this;
    }

    /// <summary>
    /// Execute instructions from the interpreter until an <see cref="IScriptTermination"/> is reached.
    /// </summary>
    /// <returns>The returned value or void.</returns>
    public BakedObject ExecuteUntilTermination()
    {
        IScriptTermination? termination = null;
        
        ExecuteUntil(delegate(InterpreterInstruction instruction)
        {
            if (instruction is IScriptTermination t)
                termination = t;

            return termination != null;
        });

        return termination?.ReturnValue ?? new BakedVoid();
    }

    /// <summary>
    /// Execute instructions from the interpreter without regard for termination.
    /// </summary>
    public void ExecuteUntilEnd()
    {
        ExecuteUntil(_ => false);
    }

    /// <summary>
    /// Execute instructions until the predicate returns true.
    /// </summary>
    /// <param name="predicate">Instruction conditional function.</param>
    public void ExecuteUntil(Func<InterpreterInstruction, bool> predicate)
    {
        AssertDisposed();

        foreach (var instruction in EnumerateInstructions(AutoExecutionMode.AfterYield))
        {
            if (predicate.Invoke(instruction))
                break;
        }
    }

    /// <summary>
    /// Enumerate instructions from the interpreter with an <see cref="AutoExecutionMode"/>, controlling
    /// how execution is handled.
    /// </summary>
    /// <param name="executionMode"></param>
    /// <returns>Enumeration of instructions from the interpreter.</returns>
    public IEnumerable<InterpreterInstruction> EnumerateInstructions(AutoExecutionMode executionMode = AutoExecutionMode.None)
    {
        AssertDisposed();
        
        while (Interpreter.TryGetNextInstruction(out var instruction))
        {
            if (executionMode == AutoExecutionMode.BeforeYield)
                instruction.Execute(Interpreter);
            
            yield return instruction;
            
            if (executionMode == AutoExecutionMode.AfterYield)
                instruction.Execute(Interpreter);
        }
    }

    /// <summary>
    /// Attempt to get the next instruction.
    /// </summary>
    /// <param name="instruction">The output instruction.</param>
    /// <returns>Whether the attempt was successful.</returns>
    public bool TryGetNextInstruction([NotNullWhen(true)] out InterpreterInstruction? instruction)
    {
        AssertDisposed();
        
        return Interpreter.TryGetNextInstruction(out instruction);
    }
    
    /// <summary>
    /// Attempt to get and execute the next instruction.
    /// </summary>
    /// <param name="instruction">The output instruction.</param>
    /// <returns>Whether the attempt was successful.</returns>
    public bool TryExecuteNextInstruction([NotNullWhen(true)] out InterpreterInstruction? instruction)
    {
        if (!TryGetNextInstruction(out instruction))
            return false;
        
        instruction.Execute(Interpreter);

        return true;
    }

    /// <summary>
    /// Dispose of the ScriptSession.
    /// </summary>
    /// <remarks>Invokes the <see cref="OnDisposing"/> event.</remarks>
    public void Dispose()
    {
        AssertDisposed();
        
        OnDisposing?.Invoke(this, EventArgs.Empty);
        Interpreter.TearDown();
        Disposed = true;
    }

    private void AssertDisposed()
    {
        if (Disposed)
            throw new ObjectDisposedException(nameof(ScriptSession), "Session has already been disposed.");
        
        Interpreter.AssertReady();
    }
}