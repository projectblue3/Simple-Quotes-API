using Microsoft.EntityFrameworkCore.Migrations;

namespace Simple_Quotes_API.Migrations
{
    public partial class AuthorUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Bio",
                table: "Authors",
                type: "nvarchar(512)",
                maxLength: 512,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DateOfBirth",
                table: "Authors",
                type: "nvarchar(64)",
                maxLength: 64,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DateOfDeath",
                table: "Authors",
                type: "nvarchar(64)",
                maxLength: 64,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Occupation",
                table: "Authors",
                type: "nvarchar(64)",
                maxLength: 64,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Bio",
                table: "Authors");

            migrationBuilder.DropColumn(
                name: "DateOfBirth",
                table: "Authors");

            migrationBuilder.DropColumn(
                name: "DateOfDeath",
                table: "Authors");

            migrationBuilder.DropColumn(
                name: "Occupation",
                table: "Authors");
        }
    }
}
