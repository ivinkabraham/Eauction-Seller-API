using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace Eauction_Seller_API.MessageSharer
{
    public class RabbitMQListener : IRabbitMQListener
    {
        private readonly ConnectionFactory _connectionFactory;
        private IConnection _connection;
        private IModel channel;
        private string exchangeName = "BidQueue";

        public RabbitMQListener(ConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
            CreateChannel();
        }

        private void CreateChannel()
        {
            if (_connection == null || _connection.IsOpen == false)
                _connection = _connectionFactory.CreateConnection();

            if (channel == null || channel.IsOpen == false)
            {
                channel = _connection.CreateModel();

                channel.QueueDeclare(queue: exchangeName,
                                  durable: false,
                                  exclusive: false,
                                  autoDelete: false,
                                  arguments: null);
            }
        }

        public void Receive()
        {
            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += Consumer_Received;

            var consumertag = channel.BasicConsume(queue: exchangeName,
                                 autoAck: true,
                                 consumer: consumer);
            channel.BasicCancel(consumertag);

        }

        private void Consumer_Received(object sender, BasicDeliverEventArgs e)
        {
            var body = e.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
        }
    }
}
