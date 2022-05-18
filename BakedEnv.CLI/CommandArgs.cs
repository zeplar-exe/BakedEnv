using CommandLine;

namespace BakedEnv.CLI;

public class CommandArgs
{
    [Verb("execute", HelpText = "Execute", ResourceType = typeof(HelpTextResources))]
    public class ExecuteArgs
    {
        [Option('f', "file", Group = "SourceGroup", Default = null, 
            HelpText = "ExecuteFilePath", ResourceType = typeof(HelpTextResources))]
        public string? FilePath { get; set; }
        
        [Option('r', "raw", Group = "SourceGroup", Default = null, 
            HelpText = "ExecuteRawString", ResourceType = typeof(HelpTextResources))]
        public string? RawString { get; set; }
        
        [Option('d', "debug", HelpText = "ExecuteDebug", ResourceType = typeof(HelpTextResources))]
        public bool Debug { get; set; }
    }
}