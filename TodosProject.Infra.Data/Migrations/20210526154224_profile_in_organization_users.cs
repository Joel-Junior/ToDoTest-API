using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TodosProject.Infra.Data.Migrations
{
    public partial class profile_in_organization_users : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "Id_Profile",
                table: "OrganizationUsers",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.UpdateData(
                table: "Profiles",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "Created_Time", "Is_Active" },
                values: new object[] { new DateTime(2021, 5, 26, 12, 42, 23, 742, DateTimeKind.Local).AddTicks(5319), true });

            migrationBuilder.UpdateData(
                table: "Profiles",
                keyColumn: "Id",
                keyValue: 2L,
                columns: new[] { "Created_Time", "Is_Active" },
                values: new object[] { new DateTime(2021, 5, 26, 12, 42, 23, 743, DateTimeKind.Local).AddTicks(9581), true });

            migrationBuilder.UpdateData(
                table: "Profiles",
                keyColumn: "Id",
                keyValue: 3L,
                columns: new[] { "Created_Time", "Is_Active" },
                values: new object[] { new DateTime(2021, 5, 26, 12, 42, 23, 743, DateTimeKind.Local).AddTicks(9757), true });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1L,
                column: "Created_Time",
                value: new DateTime(2021, 5, 26, 12, 42, 23, 746, DateTimeKind.Local).AddTicks(1382));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "Created_Time", "Is_Authenticated", "Password" },
                values: new object[] { new DateTime(2021, 5, 26, 12, 42, 23, 748, DateTimeKind.Local).AddTicks(4894), true, "AQAQJwAAL2XcWxZTfBRE2jOfsy2ag7txMuzKp5ItntbcSQ8pr08=" });

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationUsers_Id_Profile",
                table: "OrganizationUsers",
                column: "Id_Profile");

            migrationBuilder.AddForeignKey(
                name: "FK_OrganizationUsers_Profiles_Id_Profile",
                table: "OrganizationUsers",
                column: "Id_Profile",
                principalTable: "Profiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrganizationUsers_Profiles_Id_Profile",
                table: "OrganizationUsers");

            migrationBuilder.DropIndex(
                name: "IX_OrganizationUsers_Id_Profile",
                table: "OrganizationUsers");

            migrationBuilder.DropColumn(
                name: "Id_Profile",
                table: "OrganizationUsers");

            migrationBuilder.UpdateData(
                table: "Profiles",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "Created_Time", "Is_Active" },
                values: new object[] { new DateTime(2021, 5, 26, 12, 40, 18, 691, DateTimeKind.Local).AddTicks(9957), true });

            migrationBuilder.UpdateData(
                table: "Profiles",
                keyColumn: "Id",
                keyValue: 2L,
                columns: new[] { "Created_Time", "Is_Active" },
                values: new object[] { new DateTime(2021, 5, 26, 12, 40, 18, 693, DateTimeKind.Local).AddTicks(1697), true });

            migrationBuilder.UpdateData(
                table: "Profiles",
                keyColumn: "Id",
                keyValue: 3L,
                columns: new[] { "Created_Time", "Is_Active" },
                values: new object[] { new DateTime(2021, 5, 26, 12, 40, 18, 693, DateTimeKind.Local).AddTicks(1772), true });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1L,
                column: "Created_Time",
                value: new DateTime(2021, 5, 26, 12, 40, 18, 694, DateTimeKind.Local).AddTicks(7893));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "Created_Time", "Is_Authenticated", "Password" },
                values: new object[] { new DateTime(2021, 5, 26, 12, 40, 18, 696, DateTimeKind.Local).AddTicks(7947), true, "AQAQJwAAa4dRd6CGaaMZCSNR2dlDEM+IqTJG8HDmjizrKYT57QE=" });
        }
    }
}
