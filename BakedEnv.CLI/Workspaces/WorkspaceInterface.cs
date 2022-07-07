using System.IO.Compression;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BakedEnv.CLI.Workspaces;

public class WorkspaceInterface
{
    public const string WorkspaceDirectory = ".benw";
    public const string GlobalWorkspaceDirectory = ".beng";
    public const string PackExtension = ".benp";
    public const string ConfigFile = "config.json";
    public const string LibraryDirectory = "lib";
    
    public static byte[] MagicHeader => Encoding.ASCII.GetBytes("BENW");
    public static byte[] DllIdentifier => Encoding.ASCII.GetBytes("DLL");
    public static byte[] PackIdentifier => Encoding.ASCII.GetBytes("BENP");
    
    private string DirectoryPath { get; }
    
    public JsonConfig Config { get; }

    public IEnumerable<FileInfo> LibraryAssemblies
    {
        get
        {
            EnsureStructure(DirectoryPath);

            foreach (var file in Directory.EnumerateFiles(
                         Path.Join(DirectoryPath, LibraryDirectory), "*", SearchOption.AllDirectories))
            {
                yield return new FileInfo(file);
            }
        }
    }

    public IEnumerable<FileInfo> IncludedFiles
    {
        get
        {
            EnsureStructure(DirectoryPath);

            var ignoreRegex = new Regex(Config.IgnorePattern);
            
            foreach (var file in Directory.EnumerateFiles(DirectoryPath, "*", SearchOption.AllDirectories))
            {
                var info = new FileInfo(file);

                if (ignoreRegex.IsMatch(info.Name))
                    continue;

                yield return info;
            }
        }
    }

    private WorkspaceInterface(string directory, JsonConfig config)
    {
        DirectoryPath = directory;
        Config = config;
    }

    public static WorkspaceInterface FromDirectory(string directory)
    {
        var root = Directory.CreateDirectory(directory);
        var workspaceDirectory = root.CreateSubdirectory(WorkspaceDirectory);
        workspaceDirectory.Attributes |= FileAttributes.Hidden;

        return FromWorkspaceDirectory(workspaceDirectory.FullName);
    }
    
    public static WorkspaceInterface FromWorkspaceDirectory(string directory)
    {
        EnsureStructure(directory);
        
        var configJson = JObject.Load(new JsonTextReader(File.OpenText(Path.Join(directory, ConfigFile))));
        var config = configJson.ToObject<JsonConfig>() ?? new JsonConfig();
        var workspace = new WorkspaceInterface(directory, config);
        
        return workspace;
    }

    public static WorkspaceInterface Unpack(string packFile)
    {
        throw new NotImplementedException();
    }

    public void Export(string directory)
    {
        var workspaceDir = Directory.CreateDirectory(Path.Join(directory, WorkspaceDirectory));
        var libDir = workspaceDir.CreateSubdirectory(LibraryDirectory);
        using var configFile = File.CreateText(Path.Join(workspaceDir.FullName, ConfigFile));
        
        configFile.Write(Config.CreateJson().ToString());
    }
    
    public void Pack(string destinationFile)
    {
        using var stream = File.OpenRead(destinationFile);

        Pack(stream);
    }

    public void Pack(Stream stream)
    {
        var writer = new BinaryWriter(stream);
        
        writer.Write(MagicHeader);

        var compressedConfig = Compress(Config.CreateJson().ToString());
        writer.Write(compressedConfig.Length);
        writer.Write(compressedConfig);

        var files = IncludedFiles.ToArray();
        
        writer.Write(files.Length);

        foreach (var file in files)
        {
            writer.Write(file.Length);
            writer.Write(Compress(File.ReadAllText(file.FullName)));
        }

        var libraries = LibraryAssemblies.ToArray();

        writer.Write(libraries.Length);
        
        foreach (var library in libraries)
        {
            switch (library.Extension)
            {
                case PackExtension:
                    writer.Write(PackIdentifier);
                    break;
                case ".dll":
                    writer.Write(DllIdentifier);
                    break;
            }
            
            writer.Write(library.Length);
            writer.Write(File.ReadAllBytes(library.FullName));
        }
    }

    public void EnsureStructure()
    {
        EnsureStructure(DirectoryPath);
    }

    private static void EnsureStructure(string directory)
    {
        var config = Path.Join(directory, ConfigFile);

        if (!File.Exists(config))
        {
            File.Create(config).Dispose();
        }

        Directory.CreateDirectory(Path.Join(directory, LibraryDirectory));
    }
    
    private byte[] Compress(string uncompressedString)
    {
        using var uncompressedStream = new MemoryStream(Encoding.UTF8.GetBytes(uncompressedString));
        using var compressedStream = new MemoryStream();
        
        using (var compressorStream = new DeflateStream(compressedStream, CompressionLevel.Fastest, true))
        {
            uncompressedStream.CopyTo(compressorStream);
        }

        return compressedStream.ToArray();
    }
    
    private string Decompress(string compressedString)
    {
        byte[] decompressedBytes;

        var compressedStream = new MemoryStream(Convert.FromBase64String(compressedString));

        using (var decompressorStream = new DeflateStream(compressedStream, CompressionMode.Decompress))
        {
            using (var decompressedStream = new MemoryStream())
            {
                decompressorStream.CopyTo(decompressedStream);

                decompressedBytes = decompressedStream.ToArray();
            }
        }

        return Encoding.UTF8.GetString(decompressedBytes);
    }
}