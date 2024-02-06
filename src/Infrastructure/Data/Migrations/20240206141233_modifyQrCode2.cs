using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class modifyQrCode2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AuthorizedUserFirstName",
                table: "QrCodes");

            migrationBuilder.RenameColumn(
                name: "AuthorizedUserLastName",
                table: "QrCodes",
                newName: "AuthorizedUserFullName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AuthorizedUserFullName",
                table: "QrCodes",
                newName: "AuthorizedUserLastName");

            migrationBuilder.AddColumn<string>(
                name: "AuthorizedUserFirstName",
                table: "QrCodes",
                type: "text",
                nullable: true);
        }
    }
}
