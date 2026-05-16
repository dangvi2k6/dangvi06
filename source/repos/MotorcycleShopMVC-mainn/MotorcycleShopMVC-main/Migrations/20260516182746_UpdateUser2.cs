using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MotorcycleShopMVC.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUser2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "avatar_path",
                table: "users");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "avatar_path",
                table: "users",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);
        }
    }
}
