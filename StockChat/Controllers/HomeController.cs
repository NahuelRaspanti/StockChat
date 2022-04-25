using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StockChat.Data;
using StockChat.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using StockChat.Common.Contracts;
using Microsoft.EntityFrameworkCore;
using StockChat.Helper;
using System.Security.Claims;
using StockChat.ViewModels;
using Microsoft.AspNetCore.SignalR;
using StockChat.Hubs;
using StockChat.Repositories;

namespace StockChat.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IRoomRepository _roomRepository;
        private readonly IMessageRepository _messageRepository;
        public HomeController(IPublishEndpoint publishEndpoint, IRoomRepository roomRepository, IMessageRepository messageRepository)
        {
            _publishEndpoint = publishEndpoint;
            _roomRepository = roomRepository;
            _messageRepository = messageRepository;
        }

        public async Task<IActionResult> Index(int? id)
        {
            var viewModel = new RoomMessagesView();
            var rooms = await _roomRepository.GetRooms(User.GetUserId());

            Room room;
            if (id == null)
            {
                room = rooms.First();
            }
            else
            {
                room = rooms.FirstOrDefault(e => e.RoomId == id);

                if (room == null)
                    return Redirect("/");

            }

            var messages = await _messageRepository.GetMessagesByRoomId(room.RoomId);

            viewModel.SelectedRoom = room.Name; 
            viewModel.RoomId = room.RoomId.ToString();
            viewModel.Rooms = rooms;
            viewModel.Messages = messages;
            return View(viewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> PostMessage(string message, int roomId)
        {
            if (string.IsNullOrEmpty(message))
            {
                return BadRequest();
            }
            //Handle bot message
            if (message.StartsWith('/'))
            {
                var msg = new MessageContract
                {
                    Message = message,
                    RoomId = roomId
                };

                await _publishEndpoint.Publish(msg);
            }
            else
            {
                var msg = new Message
                {
                    Post = message,
                    RoomId = roomId,
                    TimeStamp = DateTime.Now,
                    UserId = User.GetUserId()
                };

                await _messageRepository.SaveMessage(msg);
            }
            return Ok();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
