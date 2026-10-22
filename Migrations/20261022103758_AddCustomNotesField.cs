using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WatJobMatcher.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddCustomNotesField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CustomNotes",
                table: "CriteriaProfiles",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CustomNotes",
                table: "CriteriaProfiles");
        }
    }
}
