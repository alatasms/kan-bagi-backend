using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PostService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class one_active_post_per_owner : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Keep only each owner's newest active post so the unique index can be created.
            migrationBuilder.Sql("""
                UPDATE "Posts" p SET "IsActive" = false
                WHERE p."IsActive" AND NOT p."IsDeleted"
                  AND EXISTS (
                    SELECT 1 FROM "Posts" newer
                    WHERE newer."OwnerId" = p."OwnerId" AND newer."IsActive" AND NOT newer."IsDeleted"
                      AND (newer."CreationTime", newer."Id") > (p."CreationTime", p."Id"))
                """);

            migrationBuilder.CreateIndex(
                name: "IX_Posts_OneActivePerOwner",
                table: "Posts",
                column: "OwnerId",
                unique: true,
                filter: "\"IsActive\" AND NOT \"IsDeleted\"");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Posts_OneActivePerOwner",
                table: "Posts");
        }
    }
}
