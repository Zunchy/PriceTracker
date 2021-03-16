using Microsoft.EntityFrameworkCore.Migrations;

namespace PriceTracker.Data.Migrations
{
    public partial class initTrackedItem2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TrackedItems_AspNetUsers_UserId1",
                table: "TrackedItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TrackedItems",
                table: "TrackedItems");

            migrationBuilder.DropIndex(
                name: "IX_TrackedItems_UserId1",
                table: "TrackedItems");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "TrackedItems");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "TrackedItems",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "TrackedItems",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TrackedItems",
                table: "TrackedItems",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_TrackedItems_UserId",
                table: "TrackedItems",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_TrackedItems_AspNetUsers_UserId",
                table: "TrackedItems",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TrackedItems_AspNetUsers_UserId",
                table: "TrackedItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TrackedItems",
                table: "TrackedItems");

            migrationBuilder.DropIndex(
                name: "IX_TrackedItems_UserId",
                table: "TrackedItems");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "TrackedItems");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "TrackedItems",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId1",
                table: "TrackedItems",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_TrackedItems",
                table: "TrackedItems",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TrackedItems_UserId1",
                table: "TrackedItems",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_TrackedItems_AspNetUsers_UserId1",
                table: "TrackedItems",
                column: "UserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
