using Common;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StandardQueue
{
    class Program
    {
        private static ConnectionFactory _connectionFactory;
        private static IConnection _connection;
        private static IModel _model;

        private const string QueueName = "StandarQueue";

        static void Main(string[] args)
        {
            var payments = new List<Payment>();

            for (int i = 1; i <= 10; i++)
            {
                var payment = new Payment() { AmountToPay = i * 5, CardNumber = "4657342588996543", Name = "Mr. X " };
                payments.Add(payment);
            }


            CreateQueue();

            payments.ForEach(payment => SendMessage(payment));

            Console.WriteLine("========================================================================================================");

            Receive();

            Console.ReadLine();
        }

        private static void Receive()
        {
            var consumer = new QueueingBasicConsumer(_model);
            var msgCount = GetMessageCount(_model, QueueName);

            _model.BasicConsume(QueueName, true, consumer);

            var count = 0;

            while (count < msgCount)
            {
                var message = (Payment)consumer.Queue.Dequeue().Body.Deserialize(typeof(Payment));

                Console.WriteLine("<=== Received {0} {1} {2}", message.CardNumber, message.AmountToPay, message.Name);
                count++;
            }
        }

        private static uint GetMessageCount(IModel channel, string queueName)
        {
            var results = channel.QueueDeclare(queueName, true, false, false, null);

            return results.MessageCount;
        }

        private static void SendMessage(Payment payment)
        {
            _model.BasicPublish("", QueueName, null, payment.Serialize());
            Console.WriteLine("===> Payment message sent: {0} {1} {2}", payment.CardNumber, payment.AmountToPay, payment.Name);
        }

        private static void CreateQueue()
        {
            _connectionFactory = new ConnectionFactory() { HostName = "localhost", UserName = "test", Password = "test" };
            _connection = _connectionFactory.CreateConnection();
            _model = _connection.CreateModel();

            _model.QueueDeclare(QueueName, true, false, false, null);
        }
    }
}
