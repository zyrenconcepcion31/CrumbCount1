using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CrumbCount2.Migrations
{
    /// <inheritdoc />
    public partial class AddUnitCostAtWasteToWasteRecordings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "UnitCostAtWaste",
                table: "WasteRecordings",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UnitCostAtWaste",
                table: "WasteRecordings");
        }
    }
}
