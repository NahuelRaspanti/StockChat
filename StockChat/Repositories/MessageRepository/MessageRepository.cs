using Microsoft.EntityFrameworkCore;
using StockChat.Data;
using StockChat.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StockChat.Repositories
{
    public class MessageRepository : IMessageRepository
    {
        private readonly ApplicationDbContext _context;
        public MessageRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task SaveMessage(Message message)
        {
            _context.Add(message);

            await _context.SaveChangesAsync();
        }

        public async Task<List<Message>> GetMessagesByRoomId(int roomId)
        {
            var result = await _context.Messages.Include(a => a.User).Where(e => e.RoomId == roomId).OrderBy(e => e.TimeStamp).Take(50).ToListAsync();

            return result;
        }
    }
}
