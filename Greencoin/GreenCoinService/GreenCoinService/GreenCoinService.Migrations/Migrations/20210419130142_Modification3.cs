using Microsoft.EntityFrameworkCore.Migrations;

namespace GreenCoinService.Migrations.Migrations
{
    public partial class Modification3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BusinessCashoutTreshold",
                table: "Municipalities");

            migrationBuilder.RenameColumn(
                name: "UserCashoutTreshold",
                table: "Municipalities",
                newName: "CoinsCashoutTreshold");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CoinsCashoutTreshold",
                table: "Municipalities",
                newName: "UserCashoutTreshold");

            migrationBuilder.AddColumn<int>(
                name: "BusinessCashoutTreshold",
                table: "Municipalities",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
