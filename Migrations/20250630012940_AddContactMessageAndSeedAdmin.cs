using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DigitalSolutionsWeb.Migrations
{
    /// <inheritdoc />
    public partial class AddContactMessageAndSeedAdmin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsProcessed",
                table: "ContactMessages",
                newName: "IsRead");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "ContactMessages",
                newName: "SubmittedAt");

            migrationBuilder.AlterColumn<string>(
                name: "Phone",
                table: "ContactMessages",
                type: "TEXT",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 20);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SubmittedAt",
                table: "ContactMessages",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "IsRead",
                table: "ContactMessages",
                newName: "IsProcessed");

            migrationBuilder.AlterColumn<string>(
                name: "Phone",
                table: "ContactMessages",
                type: "TEXT",
                maxLength: 20,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 20,
                oldNullable: true);
        }
    }
}
