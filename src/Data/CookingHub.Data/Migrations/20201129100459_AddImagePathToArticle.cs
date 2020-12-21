namespace CookingHub.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class AddImagePathToArticle : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImagePath",
                table: "Articles",
                maxLength: 1000,
                nullable: false,
                defaultValue: string.Empty);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImagePath",
                table: "Articles");
        }
    }
}
