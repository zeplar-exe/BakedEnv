using System.Text;

namespace BakedEnv.Interpreter.Sources;

public class FileSource : IBakedSource
{
    public FileInfo File { get; }
    public Encoding Encoding { get; }

    public FileSource(string filePath, Encoding encoding)
    {
        File = new FileInfo(filePath);
        Encoding = encoding;
    }
    
    public FileSource(FileInfo file, Encoding encoding)
    {
        File = file;
        Encoding = encoding;
    }
    
    public IEnumerable<char> EnumerateCharacters()
    {
        var stream = File.OpenRead();
        
        while (stream.Position != stream.Length)
        {
            var buffer = new byte[Encoding.GetMaxByteCount(1)];

            stream.Read(buffer, 0, 1);
            
            yield return Encoding.GetString(buffer).First();
        }
    }
}