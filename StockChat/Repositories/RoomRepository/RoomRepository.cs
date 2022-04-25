using Microsoft.EntityFrameworkCore;
using StockChat.Data;
using StockChat.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StockChat.Repositories
{
    public class RoomRepository : IRoomRepository
    {
        private readonly ApplicationDbContext _context;
        public RoomRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Room>> GetRooms(string userId)
        {
            var result = await _context.Rooms.Include(e => e.Users).Where(e => e.Users.Any(e => e.Id == userId)).OrderBy(e => e.RoomId).ToListAsync();

            return result;
        }
    }
}
