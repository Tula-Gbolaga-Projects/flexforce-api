using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace agency_portal_api.Migrations
{
    /// <inheritdoc />
    public partial class JobInviteMigrations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "JobDetails",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Publicity",
                table: "JobDetails",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateApproved",
                table: "Agencies",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Agencies",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ConnectedAgencies",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    JobSeekerId = table.Column<string>(type: "text", nullable: false),
                    AgencyId = table.Column<string>(type: "text", nullable: false),
                    ConnectedStatus = table.Column<int>(type: "integer", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    DateModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConnectedAgencies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ConnectedAgencies_Agencies_AgencyId",
                        column: x => x.AgencyId,
                        principalTable: "Agencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ConnectedAgencies_JobSeekers_JobSeekerId",
                        column: x => x.JobSeekerId,
                        principalTable: "JobSeekers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ConnectedAgencies_AgencyId",
                table: "ConnectedAgencies",
                column: "AgencyId");

            migrationBuilder.CreateIndex(
                name: "IX_ConnectedAgencies_JobSeekerId",
                table: "ConnectedAgencies",
                column: "JobSeekerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConnectedAgencies");

            migrationBuilder.DropColumn(
                name: "Location",
                table: "JobDetails");

            migrationBuilder.DropColumn(
                name: "Publicity",
                table: "JobDetails");

            migrationBuilder.DropColumn(
                name: "DateApproved",
                table: "Agencies");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Agencies");
        }
    }
}
