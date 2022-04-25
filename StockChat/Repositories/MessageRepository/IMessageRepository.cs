using StockChat.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StockChat.Repositories
{
    public interface IMessageRepository
    {
        public Task SaveMessage(Message message);
        Task<List<Message>> GetMessagesByRoomId(int roomId);
    }
}
