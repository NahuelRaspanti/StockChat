using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StockChat.Models
{
    public class StockUser : IdentityUser
    {
        public virtual IEnumerable<Message> Messages { get; set; }
        public virtual IEnumerable<Room> Rooms { get; set; }
    }
}
