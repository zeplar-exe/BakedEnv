using CommandLine;

namespace BakedEnv.CLI;

public class CommandArgs
{
    [Verb("execute", HelpText = "Execute", ResourceType = typeof(HelpTextResources))]
    public class ExecuteArgs
    {
        [Option('f', "file", SetName = "Source", Default = null, 
            HelpText = "ExecuteFilePath", ResourceType = typeof(HelpTextResources))]
        public string? FilePath { get; set; }
        
        [Option('r', "raw", SetName = "Source", Default = null, 
            HelpText = "ExecuteRawString", ResourceType = typeof(HelpTextResources))]
        public string? RawString { get; set; }
        
        [Option('i', "interactive", HelpText = "ExecuteInteractive", ResourceType = typeof(HelpTextResources))]
        public bool Interactive { get; set; }
        
        [Option('d', "debug", HelpText = "ExecuteDebug", ResourceType = typeof(HelpTextResources))]
        public bool Debug { get; set; }
    }
}