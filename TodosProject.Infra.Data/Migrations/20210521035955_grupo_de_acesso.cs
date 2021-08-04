using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TodosProject.Infra.Data.Migrations
{
    public partial class grupo_de_acesso : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AccessGroups",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Created_Time = table.Column<DateTime>(nullable: true),
                    Update_Time = table.Column<DateTime>(nullable: true),
                    Is_Active = table.Column<bool>(nullable: false, defaultValue: true),
                    Name = table.Column<string>(type: "VARCHAR(300)", nullable: false),
                    Description = table.Column<string>(type: "NVARCHAR(MAX)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccessGroups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AccessGroupUsers",
                columns: table => new
                {
                    Id_AccessGroup = table.Column<long>(nullable: false),
                    Id_User = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccessGroupUsers", x => new { x.Id_AccessGroup, x.Id_User });
                    table.ForeignKey(
                        name: "FK_AccessGroupUsers_AccessGroups_Id_AccessGroup",
                        column: x => x.Id_AccessGroup,
                        principalTable: "AccessGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AccessGroupUsers_Users_Id_User",
                        column: x => x.Id_User,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_AccessGroupUsers_Id_User",
                table: "AccessGroupUsers",
                column: "Id_User");

            migrationBuilder.CreateIndex(
                name: "IX_AccessGroupUsers_Id_AccessGroup_Id_User",
                table: "AccessGroupUsers",
                columns: new[] { "Id_AccessGroup", "Id_User" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccessGroupUsers");

            migrationBuilder.DropTable(
                name: "AccessGroups");

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
        }
    }
}
