using System.Runtime.Serialization.Formatters.Binary;
using CommonModels;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace MainProcessing;

internal class Program
{
    private static readonly string RabbitMQHost = @"amqp://guest:guest@localhost:5672";
    private static readonly string channelName = "webappExchange";
    private static readonly string queueName = "mainQueue";
    private static readonly string endFolder = @"C:\Projects\RecivedFiles";
    private static BinaryFormatter binaryFormatter = new BinaryFormatter();
    private static Dictionary<string, FileStream> resivingFilesBuffer = new();

    static void Main(string[] args)
    {
        var factory = new ConnectionFactory
        {
            Uri = new Uri(RabbitMQHost),
        };
        var connection = factory.CreateConnection();
        var channel = connection.CreateModel();

        channel.QueueDeclare(queueName, true, false, false);
        channel.QueueBind(queueName, channelName, "");

        var consumer = new EventingBasicConsumer(channel);

        consumer.Received += (sender, eventArgs) =>
        {
            var message = Deserialize(eventArgs.Body.ToArray());
            CombineParts(message);
            Console.WriteLine($"Part#{message.Id}/{message.PartsAmount} of {message.FileName} in {message.Data.Length} bytes has resived.");
        };

        channel.BasicConsume(queueName, true, consumer);

        Console.ReadLine();

        channel.Close();
        connection.Close();
    }

    private static FilePartMessage Deserialize(byte[] encodedMessage)
    {
        using var memStream = new MemoryStream();
        memStream.Write(encodedMessage, 0, encodedMessage.Length);
        memStream.Seek(0, SeekOrigin.Begin);
        return (FilePartMessage)binaryFormatter.Deserialize(memStream);
    }

    private static void CombineParts(FilePartMessage message)
    {
        if (message.Id == 1)
        {
            resivingFilesBuffer[message.FileName] = File.OpenWrite(Path.Combine(endFolder, message.FileName));
        }

        resivingFilesBuffer[message.FileName].Write(message.Data);

        if (message.Id == message.PartsAmount) 
        {
            resivingFilesBuffer[message.FileName].Flush();
            resivingFilesBuffer[message.FileName].Close();
        }
    }
}