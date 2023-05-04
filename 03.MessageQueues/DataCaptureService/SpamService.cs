using CommonModels;
using System.Runtime.Serialization.Formatters.Binary;
using RabbitMQ.Client;
using System.Reflection;

namespace DataCaptureService;

public class SpamService:IDisposable
{
    private const string RabbitMQHost = @"amqp://guest:guest@localhost:5672";
    private const string exchangeName = "webappExchange";
    private const int DefultMessageSize = 2048;
    private bool disposedValue;
    private readonly IConnection connection;
    private readonly IModel channel;
    private readonly int messageSize;
    private readonly BinaryFormatter binaryFormatter = new BinaryFormatter();


    public event EventHandler<MessageSendedEventArgs> MessageSended;
    public SpamService(int messageSize = DefultMessageSize)
    {
        var factory = new ConnectionFactory()
        {
            Uri = new Uri(RabbitMQHost),
        };
        connection = factory.CreateConnection();
        channel = connection.CreateModel();
        channel.ExchangeDeclare(exchangeName, ExchangeType.Fanout, true);

        this.messageSize = messageSize;
    }
    public void Send(string message)
    {
        var bytes = System.Text.Encoding.UTF8.GetBytes(message);
        channel.BasicPublish(exchangeName, string.Empty, null, bytes);
        MessageSended?.Invoke(this, new MessageSendedEventArgs(message));
    }

    public void SendFile(string filePath)
    {
        if(!File.Exists(filePath))
        {
            throw new FileNotFoundException(filePath);
        }

        ////using var file = File.OpenRead(filePath);
        using var file = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        var partSize = messageSize - CalcEmptyMessageSizeforFile(filePath);
        
        if (partSize < 1)
        {
            throw new ArgumentException("Message size is too small.");
        }

        var partCount = CalcPartCount(partSize, file.Length);
        int counter = 1;
        var fileName = Path.GetFileName(filePath);

        foreach (var part in new FileParts(file, partSize))
        {
            SendMessage(new FilePartMessage
            {
                Id = counter,
                PartsAmount = partCount,
                FileName = fileName,
                Data = part
            });
            counter += 1;
        }

        file.Close();
    }

    public void SendMessage(FilePartMessage message)
    {
        channel.BasicPublish(exchangeName, string.Empty, null, Serialize(message));
        string eventMessage = $"Part #{message.Id}/{message.PartsAmount} of {message.FileName} in {message.Data.Length} bytes is send.";
        MessageSended?.Invoke(this, new MessageSendedEventArgs(eventMessage));
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                connection.Close();
                channel.Close();
            }

            disposedValue = true;
        }
    }

     public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    private int CalcEmptyMessageSizeforFile(string filePath)
    {
        var model = new FilePartMessage
        {
            Id = 1,
            PartsAmount = 1,
            FileName = filePath,
            Data = Array.Empty<byte>(),
        };

        return Serialize(model).Length;
    }

    private static int CalcPartCount(long partSize, long totalSize)
        => (int)(totalSize/ partSize) + (totalSize % partSize > 0 ? 1 : 0);

    private byte[] Serialize (FilePartMessage message)
    {
        using var stream = new MemoryStream();
        binaryFormatter.Serialize(stream, message);
        return stream.ToArray();
    }
}
