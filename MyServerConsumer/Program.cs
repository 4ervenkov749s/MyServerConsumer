using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyServerConsumer
{
    class Program
    {
        static void Main(string[] args)

        {
            var factory = new ConnectionFactory()
            {
                HostName = "localhost"
            };

            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.ExchangeDeclare("person", ExchangeType.Fanout);

                    channel.QueueDeclare("person", durable: true, exclusive: false, autoDelete: false);

                    var consumer = new EventingBasicConsumer(channel);

                    consumer.Received += (sender, ea) =>
                    {
                        var body = ea.Body.ToArray();

                        var message = Encoding.UTF8.GetString(body);

                        Console.WriteLine(message);
                    };
                    
                    channel.BasicConsume(queue: "person", autoAck: true, consumer: consumer);
                    while(true)
                    {

                    }
                }
            }
            //Console.ReadLine();
        }
        
    }
}
