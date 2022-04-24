using System.Text;

namespace BakedEnv.Interpreter.Sources;

/// <summary>
/// A baked source which reads from a stream with the specified encoding.
/// </summary>
public class StreamSource : IBakedSource
{
    /// <summary>
    /// Target stream.
    /// </summary>
    public Stream Stream { get; }
    /// <summary>
    /// 
    /// </summary>
    public Encoding Encoding { get; }
    
    /// <exception cref="ArgumentException">The provided stream is write-only is otherwise inaccessible for reading.</exception>
    public StreamSource(Stream stream, Encoding encoding)
    {
        if (!stream.CanRead)
            throw new ArgumentException("Expected a non-write-only stream.");

        Stream = stream;
        Encoding = encoding;
    }

    /// <inheritdoc />
    /// <remarks>
    /// The stream's position is reset upon reaching its end.
    /// </remarks>
    public IEnumerable<char> EnumerateCharacters()
    {
        var index = 0;
        
        while (index != Stream.Length)
        {
            var buffer = new byte[Encoding.GetMaxByteCount(1)];

            index += Stream.Read(buffer, 0, 1);
            
            yield return Encoding.GetString(buffer).First();
        }

        Stream.Position = 0;
    }

    public void Reset()
    {
        Stream.Position = 0;
    }
}