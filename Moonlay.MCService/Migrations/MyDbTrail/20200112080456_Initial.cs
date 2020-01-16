using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace Moonlay.MasterData.WebApi.Migrations.MyDbTrail
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CustomerTrails",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTimeOffset>(nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 64, nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 64, nullable: true),
                    UpdatedAt = table.Column<DateTimeOffset>(nullable: false),
                    Deleted = table.Column<bool>(nullable: false),
                    Tested = table.Column<bool>(nullable: false),
                    CustomerId = table.Column<Guid>(nullable: false),
                    FirstName = table.Column<string>(maxLength: 64, nullable: true),
                    LastName = table.Column<string>(maxLength: 64, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerTrails", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomerTrails");
        }
    }
}
