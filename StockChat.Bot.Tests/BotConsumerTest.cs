using MassTransit.Testing;
using NUnit.Framework;
using StockChat.Common.Contracts;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace StockChat.Bot.Tests
{
    [TestFixture]
    public class BotConsumerTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task IsMessage_Consumed_AndSuccessful()
        {
            var harness = new InMemoryTestHarness();

            var consumer = harness.Consumer<MessageConsumer>();

            await harness.Start();

            try
            {
                await harness.InputQueueSendEndpoint.Send(new MessageContract
                {
                    Message = "/stock=aapl.us",
                    RoomId = 1
                });

                var message = harness.Published.Select<ChatMessage>().First().Context.Message;

                Assert.That(message.IsSuccessful, Is.True);

                Assert.That(consumer.Consumed.Select<MessageContract>().Any(), Is.True);

            }
            finally
            {
                await harness.Stop();
            }

        }

        [Test]
        public async Task IsMessage_Consumed_Throw_CommandError()
        {
            var harness = new InMemoryTestHarness();

            harness.Consumer<MessageConsumer>();

            await harness.Start();

            try
            {
                await harness.InputQueueSendEndpoint.Send(new MessageContract
                {
                    Message = "/aaaaaa",
                    RoomId = 1
                });

                var message = harness.Published.Select<ChatMessage>().First().Context.Message;

                Assert.That(message.IsSuccessful, Is.False);
                Assert.AreEqual(message.Message, "Invalid command");
            }
            finally
            {
                await harness.Stop();
            }

        }
    }
}