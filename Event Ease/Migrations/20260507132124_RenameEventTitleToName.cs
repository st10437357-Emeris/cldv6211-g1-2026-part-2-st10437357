using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Event_Ease.Migrations
{
    /// <inheritdoc />
    public partial class RenameEventTitleToName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Title",
                table: "Events",
                newName: "Name");

            migrationBuilder.AddColumn<DateTime>(
                name: "EventDate",
                table: "Events",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EventDate",
                table: "Events");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Events",
                newName: "Title");
        }
    }
}
