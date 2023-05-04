using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using CommonModels;

namespace DataCaptureService;

internal class FileParts : IEnumerable<byte[]>
{
    private readonly int partSize;
    private readonly FileStream fileStream;

    public FileParts(FileStream fileStream, int partSize)
    {
        this.fileStream = fileStream;
        this.partSize = partSize;
    }
    public IEnumerator<byte[]> GetEnumerator()
    {
        while (fileStream.Position < fileStream.Length)
        {
            int remainToRead = (int)(fileStream.Length - fileStream.Position);
            int bufferSize = remainToRead < partSize ? remainToRead : partSize;
            byte[] buffer = new byte[bufferSize];
            fileStream.Read(buffer, 0, bufferSize);
            yield return buffer;
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}