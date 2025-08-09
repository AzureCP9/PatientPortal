using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PatientPortal.Infrastructure.Common.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class FlattenConsultationSchedule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Schedule_Discriminator",
                table: "Consultations");

            migrationBuilder.RenameColumn(
                name: "Schedule_Start",
                table: "Consultations",
                newName: "ScheduleStart");

            migrationBuilder.RenameColumn(
                name: "Schedule_Duration",
                table: "Consultations",
                newName: "ScheduleDuration");

            migrationBuilder.RenameColumn(
                name: "Schedule_CancelledAt",
                table: "Consultations",
                newName: "ScheduleCancelledAt");

            migrationBuilder.RenameColumn(
                name: "Schedule_CancellationReason",
                table: "Consultations",
                newName: "ScheduleCancellationReason");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ScheduleStart",
                table: "Consultations",
                newName: "Schedule_Start");

            migrationBuilder.RenameColumn(
                name: "ScheduleDuration",
                table: "Consultations",
                newName: "Schedule_Duration");

            migrationBuilder.RenameColumn(
                name: "ScheduleCancelledAt",
                table: "Consultations",
                newName: "Schedule_CancelledAt");

            migrationBuilder.RenameColumn(
                name: "ScheduleCancellationReason",
                table: "Consultations",
                newName: "Schedule_CancellationReason");

            migrationBuilder.AddColumn<string>(
                name: "Schedule_Discriminator",
                table: "Consultations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
