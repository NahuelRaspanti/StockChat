using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace StockChat.Data.Migrations
{
    public partial class seed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StockUser",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    MessageId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Post = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TimeStamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RoomId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.MessageId);
                    table.ForeignKey(
                        name: "FK_Messages_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Rooms",
                columns: table => new
                {
                    RoomId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rooms", x => x.RoomId);
                });

            migrationBuilder.CreateTable(
                name: "RoomStockUser",
                columns: table => new
                {
                    RoomsRoomId = table.Column<int>(type: "int", nullable: false),
                    UsersId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoomStockUser", x => new { x.RoomsRoomId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_RoomStockUser_AspNetUsers_UsersId",
                        column: x => x.UsersId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RoomStockUser_Rooms_RoomsRoomId",
                        column: x => x.RoomsRoomId,
                        principalTable: "Rooms",
                        principalColumn: "RoomId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "840E4731-4C38-4CE9-B7F8-CC2C6A789E27", "1", "Chatter", "Chatter" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "StockUser", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "66C6BF10-EA61-453D-8016-880D96A25D9D", 0, "38d07548-6f31-45ec-be02-59354f79202d", "stockuser1@gmail.com", true, false, null, "stockuser1@gmail.com", "stockuser1@gmail.com", "AQAAAAEAACcQAAAAEPWl7f7x8M1XYmsbRfJ5waTTowJLFagtrof68LXDSViT8NO1288TS/oEfQ0lxGrBdA==", null, false, "1f5dbfa6-1f80-4b9b-a418-b646317edbed", 0, false, "stockuser1@gmail.com" },
                    { "EB2D2CE9-39DF-4D44-9AF6-DFB2A98EBECC", 0, "8c46178a-3795-4c61-9bc7-456c197bdc3f", "stockuser2@gmail.com", true, false, null, "stockuser2@gmail.com", "stockuser2@gmail.com", "AQAAAAEAACcQAAAAECw20l2HoQLj/5DxMoJcrEbmbYd+MDJ/cg9eRtc9Sxg3/x4sAbwH/a7blyOmvdtX2Q==", null, false, "e2922ddf-28da-4197-b0b7-b2f313ee3eee", 0, false, "stockuser2@gmail.com" },
                    { "B703272D-D0C0-4080-82E3-E87D035E35D8", 0, "ebda433c-996a-401b-a748-81416db0227d", null, false, false, null, null, "StockBot", null, null, false, "497c5d21-0928-4913-8fa3-b8ec74acd71a", 0, false, "StockBot" }
                });

            migrationBuilder.InsertData(
                table: "Rooms",
                columns: new[] { "RoomId", "Name" },
                values: new object[,]
                {
                    { 1, "Room 1" },
                    { 2, "Room 2" },
                    { 3, "Room 3" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { "840E4731-4C38-4CE9-B7F8-CC2C6A789E27", "66C6BF10-EA61-453D-8016-880D96A25D9D" },
                    { "840E4731-4C38-4CE9-B7F8-CC2C6A789E27", "EB2D2CE9-39DF-4D44-9AF6-DFB2A98EBECC" },
                    { "840E4731-4C38-4CE9-B7F8-CC2C6A789E27", "B703272D-D0C0-4080-82E3-E87D035E35D8" }
                });

            migrationBuilder.InsertData(
                table: "RoomStockUser",
                columns: new[] { "RoomsRoomId", "UsersId" },
                values: new object[,]
                {
                    { 1, "66C6BF10-EA61-453D-8016-880D96A25D9D" },
                    { 2, "66C6BF10-EA61-453D-8016-880D96A25D9D" },
                    { 3, "66C6BF10-EA61-453D-8016-880D96A25D9D" },
                    { 1, "EB2D2CE9-39DF-4D44-9AF6-DFB2A98EBECC" },
                    { 2, "EB2D2CE9-39DF-4D44-9AF6-DFB2A98EBECC" },
                    { 1, "B703272D-D0C0-4080-82E3-E87D035E35D8" },
                    { 2, "B703272D-D0C0-4080-82E3-E87D035E35D8" },
                    { 3, "B703272D-D0C0-4080-82E3-E87D035E35D8" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Messages_UserId",
                table: "Messages",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_RoomStockUser_UsersId",
                table: "RoomStockUser",
                column: "UsersId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Messages");

            migrationBuilder.DropTable(
                name: "RoomStockUser");

            migrationBuilder.DropTable(
                name: "Rooms");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "840E4731-4C38-4CE9-B7F8-CC2C6A789E27", "66C6BF10-EA61-453D-8016-880D96A25D9D" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "840E4731-4C38-4CE9-B7F8-CC2C6A789E27", "B703272D-D0C0-4080-82E3-E87D035E35D8" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "840E4731-4C38-4CE9-B7F8-CC2C6A789E27", "EB2D2CE9-39DF-4D44-9AF6-DFB2A98EBECC" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "840E4731-4C38-4CE9-B7F8-CC2C6A789E27");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "66C6BF10-EA61-453D-8016-880D96A25D9D");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "B703272D-D0C0-4080-82E3-E87D035E35D8");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "EB2D2CE9-39DF-4D44-9AF6-DFB2A98EBECC");

            migrationBuilder.DropColumn(
                name: "StockUser",
                table: "AspNetUsers");
        }
    }
}
