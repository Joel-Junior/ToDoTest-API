using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TodosProject.Infra.Data.Migrations
{
    public partial class new_tables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UpdateTime",
                table: "Users",
                newName: "Update_Time");

            migrationBuilder.RenameColumn(
                name: "IsAuthenticated",
                table: "Users",
                newName: "Is_Authenticated");

            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "Users",
                newName: "Is_Active");

            migrationBuilder.RenameColumn(
                name: "CreatedTime",
                table: "Users",
                newName: "Created_Time");

            migrationBuilder.AddColumn<long>(
                name: "IdProfile",
                table: "Users",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateTable(
                name: "Profiles",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Created_Time = table.Column<DateTime>(nullable: true),
                    Update_Time = table.Column<DateTime>(nullable: true),
                    Is_Active = table.Column<bool>(nullable: false, defaultValue: true),
                    Description = table.Column<string>(maxLength: 255, nullable: false),
                    Profile_Type = table.Column<byte>(nullable: false),
                    Access_Group = table.Column<byte>(nullable: false),
                    Login_Type = table.Column<byte>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Profiles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Created_Time = table.Column<DateTime>(nullable: true),
                    Update_Time = table.Column<DateTime>(nullable: true),
                    Is_Active = table.Column<bool>(nullable: false, defaultValue: true),
                    Description = table.Column<string>(maxLength: 255, nullable: false),
                    Role = table.Column<string>(maxLength: 80, nullable: false),
                    Active = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProfileRoles",
                columns: table => new
                {
                    Id_Profile = table.Column<long>(nullable: false),
                    Id_Role = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProfileRoles", x => new { x.Id_Profile, x.Id_Role });
                    table.ForeignKey(
                        name: "FK_ProfileRoles_Profiles_Id_Profile",
                        column: x => x.Id_Profile,
                        principalTable: "Profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProfileRoles_Roles_Id_Role",
                        column: x => x.Id_Role,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_IdProfile",
                table: "Users",
                column: "IdProfile");

            migrationBuilder.CreateIndex(
                name: "IX_ProfileRoles_Id_Role",
                table: "ProfileRoles",
                column: "Id_Role");

            migrationBuilder.CreateIndex(
                name: "IX_ProfileRoles_Id_Profile_Id_Role",
                table: "ProfileRoles",
                columns: new[] { "Id_Profile", "Id_Role" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Profiles_IdProfile",
                table: "Users",
                column: "IdProfile",
                principalTable: "Profiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Profiles_IdProfile",
                table: "Users");

            migrationBuilder.DropTable(
                name: "ProfileRoles");

            migrationBuilder.DropTable(
                name: "Profiles");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropIndex(
                name: "IX_Users_IdProfile",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "IdProfile",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "Update_Time",
                table: "Users",
                newName: "UpdateTime");

            migrationBuilder.RenameColumn(
                name: "Is_Authenticated",
                table: "Users",
                newName: "IsAuthenticated");

            migrationBuilder.RenameColumn(
                name: "Is_Active",
                table: "Users",
                newName: "IsActive");

            migrationBuilder.RenameColumn(
                name: "Created_Time",
                table: "Users",
                newName: "CreatedTime");
        }
    }
}
