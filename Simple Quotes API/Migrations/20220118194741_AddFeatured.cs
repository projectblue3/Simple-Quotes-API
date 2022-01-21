using Microsoft.EntityFrameworkCore.Migrations;

namespace Simple_Quotes_API.Migrations
{
    public partial class AddFeatured : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsFeatured",
                table: "Quotes",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsFeatured",
                table: "Authors",
                type: "bit",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsFeatured",
                table: "Quotes");

            migrationBuilder.DropColumn(
                name: "IsFeatured",
                table: "Authors");
        }
    }
}
