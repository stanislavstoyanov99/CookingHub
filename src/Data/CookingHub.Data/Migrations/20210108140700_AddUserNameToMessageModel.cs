namespace CookingHub.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class AddUserNameToMessageModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "Messages",
                nullable: false,
                defaultValue: string.Empty);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserName",
                table: "Messages");
        }
    }
}
