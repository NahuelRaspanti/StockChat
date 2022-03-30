using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StockChat.Common.Contracts;
using StockChat.Data;
using StockChat.Models;
using StockChat.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace StockChat.Consumer
{
    public class MessageConsumer : IConsumer<ChatMessage>
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IHubContext<ChatHub> _chatHub;

        public MessageConsumer(ApplicationDbContext applicationDbContext, IHubContext<ChatHub> chatHub)
        {
            _dbContext = applicationDbContext;
            _chatHub = chatHub;
        }

        public async Task Consume(ConsumeContext<ChatMessage> context)
        {
            await _chatHub.Clients.Groups(context.Message.RoomId.ToString()).SendAsync("ReceiveMessage", "StockBot", context.Message.Message);

            if (context.Message.IsSuccessful)
            {
                var botUser = await _dbContext.Users.Where(e => e.UserName == "StockBot").SingleAsync();

                var msg = new Message
                {
                    UserId = botUser.Id,
                    RoomId = context.Message.RoomId,
                    Post = context.Message.Message,
                    TimeStamp = DateTime.Now
                };

                _dbContext.Messages.Add(msg);
                await _dbContext.SaveChangesAsync();
            }

        }
    }
}
