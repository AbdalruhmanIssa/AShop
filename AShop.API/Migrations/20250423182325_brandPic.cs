using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AShop.API.Migrations
{
    /// <inheritdoc />
    public partial class brandPic : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "mainImg",
                table: "Brands",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "mainImg",
                table: "Brands");
        }
    }
}
