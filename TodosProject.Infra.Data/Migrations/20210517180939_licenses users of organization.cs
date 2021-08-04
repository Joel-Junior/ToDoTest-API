using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TodosProject.Infra.Data.Migrations
{
    public partial class licensesusersoforganization : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Licenses",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Created_Time = table.Column<DateTime>(nullable: true),
                    Update_Time = table.Column<DateTime>(nullable: true),
                    Is_Active = table.Column<bool>(nullable: false, defaultValue: true),
                    Activation_Date = table.Column<DateTime>(nullable: false),
                    Expiration_Date = table.Column<DateTime>(nullable: true),
                    Code_ID = table.Column<Guid>(nullable: false),
                    Multiplayer_Minutes = table.Column<long>(nullable: false),
                    Max_File_Capacity = table.Column<int>(nullable: false),
                    IdOrganizaton = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Licenses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Licenses_Organizations_IdOrganizaton",
                        column: x => x.IdOrganizaton,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Licenses");

            migrationBuilder.UpdateData(
                table: "Profiles",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "Created_Time", "Is_Active" },
                values: new object[] { new DateTime(2021, 5, 15, 2, 0, 56, 585, DateTimeKind.Local).AddTicks(5259), true });

            migrationBuilder.UpdateData(
                table: "Profiles",
                keyColumn: "Id",
                keyValue: 2L,
                columns: new[] { "Created_Time", "Is_Active" },
                values: new object[] { new DateTime(2021, 5, 15, 2, 0, 56, 586, DateTimeKind.Local).AddTicks(5584), true });

            migrationBuilder.UpdateData(
                table: "Profiles",
                keyColumn: "Id",
                keyValue: 3L,
                columns: new[] { "Created_Time", "Is_Active" },
                values: new object[] { new DateTime(2021, 5, 15, 2, 0, 56, 586, DateTimeKind.Local).AddTicks(5628), true });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1L,
                column: "Created_Time",
                value: new DateTime(2021, 5, 15, 2, 0, 56, 588, DateTimeKind.Local).AddTicks(3346));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "Created_Time", "Is_Authenticated", "Password" },
                values: new object[] { new DateTime(2021, 5, 15, 2, 0, 56, 589, DateTimeKind.Local).AddTicks(8707), true, "AQAQJwAARiuZhep5mFeCmEb+IqyT3q58YliuWvSTJEZCtUYEk10=" });
        }
    }
}
