using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MapProjectZiarat.Migrations
{
    /// <inheritdoc />
    public partial class mapnewregion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Region",
                table: "maps",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Region",
                table: "maps");
        }
    }
}
