using BakedEnv.Objects;

namespace BakedEnv.Interpreter.Instructions;

/// <summary>
/// An interface usually used in classes that derive <see cref="InterpreterInstruction"/>.
/// Defines a return value which is passed to a target.
/// </summary>
public interface IScriptTermination
{
    /// <summary>
    /// The return value (i.e. 0 for successful execution.)
    /// </summary>
    public BakedObject ReturnValue { get; }
}