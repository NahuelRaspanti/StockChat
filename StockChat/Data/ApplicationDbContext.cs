using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StockChat.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace StockChat.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Message> Messages { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            SeedUsers(builder);
            SeedRoles(builder);
            SeedUserRoles(builder);
            SeedRooms(builder);

            builder.Entity<IdentityUser>()
                    .HasDiscriminator<int>("StockUser")
                    .HasValue<StockUser>(0)
                    .HasValue<IdentityUser>(1);

            builder.Entity<Room>()
                   .HasKey(e => e.RoomId);

            builder.Entity<Room>()
                   .HasMany(e => e.Users)
                   .WithMany(e => e.Rooms)
                   .UsingEntity(e => e
                                      .ToTable("RoomStockUser")
                                      .HasData(new[] 
                                      {
                                        new { UsersId = "66C6BF10-EA61-453D-8016-880D96A25D9D", RoomsRoomId = 1 },
                                        new { UsersId = "66C6BF10-EA61-453D-8016-880D96A25D9D", RoomsRoomId = 2 },
                                        new { UsersId = "66C6BF10-EA61-453D-8016-880D96A25D9D", RoomsRoomId = 3 },
                                        new { UsersId = "EB2D2CE9-39DF-4D44-9AF6-DFB2A98EBECC", RoomsRoomId = 1 },
                                        new { UsersId = "EB2D2CE9-39DF-4D44-9AF6-DFB2A98EBECC", RoomsRoomId = 2 },
                                        new { UsersId = "B703272D-D0C0-4080-82E3-E87D035E35D8", RoomsRoomId = 1 },
                                        new { UsersId = "B703272D-D0C0-4080-82E3-E87D035E35D8", RoomsRoomId = 2 },
                                        new { UsersId = "B703272D-D0C0-4080-82E3-E87D035E35D8", RoomsRoomId = 3 },
                                      }));

            builder.Entity<Message>()
                   .HasKey(a => a.MessageId);

            builder.Entity<Message>()
                   .HasOne(e => e.User).WithMany(e => e.Messages)
                   .HasForeignKey(e => e.UserId).IsRequired();

        }

        private void SeedUsers(ModelBuilder builder)
        {
            StockUser stockUser1 = new StockUser()
            {
                Id = "66C6BF10-EA61-453D-8016-880D96A25D9D",
                UserName = "stockuser1@gmail.com",
                NormalizedUserName = "stockuser1@gmail.com",
                Email = "stockuser1@gmail.com",
                NormalizedEmail = "stockuser1@gmail.com",
                EmailConfirmed = true
            };

            StockUser stockUser2 = new StockUser()
            {
                Id = "EB2D2CE9-39DF-4D44-9AF6-DFB2A98EBECC",
                UserName = "stockuser2@gmail.com",
                NormalizedUserName = "stockuser2@gmail.com",
                Email = "stockuser2@gmail.com",
                NormalizedEmail = "stockuser2@gmail.com",
                EmailConfirmed = true
            };

            StockUser bot = new StockUser()
            {
                Id = "B703272D-D0C0-4080-82E3-E87D035E35D8",
                UserName = "StockBot",
                NormalizedUserName = "StockBot"
            };

            PasswordHasher<StockUser> passwordHasher = new PasswordHasher<StockUser>();
            stockUser1.PasswordHash = passwordHasher.HashPassword(stockUser1, "stockuser1");
            stockUser2.PasswordHash = passwordHasher.HashPassword(stockUser2, "stockuser2");

            builder.Entity<StockUser>().HasData(stockUser1, stockUser2, bot);
        }

        private void SeedRoles(ModelBuilder builder)
        {
            builder.Entity<IdentityRole>().HasData(
                new IdentityRole() { Id = "840E4731-4C38-4CE9-B7F8-CC2C6A789E27", Name = "Chatter", ConcurrencyStamp = "1", NormalizedName = "Chatter" }
                );
        }

        private void SeedUserRoles(ModelBuilder builder)
        {
            builder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>() { RoleId = "840E4731-4C38-4CE9-B7F8-CC2C6A789E27", UserId = "66C6BF10-EA61-453D-8016-880D96A25D9D" },
                new IdentityUserRole<string>() { RoleId = "840E4731-4C38-4CE9-B7F8-CC2C6A789E27", UserId = "EB2D2CE9-39DF-4D44-9AF6-DFB2A98EBECC" },
                new IdentityUserRole<string>() { RoleId = "840E4731-4C38-4CE9-B7F8-CC2C6A789E27", UserId = "B703272D-D0C0-4080-82E3-E87D035E35D8" }
                );
        }

        private void SeedRooms(ModelBuilder builder)
        {
            builder.Entity<Room>().HasData(
                new Room { RoomId = 1, Name = "Room 1"},
                new Room { RoomId = 2, Name = "Room 2"},
                new Room { RoomId = 3, Name = "Room 3"}
                );
        }
    }


}
