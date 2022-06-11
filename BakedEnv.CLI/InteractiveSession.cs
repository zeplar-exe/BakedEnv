namespace BakedEnv.CLI;

public class InteractiveSession : IDisposable
{
    public int Run()
    {
        Console.Clear();
        Console.Write(InteractiveScreens.InteractiveMain);
        
        while (Console.ReadKey(true).Key != ConsoleKey.X);

        return 0;
    }
    
    public void Dispose()
    {
        
    }
}