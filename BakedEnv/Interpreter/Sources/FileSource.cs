using System.Text;

namespace BakedEnv.Interpreter.Sources;

/// <summary>
/// A baked source which pulls characters from a file.
/// </summary>
public class FileSource : IBakedSource
{
    /// <summary>
    /// The target file.
    /// </summary>
    public FileInfo File { get; }
    /// <summary>
    /// The encoding to use when translating from bytes to characters.
    /// </summary>
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

    /// <inheritdoc />
    public IEnumerable<char> EnumerateCharacters()
    {
        using var stream = File.OpenRead();
        var index = 0;
        
        while (index != stream.Length)
        {
            var buffer = new byte[Encoding.GetMaxByteCount(1)];

            index += stream.Read(buffer, 0, 1);
            
            yield return Encoding.GetString(buffer).First();
        }
    }
}