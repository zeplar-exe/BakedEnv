using BakedEnv.CLI.Workspaces;
using McMaster.Extensions.CommandLineUtils;

namespace BakedEnv.CLI.CommandArgs;

[Command("workspace")]
[Subcommand(
    typeof(Init),
    typeof(Del))]
public class Workspace
{
    [Command("init")]
    public class Init
    {
        [Option("-f --force", CommandOptionType.NoValue)]
        public bool Force { get; set; }
        
        public int OnExecute(CommandLineApplication app)
        {
            var workspacePath = Path.Join(Directory.GetCurrentDirectory(), WorkspaceInterface.WorkspaceDirectoryName);
    
            if (Directory.Exists(workspacePath))
            {
                if (Force)
                {
                    Directory.Delete(workspacePath, true);
                }
                else
                {
                    Console.WriteLine("A BakedEnv workspace already exists in this directory.");
            
                    return 1;
                }
            }

            var directory = Directory.CreateDirectory(workspacePath);
            directory.Attributes |= FileAttributes.Hidden;
    
            var workspace = new WorkspaceInterface();

            workspace.Export(Directory.GetCurrentDirectory());
    
            return 0;
        }
    }

    [Command("del")]
    public class Del
    {
        [Option("-r --recursive", CommandOptionType.NoValue)]
        public bool Recursive { get; set; }
        
        public int OnExecute(CommandLineApplication app)
        {
            var workspacePath = Path.Join(Directory.GetCurrentDirectory(), WorkspaceInterface.WorkspaceDirectoryName);

            if (Directory.Exists(workspacePath))
            {
                Directory.Delete(workspacePath, true);
            }
            else if (Recursive)
            {
                var directory = new DirectoryInfo(Directory.GetCurrentDirectory());
                var name = directory.FullName;

                while (Directory.Exists(name))
                {
                    name = Path.Join(name, "..");
                    workspacePath = Path.Join(name, WorkspaceInterface.WorkspaceDirectoryName);

                    if (Directory.Exists(workspacePath))
                    {
                        Directory.Delete(workspacePath, true);

                        return 0;
                    }
                }
        
                Console.WriteLine("A BakedEnv workspace was not found in this directory tree.");

                return 1;
            }
            else
            {
                Console.WriteLine("A BakedEnv workspace does not exist in this directory.");
        
                return 1;
            }
    
            return 0;
        }
    }
}