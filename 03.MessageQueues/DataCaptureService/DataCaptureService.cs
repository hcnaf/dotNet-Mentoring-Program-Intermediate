namespace DataCaptureService;

public class DataCaptureService
{
    public event EventHandler<FileFindedEventArgs> FileFinded;
    public event EventHandler<MessageSendedEventArgs> MessageSended;
    private readonly string folderName;
    private readonly SpamService spamService;

    public DataCaptureService(SpamService spamService, string folderName)
    {
        this.spamService = spamService ?? throw new ArgumentNullException(nameof(spamService));
        this.folderName = folderName ?? throw new ArgumentNullException(nameof(folderName));
    }

    public void Run(EventWaitHandle waitingHandle)
    {
        using var watcher = new FileSystemWatcher(folderName);

        watcher.NotifyFilter = NotifyFilters.Attributes
                             | NotifyFilters.CreationTime
                             | NotifyFilters.DirectoryName
                             | NotifyFilters.FileName
                             | NotifyFilters.LastAccess
                             | NotifyFilters.LastWrite
                             | NotifyFilters.Security
                             | NotifyFilters.Size;

        watcher.Created += (s, e) => OnFileFounded(new FileFindedEventArgs(e.Name!, e.FullPath));

        watcher.EnableRaisingEvents = true;

        waitingHandle.WaitOne();
    }

    protected void OnFileFounded(FileFindedEventArgs eventArgs)
    {
        FileFinded?.Invoke(this, eventArgs);

        spamService.SendFile(eventArgs.FullPath);

        MessageSended?.Invoke(this, new MessageSendedEventArgs(":) Sended . . ."));
    }
}

public class MessageSendedEventArgs : EventArgs
{
    public string Message { get; set; }

    public MessageSendedEventArgs(string message)
    {
        Message = message;
    }
}