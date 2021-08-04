using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TodosProject.Infra.Data.Migrations
{
    public partial class assoc_access_group_in_organization : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "IdOrganization",
                table: "AccessGroups",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.UpdateData(
                table: "Profiles",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "Created_Time", "Is_Active" },
                values: new object[] { new DateTime(2021, 5, 26, 12, 9, 41, 32, DateTimeKind.Local).AddTicks(5231), true });

            migrationBuilder.UpdateData(
                table: "Profiles",
                keyColumn: "Id",
                keyValue: 2L,
                columns: new[] { "Created_Time", "Is_Active" },
                values: new object[] { new DateTime(2021, 5, 26, 12, 9, 41, 33, DateTimeKind.Local).AddTicks(9821), true });

            migrationBuilder.UpdateData(
                table: "Profiles",
                keyColumn: "Id",
                keyValue: 3L,
                columns: new[] { "Created_Time", "Is_Active" },
                values: new object[] { new DateTime(2021, 5, 26, 12, 9, 41, 33, DateTimeKind.Local).AddTicks(9927), true });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1L,
                column: "Created_Time",
                value: new DateTime(2021, 5, 26, 12, 9, 41, 36, DateTimeKind.Local).AddTicks(1038));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "Created_Time", "Is_Authenticated", "Password" },
                values: new object[] { new DateTime(2021, 5, 26, 12, 9, 41, 41, DateTimeKind.Local).AddTicks(875), true, "AQAQJwAA/4HXo7hq2sj1NrOs9lFpGG0Mff2Dio6dtpRbFnMnU+s=" });

            migrationBuilder.CreateIndex(
                name: "IX_AccessGroups_IdOrganization",
                table: "AccessGroups",
                column: "IdOrganization");

            migrationBuilder.AddForeignKey(
                name: "FK_AccessGroups_Organizations_IdOrganization",
                table: "AccessGroups",
                column: "IdOrganization",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccessGroups_Organizations_IdOrganization",
                table: "AccessGroups");

            migrationBuilder.DropIndex(
                name: "IX_AccessGroups_IdOrganization",
                table: "AccessGroups");

            migrationBuilder.DropColumn(
                name: "IdOrganization",
                table: "AccessGroups");

            migrationBuilder.UpdateData(
                table: "Profiles",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "Created_Time", "Is_Active" },
                values: new object[] { new DateTime(2021, 5, 21, 0, 59, 55, 123, DateTimeKind.Local).AddTicks(3784), true });

            migrationBuilder.UpdateData(
                table: "Profiles",
                keyColumn: "Id",
                keyValue: 2L,
                columns: new[] { "Created_Time", "Is_Active" },
                values: new object[] { new DateTime(2021, 5, 21, 0, 59, 55, 124, DateTimeKind.Local).AddTicks(5315), true });

            migrationBuilder.UpdateData(
                table: "Profiles",
                keyColumn: "Id",
                keyValue: 3L,
                columns: new[] { "Created_Time", "Is_Active" },
                values: new object[] { new DateTime(2021, 5, 21, 0, 59, 55, 124, DateTimeKind.Local).AddTicks(5363), true });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1L,
                column: "Created_Time",
                value: new DateTime(2021, 5, 21, 0, 59, 55, 126, DateTimeKind.Local).AddTicks(896));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "Created_Time", "Is_Authenticated", "Password" },
                values: new object[] { new DateTime(2021, 5, 21, 0, 59, 55, 128, DateTimeKind.Local).AddTicks(2078), true, "AQAQJwAAbumCSLlZhNxwgKDyP/ZrUSra3A80tzYi2yL316iZxac=" });
        }
    }
}
