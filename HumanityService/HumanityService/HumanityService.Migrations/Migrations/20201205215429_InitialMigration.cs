using Microsoft.EntityFrameworkCore.Migrations;

namespace HumanityService.Migrations.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.CreateTable(
                name: "locations",
                columns: table => new
                {
                    Username = table.Column<string>(maxLength: 36, nullable: false),
                    Description = table.Column<string>(nullable: false),
                    Longitude = table.Column<double>(nullable: false),
                    Latitude = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_locations", x => x.Username);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    Username = table.Column<string>(maxLength: 36, nullable: false),
                    FirstName = table.Column<string>(maxLength: 100, nullable: false),
                    LastName = table.Column<string>(maxLength: 100, nullable: false),
                    Email = table.Column<string>(maxLength: 128, nullable: false),
                    PhoneNumber = table.Column<string>(maxLength: 16, nullable: false),
                    Password = table.Column<string>(maxLength: 128, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.Username);
                });

            migrationBuilder.CreateTable(
                name: "ngos",
                columns: table => new
                {
                    Username = table.Column<string>(maxLength: 36, nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: false),
                    RegistrationNumber = table.Column<string>(maxLength: 100, nullable: false),
                    Email = table.Column<string>(maxLength: 128, nullable: false),
                    PhoneNumber = table.Column<string>(maxLength: 16, nullable: false),
                    WebsiteAddress = table.Column<string>(maxLength: 100),
                    Password = table.Column<string>(maxLength: 128, nullable: false),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ngos", x => x.Username);
                });


            migrationBuilder.CreateTable(
                name: "campaigns",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 100, nullable: false),
                    Username = table.Column<string>(maxLength: 36, nullable: false),
                    NgoName = table.Column<string>(maxLength: 100, nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: false),
                    Type = table.Column<string>(maxLength: 100, nullable: false),
                    Category = table.Column<string>(maxLength: 100, nullable: false),
                    CompletedCount = table.Column<int>(nullable: false),
                    Status = table.Column<string>(maxLength: 128, nullable: false),
                    TimeCreated = table.Column<long>(nullable: false, defaultValue: 0L),
                    TimeCompleted = table.Column<long>(nullable: false, defaultValue: 0L),
                    Description = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_campaigns", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "processes",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 100, nullable: false),
                    CampaignId = table.Column<string>(maxLength: 100, nullable: false),
                    Status = table.Column<string>(maxLength: 128, nullable: false),
                    TimeCreated = table.Column<long>(nullable: false, defaultValue: 0L),
                    TimePickedUp = table.Column<long>(nullable: false, defaultValue: 0L),
                    TimeCompleted = table.Column<long>(nullable: false, defaultValue: 0L),
                    DeliveryCode = table.Column<string>(nullable:true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_processes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "contributions",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 100, nullable: false),
                    ProcessId = table.Column<string>(maxLength: 100, nullable: false),
                    DeliveryDemandId = table.Column<string>(maxLength: 100, nullable: true),
                    DeliveryCode = table.Column<string>(nullable: true),
                    Username = table.Column<string>(maxLength: 36, nullable: false),
                    Type = table.Column<string>(maxLength: 100, nullable: false),
                    Status = table.Column<string>(maxLength: 128, nullable: false),
                    OtherInfo = table.Column<string>(nullable: true),
                    TimeWindowStart = table.Column<long>(nullable: false),
                    TimeWindowEnd = table.Column<long>(nullable: false),
                    TimeCreated = table.Column<long>(nullable: false, defaultValue: 0L),
                    TimeCompleted = table.Column<long>(nullable: false, defaultValue: 0L),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_contributions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "delivery-demands",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 100, nullable: false),
                    ProcessId = table.Column<string>(maxLength: 100, nullable: false),
                    CampaignName = table.Column<string>(maxLength: 100, nullable: false),
                    PickupUsername = table.Column<string>(maxLength: 36, nullable: false),
                    DestinationUsername = table.Column<string>(maxLength: 36, nullable: false),
                    Status = table.Column<string>(maxLength: 128, nullable: false),
                    OtherInfo = table.Column<string>(maxLength: 128, nullable: false),
                    TimeWindowStart = table.Column<long>(nullable: false),
                    TimeWindowEnd = table.Column<long>(nullable: false),
                    TimeCreated = table.Column<long>(nullable: false, defaultValue: 0L),
                    TimeCompleted = table.Column<long>(nullable: false, defaultValue: 0L)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_delivery-demands", x => x.Id);
                });


            migrationBuilder.CreateIndex(
                name: "IX_processes_CampaignId",
                table: "processes",
                column: "CampaignId");

            migrationBuilder.CreateIndex(
                name: "IX_contributions_ProcessId",
                table: "contributions",
                column: "ProcessId");

            migrationBuilder.CreateIndex(
                name: "IX_contributions_DeliveryDemandId",
                table: "contributions",
                column: "DeliveryDemandId");

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryDemands_ProcessId",
                table: "delivery-demands",
                column: "ProcessId");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "locations");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "ngos");

            migrationBuilder.DropTable(
                name: "campaigns");

            migrationBuilder.DropTable(
                name: "processes");

            migrationBuilder.DropTable(
                name: "contributions");

            migrationBuilder.DropTable(
                name: "delivery-demands");
        }
    }
}
