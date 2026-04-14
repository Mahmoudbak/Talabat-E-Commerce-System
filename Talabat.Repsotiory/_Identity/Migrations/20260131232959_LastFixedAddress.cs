using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Talabat.Repsotiory._Identity.Migrations
{
    /// <inheritdoc />
    public partial class LastFixedAddress : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FristName",
                table: "Addresses",
                newName: "FirstName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FirstName",
                table: "Addresses",
                newName: "FristName");
        }
    }
}
