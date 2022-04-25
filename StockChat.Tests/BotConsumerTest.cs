using MassTransit.Testing;
using NUnit.Framework;
using StockChat.Bot;
using StockChat.Common.Contracts;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace StockChat.Tests
{
    [TestFixture]
    public class BotConsumerTest
    {
        [Test]
        public async Task IsMessage_Consumed_AndSuccessful()
        {
            var harness = new InMemoryTestHarness();

            var consumer = harness.Consumer<MessageConsumer>();

            await harness.Start();

            await harness.InputQueueSendEndpoint.Send(new MessageContract
            {
                Message = "/stock=aapl.us",
                RoomId = 1
            });

            var message = harness.Published.Select<ChatMessage>().First().Context.Message;

            Assert.That(message.IsSuccessful, Is.True);

            Assert.That(consumer.Consumed.Select<MessageContract>().Any(), Is.True);

            await harness.Stop();
        }

        [Test]
        public async Task IsMessage_Consumed_Throw_CommandError()
        {
            var harness = new InMemoryTestHarness();

            harness.Consumer<MessageConsumer>();

            await harness.Start();

            await harness.InputQueueSendEndpoint.Send(new MessageContract
            {
                Message = "/aaaaaa",
                RoomId = 1
            });

            var message = harness.Published.Select<ChatMessage>().First().Context.Message;

            Assert.That(message.IsSuccessful, Is.False);
            Assert.AreEqual(message.Message, "Invalid command");

            await harness.Stop();
        }

        [Test]
        public async Task IsMessage_Consumed_Throw_StockNotFound()
        {
            var harness = new InMemoryTestHarness();

            harness.Consumer<MessageConsumer>();

            await harness.Start();

            await harness.InputQueueSendEndpoint.Send(new MessageContract
            {
                Message = "/stock=oqiwerjiodma",
                RoomId = 1
            });

            var message = harness.Published.Select<ChatMessage>().First().Context.Message;

            Assert.That(message.IsSuccessful, Is.False);
            Assert.AreEqual(message.Message, "Stock not found");

            await harness.Stop();
        }
    }
}