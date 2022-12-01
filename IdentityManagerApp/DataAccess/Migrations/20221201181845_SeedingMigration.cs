using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    public partial class SeedingMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql($"INSERT INTO Users (Id, LastName, FirstName, Email, PhoneNumber, Password, Address) " +
                $"VALUES('1', 'Doe', 'John', 'john@doe.com', '555-5555-555', 'P@ssw0rd1', 'North-East London')");

            migrationBuilder.Sql($"INSERT INTO Users (Id, LastName, FirstName, Email, PhoneNumber, Password, Address) " +
                $"VALUES('2', 'Donny', 'Yen', 'yen@donny.com', '555-5555-555', 'P@ssw0rd1', 'North-East China')");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
