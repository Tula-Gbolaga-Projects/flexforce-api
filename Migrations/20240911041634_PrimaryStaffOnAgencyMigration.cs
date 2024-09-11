using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace agency_portal_api.Migrations
{
    /// <inheritdoc />
    public partial class PrimaryStaffOnAgencyMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPrimary",
                table: "AgencyStaff",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPrimary",
                table: "AgencyStaff");
        }
    }
}
