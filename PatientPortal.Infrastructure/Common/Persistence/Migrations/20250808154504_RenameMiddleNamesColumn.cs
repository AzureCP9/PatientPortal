using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PatientPortal.Infrastructure.Common.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RenameMiddleNamesColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MiddleNames",
                table: "Patients",
                newName: "Name_MiddleNames");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name_MiddleNames",
                table: "Patients",
                newName: "MiddleNames");
        }
    }
}
