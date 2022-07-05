using System.IO.Compression;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BakedEnv.CLI.Workspaces;

public class WorkspaceInterface
{
    public const string WorkspaceDirectory = ".benw";
    public const string GlobalWorkspaceDirectory = ".beng";
    public const string ConfigFile = "config.json";
    public const string LibraryDirectory = "lib";
    public static byte[] MagicHeader => Encoding.ASCII.GetBytes("BENW");
    // need refernece to direcytory its placed in or smthn
    [JsonIgnore]
    public List<FileInfo> LibraryAssemblies { get; }
    public InclusionMode Inclusion { get; set; }

    public WorkspaceInterface()
    {
        LibraryAssemblies = new List<FileInfo>();
    }
    
    public static WorkspaceInterface FromGlobal()
    {
        var userDirectory = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        var globalDir = Path.Join(userDirectory, GlobalWorkspaceDirectory);

        if (!Directory.Exists(globalDir))
        {
            globalDir = Directory.CreateDirectory(Path.Join(userDirectory, GlobalWorkspaceDirectory)).FullName;
        }
        
        return FromWorkspaceDirectory(globalDir);
    }

    public static WorkspaceInterface FromWorkspaceDirectory(string directory)
    {
        EnsureStructure(directory);
        
        var configJson = JObject.Load(new JsonTextReader(File.OpenText(Path.Join(directory, ConfigFile))));
        var workspace = configJson.ToObject<WorkspaceInterface>() ?? new WorkspaceInterface();
        
        
        
        return workspace;
    }

    public static WorkspaceInterface Unpack(string packFile)
    {
        
    }

    public void Export(string directory)
    {
        var workspaceDir = Directory.CreateDirectory(Path.Join(directory, WorkspaceDirectory));
        var libDir = workspaceDir.CreateSubdirectory(LibraryDirectory);
        using var configFile = File.CreateText(Path.Join(workspaceDir.FullName, ConfigFile));
        
        configFile.Write(CreateJson().ToString());
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
        
        writer.Write(Compress(CreateJson().ToString()));

        // Write files
        
        // Write libraries
    }

    public JObject CreateJson()
    {
        dynamic json = new JObject();

        json.InclusionMode = Inclusion;

        return json;
    } // wtf https://stackoverflow.com/a/18246895/16324801

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