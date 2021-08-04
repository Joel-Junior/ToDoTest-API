using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TodosProject.Infra.Data.Migrations
{
    public partial class create_organizations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Address",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    City = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                    State = table.Column<string>(type: "CHAR(2)", nullable: true),
                    Cep = table.Column<string>(type: "VARCHAR(10)", nullable: true),
                    PublicPlace = table.Column<string>(type: "NVARCHAR(MAX)", nullable: true),
                    Neighborhood = table.Column<string>(type: "VARCHAR(225)", nullable: true),
                    Number = table.Column<string>(type: "VARCHAR(10)", nullable: true),
                    Complement = table.Column<string>(type: "VARCHAR(255)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Address", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Organizations",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Created_Time = table.Column<DateTime>(nullable: true),
                    Update_Time = table.Column<DateTime>(nullable: true),
                    Is_Active = table.Column<bool>(nullable: false, defaultValue: true),
                    Name = table.Column<string>(type: "VARCHAR(300)", nullable: false),
                    Description = table.Column<string>(type: "NVARCHAR(MAX)", nullable: false),
                    Logo = table.Column<string>(type: "NVARCHAR(MAX)", nullable: true),
                    Phone = table.Column<string>(type: "VARCHAR(100)", nullable: true),
                    IdAddress = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Organizations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Organizations_Address_IdAddress",
                        column: x => x.IdAddress,
                        principalTable: "Address",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OrganizationUsers",
                columns: table => new
                {
                    Id_Organization = table.Column<long>(nullable: false),
                    Id_User = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrganizationUsers", x => new { x.Id_Organization, x.Id_User });
                    table.ForeignKey(
                        name: "FK_OrganizationUsers_Organizations_Id_Organization",
                        column: x => x.Id_Organization,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrganizationUsers_Users_Id_User",
                        column: x => x.Id_User,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            
            migrationBuilder.CreateIndex(
                name: "IX_Organizations_IdAddress",
                table: "Organizations",
                column: "IdAddress",
                unique: true,
                filter: "[IdAddress] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationUsers_Id_User",
                table: "OrganizationUsers",
                column: "Id_User");

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationUsers_Id_Organization_Id_User",
                table: "OrganizationUsers",
                columns: new[] { "Id_Organization", "Id_User" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrganizationUsers");

            migrationBuilder.DropTable(
                name: "Organizations");

            migrationBuilder.DropTable(
                name: "Address");           
        }
    }
}
