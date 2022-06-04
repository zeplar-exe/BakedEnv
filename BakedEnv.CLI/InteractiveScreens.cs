namespace BakedEnv.CLI;

internal static class InteractiveScreens
{
    private static string ScreenDirectory => Path.Join(Directory.GetCurrentDirectory(), "InteractiveResources/Screens");
    
    public static string InteractiveMain { get; private set; }
    
    static InteractiveScreens()
    {
        LoadAll();
    }

    public static void LoadAll()
    {
        InteractiveMain = Load("interactive_main.txt");
    }

    private static string Load(string fileName)
    {
        var file = Path.Join(ScreenDirectory, fileName);
        
        return File.Exists(file) ? File.ReadAllText(file) : string.Empty;
    }
}