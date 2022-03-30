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

namespace StockChat.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IHubContext<ChatHub> _hubContext;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, IPublishEndpoint publishEndpoint, IHubContext<ChatHub> hubContext)
        {
            _logger = logger;
            _context = context;
            _publishEndpoint = publishEndpoint;
            _hubContext = hubContext;
        }

        public async Task<IActionResult> Index(int? id)
        {
            var viewModel = new RoomMessagesView();
            var rooms = await _context.Rooms.Include(e => e.Users).Where(e => e.Users.Any(e => e.Id == User.GetUserId())).OrderBy(e => e.RoomId).ToListAsync();

            if (id == null)
            {
                id = rooms.First().RoomId;
            }

            var messages = await _context.Messages.Include(a => a.User).Where(e => e.RoomId == id).OrderBy(e => e.TimeStamp).Take(50).ToListAsync();

            viewModel.SelectedRoom = rooms.First(e => e.RoomId == id).Name; 
            viewModel.RoomId = id.ToString();
            viewModel.Rooms = rooms;
            viewModel.Messages = messages;
            return View(viewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public async Task<ActionResult> PostMessage(string message, int roomId)
        {

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

                _context.Messages.Add(msg);
                await _context.SaveChangesAsync();
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
