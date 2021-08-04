using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TodosProject.Infra.Data.Migrations
{
    public partial class grow_field_Lastpassword_tb_user : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Last_Password",
                table: "Users",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Profiles",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "Created_Time", "Is_Active" },
                values: new object[] { new DateTime(2021, 4, 27, 23, 23, 51, 287, DateTimeKind.Local).AddTicks(9776), true });

            migrationBuilder.UpdateData(
                table: "Profiles",
                keyColumn: "Id",
                keyValue: 2L,
                columns: new[] { "Created_Time", "Is_Active" },
                values: new object[] { new DateTime(2021, 4, 27, 23, 23, 51, 288, DateTimeKind.Local).AddTicks(7826), true });

            migrationBuilder.UpdateData(
                table: "Profiles",
                keyColumn: "Id",
                keyValue: 3L,
                columns: new[] { "Created_Time", "Is_Active" },
                values: new object[] { new DateTime(2021, 4, 27, 23, 23, 51, 288, DateTimeKind.Local).AddTicks(7868), true });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1L,
                column: "Created_Time",
                value: new DateTime(2021, 4, 27, 23, 23, 51, 289, DateTimeKind.Local).AddTicks(7814));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Last_Password",
                table: "Users",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Profiles",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "Created_Time", "Is_Active" },
                values: new object[] { new DateTime(2021, 4, 27, 23, 23, 4, 879, DateTimeKind.Local).AddTicks(2525), true });

            migrationBuilder.UpdateData(
                table: "Profiles",
                keyColumn: "Id",
                keyValue: 2L,
                columns: new[] { "Created_Time", "Is_Active" },
                values: new object[] { new DateTime(2021, 4, 27, 23, 23, 4, 880, DateTimeKind.Local).AddTicks(1320), true });

            migrationBuilder.UpdateData(
                table: "Profiles",
                keyColumn: "Id",
                keyValue: 3L,
                columns: new[] { "Created_Time", "Is_Active" },
                values: new object[] { new DateTime(2021, 4, 27, 23, 23, 4, 880, DateTimeKind.Local).AddTicks(1360), true });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1L,
                column: "Created_Time",
                value: new DateTime(2021, 4, 27, 23, 23, 4, 881, DateTimeKind.Local).AddTicks(1132));
        }
    }
}
