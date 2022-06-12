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
    /// Encoding to use when translating from bytes to characters.
    /// </summary>
    public Encoding Encoding { get; }

    /// <summary>
    /// Initialize a FileSource with a raw file path and an encoding.
    /// </summary>
    /// <param name="filePath">The target file.</param>
    /// <param name="encoding">Encoding to use when translating from bytes to characters.</param>
    public FileSource(string filePath, Encoding encoding)
    {
        File = new FileInfo(filePath);
        Encoding = encoding;
    }
    
    /// <summary>
    /// Initialize a FileSource with a FileInfo and an encoding.
    /// </summary>
    /// <param name="file">The target file.</param>
    /// <param name="encoding">Encoding to use when translating from bytes to characters.</param>
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