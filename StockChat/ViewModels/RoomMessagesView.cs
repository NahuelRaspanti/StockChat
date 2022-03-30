using StockChat.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StockChat.ViewModels
{
    public class RoomMessagesView
    {
        public IEnumerable<Room> Rooms { get; set; }
        public IEnumerable<Message> Messages { get; set; }
        public string RoomId { get; set; }
        public string SelectedRoom { get; set; }
    }
}
