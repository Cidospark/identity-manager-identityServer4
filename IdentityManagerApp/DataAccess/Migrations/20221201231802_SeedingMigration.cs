using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    public partial class SeedingMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql($"INSERT INTO Resources (Id, State, Format, Description) VALUES('1', 'Soft', 'pdf', 'C# documentation for version 9')");
            migrationBuilder.Sql($"INSERT INTO Resources (Id, State, Format, Description) VALUES('2', 'Hard', 'Novel', 'Chike and the river, a native Nigeria commic book')");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
