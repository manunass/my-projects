using Microsoft.EntityFrameworkCore.Migrations;

namespace GreenCoinService.Migrations.Migrations
{
    public partial class Modification1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Batches_Recyclables_RecyclableId",
                table: "Batches");

            migrationBuilder.AddForeignKey(
                name: "FK_Batches_Recyclables_RecyclableId",
                table: "Batches",
                column: "RecyclableId",
                principalTable: "Recyclables",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Batches_Recyclables_RecyclableId",
                table: "Batches");

            migrationBuilder.AddForeignKey(
                name: "FK_Batches_Recyclables_RecyclableId",
                table: "Batches",
                column: "RecyclableId",
                principalTable: "Recyclables",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
