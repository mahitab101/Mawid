using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mawid.Booking.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddAppointmentUniqueSlotIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Appointments_ProviderId",
                table: "Appointments");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_ProviderId_StartTime",
                table: "Appointments",
                columns: new[] { "ProviderId", "StartTime" },
                unique: true,
                filter: "\"Status\" <> 2");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Appointments_ProviderId_StartTime",
                table: "Appointments");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_ProviderId",
                table: "Appointments",
                column: "ProviderId");
        }
    }
}
