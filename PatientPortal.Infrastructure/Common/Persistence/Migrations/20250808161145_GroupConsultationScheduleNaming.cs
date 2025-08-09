using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PatientPortal.Infrastructure.Common.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class GroupConsultationScheduleNaming : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
    }
}
