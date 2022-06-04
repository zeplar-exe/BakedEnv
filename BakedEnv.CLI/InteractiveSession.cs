namespace BakedEnv.CLI;

public class InteractiveSession : IDisposable
{
    public void Start()
    {
        Console.Clear();
        Console.Write(InteractiveScreens.InteractiveMain);
        
        while (Console.ReadKey(true).Key != ConsoleKey.X);
    }
    
    public void Dispose()
    {
        
    }
}