using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Talabat.Repsotiory.Data.migration
{
    /// <inheritdoc />
    public partial class lastfixed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ShippingAddress_FristName",
                table: "Orders",
                newName: "ShippingAddress_FirstName");


        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ShippingAddress_FirstName",
                table: "Orders",
                newName: "ShippingAddress_FristName");

            migrationBuilder.RenameColumn(
                name: "PaymantIntentId",
                table: "Orders",
                newName: "PaymantInentId");
        }
    }
}
