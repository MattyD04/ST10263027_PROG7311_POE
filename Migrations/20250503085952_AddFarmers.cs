using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ST10263027_PROG7311_POE.Migrations
{
    /// <inheritdoc />
    public partial class AddFarmers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Farmers",
                columns: table => new
                {
                    FarmerId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FarmerUserName = table.Column<string>(type: "nvarchar(MAX)", nullable: true),
                    FarmerPassword = table.Column<string>(type: "nvarchar(MAX)", nullable: true),
                    FarmerContactNum = table.Column<string>(type: "nvarchar(MAX)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Farmers", x => x.FarmerId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Farmers");
        }
    }
}
