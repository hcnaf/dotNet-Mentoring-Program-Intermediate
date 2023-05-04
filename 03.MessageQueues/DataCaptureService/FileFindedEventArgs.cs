namespace DataCaptureService;

public class FileFindedEventArgs : EventArgs
{
    public string FileName { get; set; }
    public string FullPath { get; set; }

    public FileFindedEventArgs(string fileName, string fullPath)
    {
        FileName = fileName ?? throw new ArgumentNullException(nameof(fileName));
        FullPath = fullPath ?? throw new ArgumentNullException(nameof(fullPath));
    }
}
