using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TodosProject.Infra.Data.Migrations
{
    public partial class remove_field_active_table_role : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Active",
                table: "Roles");

            migrationBuilder.InsertData(
                table: "Profiles",
                columns: new[] { "Id", "Access_Group", "Created_Time", "Description", "Is_Active", "Login_Type", "Profile_Type", "Update_Time" },
                values: new object[,]
                {
                    { 1L, (byte)0, new DateTime(2021, 4, 27, 22, 13, 1, 14, DateTimeKind.Local).AddTicks(268), "Perfil Usuário", true, (byte)1, (byte)0, null },
                    { 2L, (byte)0, new DateTime(2021, 4, 27, 22, 13, 1, 14, DateTimeKind.Local).AddTicks(8859), "Perfil Administrador", true, (byte)1, (byte)1, null },
                    { 3L, (byte)0, new DateTime(2021, 4, 27, 22, 13, 1, 14, DateTimeKind.Local).AddTicks(8897), "Perfil Manager", true, (byte)1, (byte)2, null }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "Created_Time", "Description", "Role", "Update_Time" },
                values: new object[] { 1L, new DateTime(2021, 4, 27, 22, 13, 1, 15, DateTimeKind.Local).AddTicks(8979), "Regra de acesso a tela de Auditoria", "ROLE_AUDIT", null });

            migrationBuilder.InsertData(
                table: "ProfileRoles",
                columns: new[] { "Id_Profile", "Id_Role" },
                values: new object[] { 1L, 1L });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ProfileRoles",
                keyColumns: new[] { "Id_Profile", "Id_Role" },
                keyValues: new object[] { 1L, 1L });

            migrationBuilder.DeleteData(
                table: "Profiles",
                keyColumn: "Id",
                keyValue: 2L);

            migrationBuilder.DeleteData(
                table: "Profiles",
                keyColumn: "Id",
                keyValue: 3L);

            migrationBuilder.DeleteData(
                table: "Profiles",
                keyColumn: "Id",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1L);

            migrationBuilder.AddColumn<bool>(
                name: "Active",
                table: "Roles",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
