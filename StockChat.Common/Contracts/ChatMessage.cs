using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockChat.Common.Contracts
{
    public class ChatMessage
    {
        public int UserId { get; set; }
        public int RoomId { get; set; }
        public string Message { get; set; }
        public DateTime TimeStamp { get; set; }
        public bool IsSuccessful { get; set; }
    }
}
