using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StockChat.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(string user, string message, string roomId)
        {
            await Clients.Group(roomId).SendAsync("ReceiveMessage", user, message);
        }

        public async Task JoinGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        }
    }
}
