using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mawid.Booking.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ServiceProviderManyToMany : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // ١. الجدول الجديد الأول
            migrationBuilder.CreateTable(
                name: "ProviderServices",
                columns: table => new
                {
                    ProviderId = table.Column<Guid>(type: "uuid", nullable: false),
                    ServiceId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProviderServices", x => new { x.ProviderId, x.ServiceId });
                    table.ForeignKey(
                        name: "FK_ProviderServices_Providers_ProviderId",
                        column: x => x.ProviderId,
                        principalTable: "Providers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProviderServices_Services_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Services",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProviderServices_ServiceId",
                table: "ProviderServices",
                column: "ServiceId");

            // ٢. ننقل الربط القديم
            migrationBuilder.Sql(
                "INSERT INTO \"ProviderServices\" (\"ProviderId\", \"ServiceId\") SELECT \"ProviderId\", \"Id\" FROM \"Services\";");

            // ٣. بعدها نمسح العمود القديم
            migrationBuilder.DropForeignKey(
                name: "FK_Services_Providers_ProviderId",
                table: "Services");

            migrationBuilder.DropIndex(
                name: "IX_Services_ProviderId",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "ProviderId",
                table: "Services");
        }
        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProviderServices");

            migrationBuilder.AddColumn<Guid>(
                name: "ProviderId",
                table: "Services",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Services_ProviderId",
                table: "Services",
                column: "ProviderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Services_Providers_ProviderId",
                table: "Services",
                column: "ProviderId",
                principalTable: "Providers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
