using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;
using NUnit.Framework;
using StockChat.Common.Contracts;
using StockChat.Controllers;
using StockChat.Data;
using StockChat.Models;
using StockChat.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StockChat.Tests
{
    [TestFixture]
    public class HomeControllerTest
    {
        private Mock<IRoomRepository> mockRoomRepository;
        private Mock<IMessageRepository> mockMessageRepository;
        private Mock<IPublishEndpoint> mockPublish;

        [SetUp]
        public void Setup()
        {
            mockRoomRepository = new Mock<IRoomRepository>();

            mockMessageRepository = new Mock<IMessageRepository>();

            mockPublish = new Mock<IPublishEndpoint>();
        }

        [Test]
        public async Task PostMessageActionResult_ReturnsOk_GivenACommandMessage()
        {
            var controller = new HomeController(mockPublish.Object, mockRoomRepository.Object, mockMessageRepository.Object);

            var result = await controller.PostMessage("/stock=aapl.us", 1);

            mockPublish.Verify(e => e.Publish(It.IsAny<MessageContract>(), It.IsAny<CancellationToken>()), Times.Once);
            Assert.IsInstanceOf(typeof(OkResult), result);
        }

        [Test]
        public async Task PostMessageActionResult_ReturnsOk_GivenMessage()
        {
            var controller = new HomeController(mockPublish.Object, mockRoomRepository.Object, mockMessageRepository.Object);

            controller.ControllerContext.HttpContext = new DefaultHttpContext
            {
                User = new TestPrincipal(new Claim("name", "John Doe"))
            };

            string postMsg = "Hello!";
            int roomId = 1;

            var msg = new Message
            {
                Post = postMsg,
                RoomId = roomId,
                TimeStamp = It.IsAny<DateTime>(),
                UserId = It.IsAny<string>()
            };

            var result = await controller.PostMessage(postMsg, roomId);

            mockPublish.Verify(e => e.Publish(It.IsAny<MessageContract>(), It.IsAny<CancellationToken>()), Times.Never);
            mockMessageRepository.Verify(e => e.SaveMessage(It.IsAny<Message>()), Times.Once);
            Assert.IsInstanceOf(typeof(OkResult), result);
        }

        [Test]
        public async Task PostMessageActionResult_ReturnsBadRequest_GivenEmptyMessage()
        {
            var controller = new HomeController(mockPublish.Object, mockRoomRepository.Object, mockMessageRepository.Object);

            controller.ControllerContext.HttpContext = new DefaultHttpContext
            {
                User = new TestPrincipal(new Claim("name", "John Doe"))
            };

            string postMsg = "";
            int roomId = 1;

            var msg = new Message
            {
                Post = postMsg,
                RoomId = roomId,
                TimeStamp = It.IsAny<DateTime>(),
                UserId = It.IsAny<string>()
            };

            var result = await controller.PostMessage(postMsg, roomId);

            mockPublish.Verify(e => e.Publish(It.IsAny<MessageContract>(), It.IsAny<CancellationToken>()), Times.Never);
            mockMessageRepository.Verify(e => e.SaveMessage(It.IsAny<Message>()), Times.Never);
            Assert.IsInstanceOf(typeof(BadRequestResult), result);
        }

        [Test]
        public async Task IndexActionResult_ReturnsView_GivenAValidRoomId()
        {
            var controller = new HomeController(mockPublish.Object, mockRoomRepository.Object, mockMessageRepository.Object);

            int roomId = 1;

            var roomList = new List<Room>
            {
                new Room { RoomId = 1, Name = "Room 1"},
                new Room { RoomId = 2, Name = "Room 2"},
                new Room { RoomId = 3, Name = "Room 3"}
            };

            var messageList = new List<Message>
            {
                new Message { MessageId = 1, Post = "Hello!", RoomId = roomId}
            };

            mockRoomRepository.Setup(e => e.GetRooms(It.IsAny<string>()).Result).Returns(roomList);
            mockMessageRepository.Setup(e => e.GetMessagesByRoomId(It.IsAny<int>()).Result).Returns(messageList);

            controller.ControllerContext.HttpContext = new DefaultHttpContext
            {
                User = new TestPrincipal(new Claim("name", "John Doe"))
            };

            var result = await controller.Index(roomId);

            mockRoomRepository.Verify(e => e.GetRooms(It.IsAny<string>()), Times.Once);
            mockMessageRepository.Verify(e => e.GetMessagesByRoomId(roomId), Times.Once);

            Assert.IsInstanceOf(typeof(ViewResult), result);

        }

        [Test]
        public async Task IndexActionResult_Redirects_GivenInvalidRoomId()
        {
            var controller = new HomeController(mockPublish.Object, mockRoomRepository.Object, mockMessageRepository.Object);

            int roomId = 4;

            var roomList = new List<Room>
            {
                new Room { RoomId = 1, Name = "Room 1"},
                new Room { RoomId = 2, Name = "Room 2"},
                new Room { RoomId = 3, Name = "Room 3"}
            };

            mockRoomRepository.Setup(e => e.GetRooms(It.IsAny<string>()).Result).Returns(roomList);

            controller.ControllerContext.HttpContext = new DefaultHttpContext
            {
                User = new TestPrincipal(new Claim("name", "John Doe"))
            };

            var result = await controller.Index(roomId);

            mockRoomRepository.Verify(e => e.GetRooms(It.IsAny<string>()), Times.Once);
            mockMessageRepository.Verify(e => e.GetMessagesByRoomId(roomId), Times.Never);

            Assert.IsInstanceOf(typeof(RedirectResult), result);

        }

        [Test]
        public async Task IndexActionResult_ReturnsView_GivenNoId()
        {
            var controller = new HomeController(mockPublish.Object, mockRoomRepository.Object, mockMessageRepository.Object);

            var roomList = new List<Room>
            {
                new Room { RoomId = 1, Name = "Room 1"},
                new Room { RoomId = 2, Name = "Room 2"},
                new Room { RoomId = 3, Name = "Room 3"}
            };

            var messageList = new List<Message>
            {
                new Message { MessageId = 1, Post = "Hello!", RoomId = 1}
            };

            mockRoomRepository.Setup(e => e.GetRooms(It.IsAny<string>()).Result).Returns(roomList);
            mockMessageRepository.Setup(e => e.GetMessagesByRoomId(It.IsAny<int>()).Result).Returns(messageList);


            controller.ControllerContext.HttpContext = new DefaultHttpContext
            {
                User = new TestPrincipal(new Claim("name", "John Doe"))
            };

            var result = await controller.Index(null);

            mockRoomRepository.Verify(e => e.GetRooms(It.IsAny<string>()), Times.Once);
            mockMessageRepository.Verify(e => e.GetMessagesByRoomId(roomList.First().RoomId), Times.Once);

            Assert.IsInstanceOf(typeof(ViewResult), result);

        }

    }

    public class TestPrincipal : ClaimsPrincipal
    {
        public TestPrincipal(params Claim[] claims) : base(new TestIdentity(claims))
        {
        }
    }

    public class TestIdentity : ClaimsIdentity
    {
        public TestIdentity(params Claim[] claims) : base(claims)
        {
        }
    }

 

}
