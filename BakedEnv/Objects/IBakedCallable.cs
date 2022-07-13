using BakedEnv.Interpreter;
using BakedEnv.Interpreter.Scopes;

namespace BakedEnv.Objects;

/// <summary>
/// An interface usually paired with a <see cref="BakedObject"/>. Allows for invocation using parameters.
/// </summary>
public interface IBakedCallable
{
    /// <summary>
    /// Invoke this callable type with a list of parameters in the specified interpreter and scope.
    /// </summary>
    /// <param name="parameters">NameList to use during invocation.</param>
    /// <param name="interpreter">The target interpreter.</param>
    /// <param name="scope">The target scope.</param>
    /// <returns></returns>
    public BakedObject Invoke(BakedObject[] parameters, InvocationContext context);
}