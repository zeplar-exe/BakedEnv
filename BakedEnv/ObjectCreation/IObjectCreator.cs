using System.Diagnostics.CodeAnalysis;
using BakedEnv.Objects;

namespace BakedEnv.ObjectCreation;

/// <summary>
/// <see cref="BakedObject"/> creator used during parsing.
/// </summary>
public interface IObjectCreator
{
    /// <summary>
    /// Attempt to create a <see cref="BakedObject"/> from an object.
    /// </summary>
    /// <param name="o">Object to test against.</param>
    /// <param name="bakedObject">Object to output, or null.</param>
    /// <returns>Whether the object was created.</returns>
    public bool TryCreate(object? o, [NotNullWhen(true)] out BakedObject? bakedObject);
}