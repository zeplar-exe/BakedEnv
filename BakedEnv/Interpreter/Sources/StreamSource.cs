using System.Text;

namespace BakedEnv.Interpreter.Sources;

public class StreamSource : IBakedSource
{
    public Stream Stream { get; }
    public Encoding Encoding { get; }

    public StreamSource(Stream stream, Encoding encoding)
    {
        if (!stream.CanRead)
            throw new ArgumentException("Expected a non-write-only stream.");

        Stream = stream;
        Encoding = encoding;
    }

    public IEnumerable<char> EnumerateCharacters()
    {
        while (Stream.Position != Stream.Length)
        {
            var buffer = new byte[Encoding.GetMaxByteCount(1)];

            Stream.Read(buffer, 0, 1);
            
            yield return Encoding.GetString(buffer).First();
        }
    }

    public void Reset()
    {
        Stream.Position = 0;
    }
}