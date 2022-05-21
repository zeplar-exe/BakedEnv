using System.Diagnostics.CodeAnalysis;
using BakedEnv.Interpreter;
using BakedEnv.Interpreter.Instructions;
using BakedEnv.Interpreter.Sources;
using BakedEnv.Objects;

namespace BakedEnv;

public class ScriptSession : IDisposable
{
    private bool Disposed { get; set; }
    // benchmarking
    public BakedInterpreter Interpreter { get; }
    
    public event EventHandler? OnDisposing;

    public ScriptSession(BakedInterpreter interpreter)
    {
        Interpreter = interpreter;
    }
    
    public ScriptSession Init()
    {
        Interpreter.Init();

        return this;
    }

    public ScriptSession WithSource(IBakedSource source)
    {
        Interpreter.WithSource(source);

        return this;
    }

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

    public void ExecuteUntilEnd()
    {
        ExecuteUntil(_ => false);
    }

    public void ExecuteUntil(Func<InterpreterInstruction, bool> predicate)
    {
        AssertDisposed();

        foreach (var instruction in EnumerateInstructions(AutoExecutionMode.AfterYield))
        {
            if (predicate.Invoke(instruction))
                break;
        }
    }

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

    public bool TryGetNextInstruction([NotNullWhen(true)] out InterpreterInstruction? instruction)
    {
        AssertDisposed();
        
        return Interpreter.TryGetNextInstruction(out instruction);
    }
    
    public bool TryExecuteNextInstruction([NotNullWhen(true)] out InterpreterInstruction? instruction)
    {
        if (!TryGetNextInstruction(out instruction))
            return false;
        
        instruction.Execute(Interpreter);

        return true;
    }

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