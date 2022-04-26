using System.Reflection;

namespace BakedEnv;

/// <summary>
/// Interface to expose GetProperty/GetMethod methods.
/// Used during runtime value lookups.
/// </summary>
public interface IAccessible
{
    /// <summary>
    /// Get a property's value by its name.
    /// </summary>
    /// <param name="name">Name of the requested property.</param>
    /// <returns>The property's value, or null if it is an invalid proeprty.</returns>
    public object? GetPropertyValue(string name);
    /// <summary>
    /// Get an object method by its name.
    /// </summary>
    /// <param name="name">Name of the requested method.</param>
    /// <returns>The method, if it exists, or null.</returns>
    public MethodInfo? GetMethod(string name);
}