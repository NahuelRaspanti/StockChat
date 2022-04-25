using StockChat.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StockChat.Repositories
{
    public interface IRoomRepository
    {
        public Task<List<Room>> GetRooms(string userId);
    }
}
