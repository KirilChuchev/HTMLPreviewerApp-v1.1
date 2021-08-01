using Microsoft.EntityFrameworkCore.Migrations;

namespace HTMLPreviewerApp.Data.Migrations
{
    public partial class ApplicationUserModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "HtmlSamples",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "UserId1",
                table: "HtmlSamples",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_HtmlSamples_UserId1",
                table: "HtmlSamples",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_HtmlSamples_AspNetUsers_UserId1",
                table: "HtmlSamples",
                column: "UserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HtmlSamples_AspNetUsers_UserId1",
                table: "HtmlSamples");

            migrationBuilder.DropIndex(
                name: "IX_HtmlSamples_UserId1",
                table: "HtmlSamples");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "HtmlSamples");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "HtmlSamples");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "AspNetUsers");
        }
    }
}
