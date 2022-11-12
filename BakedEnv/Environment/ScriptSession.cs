using System.Diagnostics.CodeAnalysis;

using BakedEnv.Interpreter;
using BakedEnv.Interpreter.Instructions;
using BakedEnv.Objects;
using BakedEnv.Sources;
using BakedEnv.Variables;

namespace BakedEnv.Environment;

/// <summary>
/// Singular unit of a BakedEnv script session. 
/// </summary>
public sealed class ScriptSession : IDisposable
{
    private bool Disposed { get; set; }
    
    /// <summary>
    /// Interpreter used by the session.
    /// </summary>
    public BakedInterpreter Interpreter { get; }

    public VariableContainer TopVariables => Interpreter.Context.Variables;

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
    /// Execute instructions from the interpreter until an <see cref="IScriptTermination"/> is reached.
    /// </summary>
    /// <returns>The returned value or void.</returns>
    public BakedObject? ExecuteUntilTermination()
    {
        IScriptTermination? termination = null;
        
        ExecuteUntil(delegate(InterpreterInstruction instruction)
        {
            if (instruction is IScriptTermination t)
                termination = t;

            return termination != null;
        });

        return termination?.ReturnValue;
    }

    /// <summary>
    /// Execute instructions from the interpreter without regard for termination.
    /// </summary>
    public void ExecuteUntilEnd()
    {
        ExecuteUntil(_ => false);
    }

    public void ExecuteUntilError()
    {
        ExecuteUntil(_ => Interpreter.Error.AnyError);
    }

    /// <summary>
    /// Execute instructions until the predicate returns true.
    /// </summary>
    /// <param name="predicate">Instruction conditional function.</param>
    public void ExecuteUntil(Func<InterpreterInstruction, bool> predicate)
    {
        AssertNotDisposed();

        foreach (var instruction in EnumerateInstructions())
        {
            if (predicate.Invoke(instruction))
                break;
            
            instruction.Execute(Interpreter);
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
        AssertNotDisposed();
        
        while (Interpreter.TryGetNextInstruction(out var instruction))
        {
            if (executionMode == AutoExecutionMode.BeforeYield)
                instruction.Execute(Interpreter);
            
            yield return instruction;
        }
    }

    /// <summary>
    /// Attempt to get the next instruction.
    /// </summary>
    /// <param name="instruction">The output instruction.</param>
    /// <returns>Whether the attempt was successful.</returns>
    public bool TryGetNextInstruction([NotNullWhen(true)] out InterpreterInstruction? instruction)
    {
        AssertNotDisposed();
        
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
        AssertNotDisposed();
        
        OnDisposing?.Invoke(this, EventArgs.Empty);
        Interpreter.Dispose();
        Disposed = true;
    }

    private void AssertNotDisposed()
    {
        if (Disposed)
            throw new ObjectDisposedException(nameof(ScriptSession), "Session has already been disposed.");
    }
}