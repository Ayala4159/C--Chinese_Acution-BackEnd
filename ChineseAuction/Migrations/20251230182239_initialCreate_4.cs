using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChineseAuction.Migrations
{
    /// <inheritdoc />
    public partial class initialCreate_4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Is_approved",
                table: "Gifts",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Is_approved",
                table: "Gifts");
        }
    }
}
