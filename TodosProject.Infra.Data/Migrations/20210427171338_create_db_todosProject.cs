using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TodosProject.Infra.Data.Migrations
{
    public partial class create_db_todosProject : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedTime = table.Column<DateTime>(nullable: true),
                    UpdateTime = table.Column<DateTime>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    Login = table.Column<string>(maxLength: 120, nullable: false),
                    Password = table.Column<string>(maxLength: 20, nullable: false),
                    Last_Password = table.Column<string>(maxLength: 20, nullable: true),
                    IsAuthenticated = table.Column<bool>(nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
