using Microsoft.EntityFrameworkCore.Migrations;

namespace HTMLPreviewerApp.Data.Migrations
{
    public partial class ApplicationRoleModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HtmlSamples_AspNetUsers_UserId1",
                table: "HtmlSamples");

            migrationBuilder.DropIndex(
                name: "IX_HtmlSamples_UserId1",
                table: "HtmlSamples");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "HtmlSamples");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "HtmlSamples",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_HtmlSamples_UserId",
                table: "HtmlSamples",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_HtmlSamples_AspNetUsers_UserId",
                table: "HtmlSamples",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HtmlSamples_AspNetUsers_UserId",
                table: "HtmlSamples");

            migrationBuilder.DropIndex(
                name: "IX_HtmlSamples_UserId",
                table: "HtmlSamples");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "HtmlSamples",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId1",
                table: "HtmlSamples",
                type: "nvarchar(450)",
                nullable: true);

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
    }
}
