﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;
using Confluent.Kafka.Serialization;
using Microsoft.Extensions.Options;
using Crm.Utils.Json;
using Message = Crm.Infrastructure.MessageBroking.Models.Message;

namespace Crm.Infrastructure.MessageBroking.Consuming
{
    public class Consumer : IConsumer
    {
        private readonly Consumer<Null, string> _consumer;
        private bool _isWorking;

        public Consumer(IOptions<ConsumerSettings> options)
        {
            var kafkaConfig = new Dictionary<string, object>
            {
                {"bootstrap.servers", options.Value.Host},
                {"enable.auto.commit", "false"},
                {"group.id", "1"}
            };

            var deserializer = new StringDeserializer(Encoding.UTF8);
            _consumer = new Consumer<Null, string>(kafkaConfig, null, deserializer);
        }

        public void Consume(string topic, Action<Message, CancellationToken> func)
        {
            Consume(topic, (x, y) => Task.FromResult(func));
        }

        public void Consume(string topic, Func<Message, CancellationToken, Task> action)
        {
            _consumer.Subscribe(topic);

            _consumer.OnMessage += async (_, message) =>
            {
                var result = message.Value.FromJsonString<Message>();

                await action(result, CancellationToken.None).ConfigureAwait(false); 
                await _consumer.CommitAsync(message).ConfigureAwait(false);
            };

            _isWorking = true;

            while (_isWorking)
            {
                _consumer.Poll(100);
            }
        }

        public void UnConsume()
        {
            _consumer.Unsubscribe();
            _consumer.Dispose();
        }
    }
}