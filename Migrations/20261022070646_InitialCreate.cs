using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WatJobMatcher.Api.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CriteriaProfiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ProfileName = table.Column<string>(type: "TEXT", nullable: false),
                    MinimumOpenings = table.Column<int>(type: "INTEGER", nullable: false),
                    EarliestAcceptableStartDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    AllowedPositions = table.Column<string>(type: "TEXT", nullable: false),
                    RejectedPositions = table.Column<string>(type: "TEXT", nullable: false),
                    CoupleApplicationRequired = table.Column<bool>(type: "INTEGER", nullable: false),
                    CoupleHousingRequired = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CriteriaProfiles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "JobListings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    HostName = table.Column<string>(type: "TEXT", nullable: false),
                    State = table.Column<string>(type: "TEXT", nullable: false),
                    City = table.Column<string>(type: "TEXT", nullable: false),
                    AvailableJobs = table.Column<int>(type: "INTEGER", nullable: false),
                    Positions = table.Column<string>(type: "TEXT", nullable: false),
                    EarliestStartDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    LatestStartDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Wage = table.Column<decimal>(type: "TEXT", nullable: false),
                    HoursPerWeek = table.Column<int>(type: "INTEGER", nullable: false),
                    HousingAvailable = table.Column<bool>(type: "INTEGER", nullable: false),
                    CoupleHousingCompatibility = table.Column<string>(type: "TEXT", nullable: false),
                    MatchStatus = table.Column<string>(type: "TEXT", nullable: false),
                    MatchScore = table.Column<double>(type: "REAL", nullable: false),
                    MatchReason = table.Column<string>(type: "TEXT", nullable: false),
                    SourceDocument = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobListings", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CriteriaProfiles");

            migrationBuilder.DropTable(
                name: "JobListings");
        }
    }
}
