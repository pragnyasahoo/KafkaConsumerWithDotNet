using System;
using Confluent.Kafka;
using Microsoft.AspNetCore.Mvc;

namespace KafkaProducer.Controllers
{
    [Route("api/kafka")]
    [ApiController]
    public class KafkaProducerController : ControllerBase
    {
        private readonly ProducerConfig config = new ProducerConfig {   BootstrapServers = "localhost:9092" };


        private readonly string topicName = "mytesttopic";
       
        [HttpPost]
        public IActionResult Post([FromQuery] string message)
        {
            return Created(string.Empty, SendMessageToKafka(topicName, message));
        }
        private Object SendMessageToKafka(string topicName, string message)
        {
            using (var producer =
                 new ProducerBuilder<Null, string>(config).Build())
            {
                try
                {
                    return producer.ProduceAsync(topicName, new Message<Null, string> { Value = message })
                        .GetAwaiter()
                        .GetResult();
                }
                catch (Exception e)
                {
                    Console.WriteLine($" something went wrong in Producer: {e}");
                }
            }
            return null;
        }
    }
}