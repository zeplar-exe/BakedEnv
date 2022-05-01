namespace BakedEnv.Objects.Interfaces;

public interface IBakedCallable
{
    public BakedObject? Invoke(BakedObject?[] parameters);
}