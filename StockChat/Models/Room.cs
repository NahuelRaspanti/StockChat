using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StockChat.Models
{
    public class Room
    {
        public int RoomId { get; set; }
        public string Name { get; set; }
        public virtual IEnumerable<StockUser> Users { get; set; }
    }
}
