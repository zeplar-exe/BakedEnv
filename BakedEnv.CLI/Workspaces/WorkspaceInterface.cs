using Newtonsoft.Json.Linq;

namespace BakedEnv.CLI.Workspaces;

public class WorkspaceInterface
{
    public const string WorkspaceDirectoryName = ".benw";
    
    public InclusionMode Inclusion { get; }

    public void Export(string directory)
    {
        var workspaceDir = Directory.CreateDirectory(Path.Join(directory, WorkspaceDirectoryName));
        var libDir = workspaceDir.CreateSubdirectory("lib");
        using var configFile = File.CreateText(Path.Join(workspaceDir.FullName, "config.json"));
        
        configFile.Write(CreateJson().ToString());
    }

    public JObject CreateJson()
    {
        dynamic json = new JObject();

        json.InclusionMode = Inclusion;

        return json;
    } // wtf https://stackoverflow.com/a/18246895/16324801
}