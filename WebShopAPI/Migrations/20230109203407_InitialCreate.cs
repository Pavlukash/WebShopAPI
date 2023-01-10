using Microsoft.EntityFrameworkCore.Migrations;

namespace WebShopAPI.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Orders_OrderEntityId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_OrderEntityId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "OrderEntityId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Password",
                table: "Clients");

            migrationBuilder.AddColumn<string>(
                name: "PasswordHash",
                table: "Clients",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PasswordSalt",
                table: "Clients",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PasswordHash",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "PasswordSalt",
                table: "Clients");

            migrationBuilder.AddColumn<int>(
                name: "OrderEntityId",
                table: "Products",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "Clients",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Products_OrderEntityId",
                table: "Products",
                column: "OrderEntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Orders_OrderEntityId",
                table: "Products",
                column: "OrderEntityId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
