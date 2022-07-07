using Newtonsoft.Json.Linq;

namespace BakedEnv.CLI.Workspaces;

public class JsonConfig
{
    public string IgnorePattern { get; set; }
    public InclusionMode Inclusion { get; set; }
    
    public JObject CreateJson()
    {
        dynamic json = new JObject();

        json.InclusionMode = Inclusion;

        return json;
    } // wtf https://stackoverflow.com/a/18246895/16324801
}