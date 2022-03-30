using MassTransit;
using Microsoft.Extensions.Logging;
using StockChat.Common.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockChat.Bot
{
    public class MessageConsumer : IConsumer<MessageContract>
    {
        public async Task Consume(ConsumeContext<MessageContract> context)
        {
            var msg = new ChatMessage();

            var command = context.Message.Message;

            if (!command.StartsWith("/stock"))
            {
                msg = new ChatMessage
                {
                    UserId = context.Message.UserId,
                    RoomId = context.Message.RoomId,
                    Message = "Invalid command",
                    TimeStamp = DateTime.Now
                };
                Console.WriteLine("Invalid Command");
                await context.Publish(msg);
                return;
            }

            var code = command.Split("=")[1];
            msg = await StockService.GetStock(code);

            msg.RoomId = context.Message.RoomId;
            msg.UserId = context.Message.UserId;

            Console.WriteLine(msg.IsSuccessful ? "Got stock!" : msg.Message);

            await context.Publish(msg);

            Console.Write("Bot message published");
        }
    }
}
