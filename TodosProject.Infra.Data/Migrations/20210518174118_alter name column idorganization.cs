using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TodosProject.Infra.Data.Migrations
{
    public partial class alternamecolumnidorganization : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Licenses_Organizations_IdOrganizaton",
                table: "Licenses");

            migrationBuilder.DropIndex(
                name: "IX_Licenses_IdOrganizaton",
                table: "Licenses");

            migrationBuilder.DropColumn(
                name: "IdOrganizaton",
                table: "Licenses");

            migrationBuilder.AddColumn<long>(
                name: "IdOrganization",
                table: "Licenses",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.UpdateData(
                table: "Profiles",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "Created_Time", "Is_Active" },
                values: new object[] { new DateTime(2021, 5, 18, 14, 41, 18, 144, DateTimeKind.Local).AddTicks(2193), true });

            migrationBuilder.UpdateData(
                table: "Profiles",
                keyColumn: "Id",
                keyValue: 2L,
                columns: new[] { "Created_Time", "Is_Active" },
                values: new object[] { new DateTime(2021, 5, 18, 14, 41, 18, 145, DateTimeKind.Local).AddTicks(9548), true });

            migrationBuilder.UpdateData(
                table: "Profiles",
                keyColumn: "Id",
                keyValue: 3L,
                columns: new[] { "Created_Time", "Is_Active" },
                values: new object[] { new DateTime(2021, 5, 18, 14, 41, 18, 145, DateTimeKind.Local).AddTicks(9609), true });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1L,
                column: "Created_Time",
                value: new DateTime(2021, 5, 18, 14, 41, 18, 147, DateTimeKind.Local).AddTicks(5073));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "Created_Time", "Is_Authenticated", "Password" },
                values: new object[] { new DateTime(2021, 5, 18, 14, 41, 18, 148, DateTimeKind.Local).AddTicks(8914), true, "AQAQJwAABYn1+BAU4e9dDlg5ENnJFlcDvM9Kih/TJ3wNde6+QYc=" });

            migrationBuilder.CreateIndex(
                name: "IX_Licenses_IdOrganization",
                table: "Licenses",
                column: "IdOrganization");

            migrationBuilder.AddForeignKey(
                name: "FK_Licenses_Organizations_IdOrganization",
                table: "Licenses",
                column: "IdOrganization",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Licenses_Organizations_IdOrganization",
                table: "Licenses");

            migrationBuilder.DropIndex(
                name: "IX_Licenses_IdOrganization",
                table: "Licenses");

            migrationBuilder.DropColumn(
                name: "IdOrganization",
                table: "Licenses");

            migrationBuilder.AddColumn<long>(
                name: "IdOrganizaton",
                table: "Licenses",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.UpdateData(
                table: "Profiles",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "Created_Time", "Is_Active" },
                values: new object[] { new DateTime(2021, 5, 17, 15, 9, 38, 601, DateTimeKind.Local).AddTicks(3397), true });

            migrationBuilder.UpdateData(
                table: "Profiles",
                keyColumn: "Id",
                keyValue: 2L,
                columns: new[] { "Created_Time", "Is_Active" },
                values: new object[] { new DateTime(2021, 5, 17, 15, 9, 38, 602, DateTimeKind.Local).AddTicks(4035), true });

            migrationBuilder.UpdateData(
                table: "Profiles",
                keyColumn: "Id",
                keyValue: 3L,
                columns: new[] { "Created_Time", "Is_Active" },
                values: new object[] { new DateTime(2021, 5, 17, 15, 9, 38, 602, DateTimeKind.Local).AddTicks(4088), true });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1L,
                column: "Created_Time",
                value: new DateTime(2021, 5, 17, 15, 9, 38, 603, DateTimeKind.Local).AddTicks(9498));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "Created_Time", "Is_Authenticated", "Password" },
                values: new object[] { new DateTime(2021, 5, 17, 15, 9, 38, 606, DateTimeKind.Local).AddTicks(473), true, "AQAQJwAAKjS/WOz7Y8bhuZ8Mup+HDgI8qDb14iJaIvOHH7BZY+Q=" });

            migrationBuilder.CreateIndex(
                name: "IX_Licenses_IdOrganizaton",
                table: "Licenses",
                column: "IdOrganizaton");

            migrationBuilder.AddForeignKey(
                name: "FK_Licenses_Organizations_IdOrganizaton",
                table: "Licenses",
                column: "IdOrganizaton",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
