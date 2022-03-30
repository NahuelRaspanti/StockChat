using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StockChat.Models
{
    public class Message
    {
        public int MessageId { get; set; }
        public string UserId { get; set; }
        public string Post { get; set; }
        public virtual StockUser User { get; set; }
        public DateTime TimeStamp { get; set; }
        public int RoomId { get; set; }
    }
}
