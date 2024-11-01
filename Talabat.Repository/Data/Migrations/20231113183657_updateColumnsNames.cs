using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Talabat.Repository.Data.Migrations
{
    public partial class updateColumnsNames : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ShippingAddress_Street",
                table: "Orders",
                newName: "ShipToAddress_Street");

            migrationBuilder.RenameColumn(
                name: "ShippingAddress_LastName",
                table: "Orders",
                newName: "ShipToAddress_LastName");

            migrationBuilder.RenameColumn(
                name: "ShippingAddress_FirstName",
                table: "Orders",
                newName: "ShipToAddress_FirstName");

            migrationBuilder.RenameColumn(
                name: "ShippingAddress_Country",
                table: "Orders",
                newName: "ShipToAddress_Country");

            migrationBuilder.RenameColumn(
                name: "ShippingAddress_City",
                table: "Orders",
                newName: "ShipToAddress_City");

            migrationBuilder.RenameColumn(
                name: "State",
                table: "Orders",
                newName: "Status");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ShipToAddress_Street",
                table: "Orders",
                newName: "ShippingAddress_Street");

            migrationBuilder.RenameColumn(
                name: "ShipToAddress_LastName",
                table: "Orders",
                newName: "ShippingAddress_LastName");

            migrationBuilder.RenameColumn(
                name: "ShipToAddress_FirstName",
                table: "Orders",
                newName: "ShippingAddress_FirstName");

            migrationBuilder.RenameColumn(
                name: "ShipToAddress_Country",
                table: "Orders",
                newName: "ShippingAddress_Country");

            migrationBuilder.RenameColumn(
                name: "ShipToAddress_City",
                table: "Orders",
                newName: "ShippingAddress_City");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "Orders",
                newName: "State");
        }
    }
}
