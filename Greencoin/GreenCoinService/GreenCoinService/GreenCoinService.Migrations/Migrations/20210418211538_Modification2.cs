using Microsoft.EntityFrameworkCore.Migrations;

namespace GreenCoinService.Migrations.Migrations
{
    public partial class Modification2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "UnixTimeCompleted",
                table: "Requests",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UnixTimeCompleted",
                table: "Requests");
        }
    }
}
