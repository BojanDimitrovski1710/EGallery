using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EGallery.Repository.Migrations
{
    public partial class newNew : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "ProductInOrder",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "ProductInOrder",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Id",
                table: "ProductInOrder");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "ProductInOrder");
        }
    }
}
