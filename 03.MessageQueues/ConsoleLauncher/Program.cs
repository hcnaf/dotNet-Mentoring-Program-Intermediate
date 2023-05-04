namespace DataCaptureService;

public static class ConsoleLauncher
{
    private const string WatchingDirectoryPath = @"C:\Projects\SampleDirectory";
    public static void Main()
    {
        Console.WriteLine("Hello, World!");

        var spamService = new SpamService();
        spamService.MessageSended += (s, e) =>
        {
            Console.WriteLine(e.Message);
        };

        var dataCaptureService = new DataCaptureService(spamService, WatchingDirectoryPath);
        dataCaptureService.FileFinded += (s, e) =>
        {
            Console.WriteLine($"{e.FileName} founded.");
        };
        dataCaptureService.MessageSended += (s, e) =>
        {
            Console.WriteLine(e.Message);
        };

        EventWaitHandle waitHandle = new AutoResetEvent(false);
        Task.Run(() => dataCaptureService.Run(waitHandle));

        Console.WriteLine("Press ESC to exit.");
        while (Console.ReadKey(true).Key != ConsoleKey.Escape)
        {
            // Just waiting for escape pressing
        }

        waitHandle.Set();
    }
}
