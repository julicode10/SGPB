using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SGPB.Web.Migrations
{
    public partial class AddLendingEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Lendings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateStatus = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LendingStatus = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lendings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Lendings_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "LendingDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookId = table.Column<int>(type: "int", nullable: true),
                    Quantity = table.Column<float>(type: "real", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LendingId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LendingDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LendingDetails_Books_BookId",
                        column: x => x.BookId,
                        principalTable: "Books",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_LendingDetails_Lendings_LendingId",
                        column: x => x.LendingId,
                        principalTable: "Lendings",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_LendingDetails_BookId",
                table: "LendingDetails",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_LendingDetails_LendingId",
                table: "LendingDetails",
                column: "LendingId");

            migrationBuilder.CreateIndex(
                name: "IX_Lendings_UserId",
                table: "Lendings",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LendingDetails");

            migrationBuilder.DropTable(
                name: "Lendings");
        }
    }
}
