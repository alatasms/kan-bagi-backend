using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class identity_verification : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Stored hashes are HMACs keyed with IdentityVerification:HashKey; existing values are not.
            migrationBuilder.DropColumn(
                name: "TCIdentityNumber",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "IsCitizenshipConfirmed",
                table: "Users",
                newName: "IsIdentityVerified");

            migrationBuilder.AddColumn<string>(
                name: "TCIdentityNumberHash",
                table: "Users",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_TCIdentityNumberHash",
                table: "Users",
                column: "TCIdentityNumberHash",
                unique: true,
                filter: "\"TCIdentityNumberHash\" IS NOT NULL AND \"IsDeleted\" = false");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_TCIdentityNumberHash",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "TCIdentityNumberHash",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "IsIdentityVerified",
                table: "Users",
                newName: "IsCitizenshipConfirmed");

            migrationBuilder.AddColumn<string>(
                name: "TCIdentityNumber",
                table: "Users",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
