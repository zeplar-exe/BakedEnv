using BakedEnv.CLI.CommandArgs;
using McMaster.Extensions.CommandLineUtils;

namespace BakedEnv.CLI;

[Command("ben")]
[Subcommand(
    typeof(Execute),
    typeof(Interactive),
    typeof(Debug))]
public class Program
{
    public static int Main(string[] args) => CommandLineApplication.Execute<Program>(args);

    private int OnExecute()
    {
        return 0;
    }
}