using CommandLine;

namespace BakedEnv.CLI;

public class CommandArgs
{
    [Verb("execute", HelpText = "Execute", ResourceType = typeof(HelpTextResources))]
    public class ExecuteArgs
    {
        [Option('f', "file", Group = "Source", Default = null, 
            HelpText = "ExecuteFilePath", ResourceType = typeof(HelpTextResources))]
        public string? FilePath { get; set; }
        
        [Option('r', "raw", Group = "Source", Default = null, 
            HelpText = "ExecuteRawString", ResourceType = typeof(HelpTextResources))]
        public string? RawString { get; set; }

        [Option('s', "silent", HelpText = "ExecuteSilent", ResourceType = typeof(HelpTextResources))]
        public bool Silent { get; set; }
    }

    [Verb("interactive", HelpText = "Interactive", ResourceType = typeof(HelpTextResources))]
    public class InteractiveArgs
    {
        [Option('e', "exit", HelpText = "InteractiveExitMethod", ResourceType = typeof(HelpTextResources))]
        public string? ExitMethod { get; set; }
        
        [Option('s', "silent", HelpText = "InteractiveSilent", ResourceType = typeof(HelpTextResources))]
        public bool Silent { get; set; }
    }
    
    [Verb("debug", HelpText = "Debug", ResourceType = typeof(HelpTextResources))]
    public class DebugArgs
    {
        [Option('i', "interactive", HelpText = "DebugInteractive", ResourceType = typeof(HelpTextResources))]
        public bool Interactive { get; set; }
        
        [Option('f', "file", SetName = "Source", Default = null, 
            HelpText = "DebugFilePath", ResourceType = typeof(HelpTextResources))]
        public string? FilePath { get; set; }
        
        [Option('r', "raw", SetName = "Source", Default = null, 
            HelpText = "DebugRawString", ResourceType = typeof(HelpTextResources))]
        public string? RawString { get; set; }
    }
}