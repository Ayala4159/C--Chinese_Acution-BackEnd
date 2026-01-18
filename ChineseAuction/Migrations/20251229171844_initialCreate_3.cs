using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChineseAuction.Migrations
{
    /// <inheritdoc />
    public partial class initialCreate_3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Basket_Gifts_GiftId",
                table: "Basket");

            migrationBuilder.DropForeignKey(
                name: "FK_Basket_Package_PackageId",
                table: "Basket");

            migrationBuilder.DropForeignKey(
                name: "FK_Basket_Users_UserId",
                table: "Basket");

            migrationBuilder.DropForeignKey(
                name: "FK_Purchase_Gifts_GiftId",
                table: "Purchase");

            migrationBuilder.DropForeignKey(
                name: "FK_Purchase_Package_PackageId",
                table: "Purchase");

            migrationBuilder.DropForeignKey(
                name: "FK_Purchase_Users_UserId",
                table: "Purchase");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Purchase",
                table: "Purchase");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Package",
                table: "Package");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Basket",
                table: "Basket");

            migrationBuilder.DropColumn(
                name: "IsWon",
                table: "Package");

            migrationBuilder.RenameTable(
                name: "Purchase",
                newName: "Purchases");

            migrationBuilder.RenameTable(
                name: "Package",
                newName: "Packages");

            migrationBuilder.RenameTable(
                name: "Basket",
                newName: "Baskets");

            migrationBuilder.RenameIndex(
                name: "IX_Purchase_UserId",
                table: "Purchases",
                newName: "IX_Purchases_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Purchase_PackageId",
                table: "Purchases",
                newName: "IX_Purchases_PackageId");

            migrationBuilder.RenameIndex(
                name: "IX_Purchase_GiftId",
                table: "Purchases",
                newName: "IX_Purchases_GiftId");

            migrationBuilder.RenameIndex(
                name: "IX_Basket_UserId",
                table: "Baskets",
                newName: "IX_Baskets_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Basket_PackageId",
                table: "Baskets",
                newName: "IX_Baskets_PackageId");

            migrationBuilder.RenameIndex(
                name: "IX_Basket_GiftId",
                table: "Baskets",
                newName: "IX_Baskets_GiftId");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Categories",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<bool>(
                name: "IsWon",
                table: "Purchases",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Purchases",
                table: "Purchases",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Packages",
                table: "Packages",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Baskets",
                table: "Baskets",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_Name",
                table: "Categories",
                column: "Name",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Baskets_Gifts_GiftId",
                table: "Baskets",
                column: "GiftId",
                principalTable: "Gifts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Baskets_Packages_PackageId",
                table: "Baskets",
                column: "PackageId",
                principalTable: "Packages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Baskets_Users_UserId",
                table: "Baskets",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Purchases_Gifts_GiftId",
                table: "Purchases",
                column: "GiftId",
                principalTable: "Gifts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Purchases_Packages_PackageId",
                table: "Purchases",
                column: "PackageId",
                principalTable: "Packages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Purchases_Users_UserId",
                table: "Purchases",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Baskets_Gifts_GiftId",
                table: "Baskets");

            migrationBuilder.DropForeignKey(
                name: "FK_Baskets_Packages_PackageId",
                table: "Baskets");

            migrationBuilder.DropForeignKey(
                name: "FK_Baskets_Users_UserId",
                table: "Baskets");

            migrationBuilder.DropForeignKey(
                name: "FK_Purchases_Gifts_GiftId",
                table: "Purchases");

            migrationBuilder.DropForeignKey(
                name: "FK_Purchases_Packages_PackageId",
                table: "Purchases");

            migrationBuilder.DropForeignKey(
                name: "FK_Purchases_Users_UserId",
                table: "Purchases");

            migrationBuilder.DropIndex(
                name: "IX_Categories_Name",
                table: "Categories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Purchases",
                table: "Purchases");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Packages",
                table: "Packages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Baskets",
                table: "Baskets");

            migrationBuilder.DropColumn(
                name: "IsWon",
                table: "Purchases");

            migrationBuilder.RenameTable(
                name: "Purchases",
                newName: "Purchase");

            migrationBuilder.RenameTable(
                name: "Packages",
                newName: "Package");

            migrationBuilder.RenameTable(
                name: "Baskets",
                newName: "Basket");

            migrationBuilder.RenameIndex(
                name: "IX_Purchases_UserId",
                table: "Purchase",
                newName: "IX_Purchase_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Purchases_PackageId",
                table: "Purchase",
                newName: "IX_Purchase_PackageId");

            migrationBuilder.RenameIndex(
                name: "IX_Purchases_GiftId",
                table: "Purchase",
                newName: "IX_Purchase_GiftId");

            migrationBuilder.RenameIndex(
                name: "IX_Baskets_UserId",
                table: "Basket",
                newName: "IX_Basket_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Baskets_PackageId",
                table: "Basket",
                newName: "IX_Basket_PackageId");

            migrationBuilder.RenameIndex(
                name: "IX_Baskets_GiftId",
                table: "Basket",
                newName: "IX_Basket_GiftId");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Categories",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<bool>(
                name: "IsWon",
                table: "Package",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Purchase",
                table: "Purchase",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Package",
                table: "Package",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Basket",
                table: "Basket",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Basket_Gifts_GiftId",
                table: "Basket",
                column: "GiftId",
                principalTable: "Gifts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Basket_Package_PackageId",
                table: "Basket",
                column: "PackageId",
                principalTable: "Package",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Basket_Users_UserId",
                table: "Basket",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Purchase_Gifts_GiftId",
                table: "Purchase",
                column: "GiftId",
                principalTable: "Gifts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Purchase_Package_PackageId",
                table: "Purchase",
                column: "PackageId",
                principalTable: "Package",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Purchase_Users_UserId",
                table: "Purchase",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
