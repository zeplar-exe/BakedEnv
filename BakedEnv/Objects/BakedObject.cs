namespace BakedEnv.Objects;

/// <summary>
/// A sandboxed root object type.
/// </summary>
public abstract class BakedObject : IEquatable<BakedObject>
{
    /// <summary>
    /// Get the value (if any) of this object.
    /// </summary>
    /// <returns>The raw value of this object, or null.</returns>
    public abstract object? GetValue();

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        return Equals(obj as BakedObject);
    }

    /// <inheritdoc />
    public virtual bool Equals(BakedObject? other)
    {
        if (ReferenceEquals(null, other))
            return false;

        return ReferenceEquals(this, other) || GetHashCode() == other.GetHashCode();
    }

    /// <inheritdoc />
    public abstract override int GetHashCode();

    /// <summary>
    /// Attempt to get a child object by name.
    /// </summary>
    /// <param name="name">Child name to get.</param>
    /// <param name="bakedObject">Value retrieved by name.</param>
    /// <returns>Whether a child object was found.</returns>
    public virtual bool TryGetContainedObject(string name, out BakedObject bakedObject)
    {
        bakedObject = new BakedNull();
        
        return false;
    }
    
    /// <summary>
    /// Attempt to set a child object by name.
    /// </summary>
    /// <param name="name">Child name to set.</param>
    /// <param name="bakedObject">Value to use for setting.</param>
    /// <returns>Whether a child object was found.</returns>
    public virtual bool TrySetContainedObject(string name, BakedObject bakedObject)
    {
        return false;
    }

    /// <summary>
    /// Attempt negation of this object.
    /// </summary>
    /// <param name="result">The negated form of this object.</param>
    /// <returns>Whether the object could be negated.</returns>
    public virtual bool TryNegate(out BakedObject? result) 
    { 
        result = null;
        return false;
    }
    
    /// <summary>
    /// Attempt to add <paramref name="bakedObject"/> to this object.
    /// </summary>
    /// <param name="bakedObject">The target object to add.</param>
    /// <param name="result">The mutated result of this object.</param>
    /// <returns>Whether the operation was successful.</returns>
    public virtual bool TryAdd(BakedObject bakedObject, out BakedObject? result)
    { 
        result = null;
        return false;
    }
    
    /// <summary>
    /// Attempt to subtract <paramref name="bakedObject"/> from this object.
    /// </summary>
    /// <param name="bakedObject">The target object to subtract.</param>
    /// <param name="result">The mutated result of this object.</param>
    /// <returns>Whether the operation was successful.</returns>
    public virtual bool TrySubtract(BakedObject bakedObject, out BakedObject? result)
    { 
        result = null;
        return false;
    }
    
    /// <summary>
    /// Attempt to multiply <paramref name="bakedObject"/> with this object.
    /// </summary>
    /// <param name="bakedObject">The target object to multiply with.</param>
    /// <param name="result">The mutated result of this object.</param>
    /// <returns>Whether the operation was successful.</returns>
    public virtual bool TryMultiply(BakedObject bakedObject, out BakedObject? result)
    { 
        result = null;
        return false;
    }
    
    /// <summary>
    /// Attempt to raise this object to the power of <paramref name="bakedObject"/>.
    /// </summary>
    /// <param name="bakedObject">The target object to exponentiate.</param>
    /// <param name="result">The mutated result of this object.</param>
    /// <returns>Whether the operation was successful.</returns>
    public virtual bool TryExponent(BakedObject bakedObject, out BakedObject? result)
    { 
        result = null;
        return false;
    }
    
    /// <summary>
    /// Attempt to divide <paramref name="bakedObject"/> from this object.
    /// </summary>
    /// <param name="bakedObject">The target object to divide from.</param>
    /// <param name="result">The mutated result of this object.</param>
    /// <returns>Whether the operation was successful.</returns>
    public virtual bool TryDivide(BakedObject bakedObject, out BakedObject? result)
    { 
        result = null;
        return false;
    }
    
    /// <summary>
    /// Attempt to get the remainder of <paramref name="bakedObject"/> from this object.
    /// </summary>
    /// <param name="bakedObject">The target object to perform module with.</param>
    /// <param name="result">The modulo result.</param>
    /// <returns>Whether the operation was successful.</returns>
    public virtual bool TryModulus(BakedObject bakedObject, out BakedObject? result)
    { 
        result = null;
        return false;
    }
    
    /// <summary>
    /// Attempt to compare <paramref name="bakedObject"/> as less than to this object.
    /// </summary>
    /// <param name="bakedObject">The target object to compare.</param>
    /// <param name="result">Whether the <paramref name="bakedObject"/> is less than this object.</param>
    /// <returns>Whether the operation was successful.</returns>
    public virtual bool TryLessThan(BakedObject bakedObject, out bool result)
    { 
        result = false;
        return false;
    }
    
    /// <summary>
    /// Attempt to compare <paramref name="bakedObject"/> as greater than to this object.
    /// </summary>
    /// <param name="bakedObject">The target object to compare.</param>
    /// <param name="result">Whether the <paramref name="bakedObject"/> is greater than this object.</param>
    /// <returns>Whether the operation was successful.</returns>
    public virtual bool TryGreaterThan(BakedObject bakedObject, out bool result)
    { 
        result = false;
        return false;
    }
    
    /// <summary>
    /// Attempt to compare <paramref name="bakedObject"/> as less than or equal to this object.
    /// </summary>
    /// <param name="bakedObject">The target object to compare.</param>
    /// <param name="result">Whether the <paramref name="bakedObject"/> is less than or equal this object.</param>
    /// <returns>Whether the operation was successful.</returns>
    public virtual bool TryLessThanOrEqual(BakedObject bakedObject, out bool result)
    { 
        result = false;
        return false;
    }

    /// <summary>
    /// Attempt to compare <paramref name="bakedObject"/> as greater than or equal to this object.
    /// </summary>
    /// <param name="bakedObject">The target object to compare.</param>
    /// <param name="result">Whether the <paramref name="bakedObject"/> is greater than or equal this object.</param>
    /// <returns>Whether the operation was successful.</returns>
    public virtual bool TryGreaterThanOrEqual(BakedObject bakedObject, out bool result)
    { 
        result = false;
        return false;
    }

    /// <inheritdoc />
    public abstract override string ToString();
}