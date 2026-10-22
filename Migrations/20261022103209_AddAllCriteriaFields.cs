using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WatJobMatcher.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddAllCriteriaFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AcceptedPositions",
                table: "CriteriaProfiles",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "DoubleAccommodationRequired",
                table: "CriteriaProfiles",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LatestAcceptableStartDate",
                table: "CriteriaProfiles",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AcceptedPositions",
                table: "CriteriaProfiles");

            migrationBuilder.DropColumn(
                name: "DoubleAccommodationRequired",
                table: "CriteriaProfiles");

            migrationBuilder.DropColumn(
                name: "LatestAcceptableStartDate",
                table: "CriteriaProfiles");
        }
    }
}
