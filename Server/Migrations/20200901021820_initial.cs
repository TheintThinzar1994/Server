using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Server.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Departments",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(nullable: true),
                    Is_Active = table.Column<bool>(nullable: false),
                    ts = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Menu",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Menu_Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Parent_Id = table.Column<long>(nullable: false),
                    Created_Date = table.Column<DateTime>(nullable: false),
                    Updated_Date = table.Column<DateTime>(nullable: false),
                    Action = table.Column<string>(nullable: true),
                    RoutePath = table.Column<string>(nullable: true),
                    Icon = table.Column<string>(nullable: true),
                    Is_Active = table.Column<bool>(nullable: false),
                    ts = table.Column<DateTime>(nullable: false),
                    order = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Menu", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(nullable: true),
                    isActive = table.Column<bool>(nullable: false),
                    ts = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ThankCards",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    From_Employee_Id = table.Column<long>(nullable: false),
                    To_Employee_Id = table.Column<long>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    SendText = table.Column<string>(nullable: true),
                    SendDate = table.Column<DateTime>(nullable: false),
                    ReplyText = table.Column<string>(nullable: true),
                    ReplyDate = table.Column<DateTime>(nullable: false),
                    Status = table.Column<string>(nullable: true),
                    isActive = table.Column<bool>(nullable: false),
                    ts = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThankCards", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SubDepartments",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(nullable: true),
                    Dept_Id = table.Column<long>(nullable: false),
                    Is_Active = table.Column<long>(nullable: false),
                    ts = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubDepartments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubDepartments_Departments_Dept_Id",
                        column: x => x.Dept_Id,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MenuRole",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Created_Date = table.Column<DateTime>(nullable: false),
                    Updated_Date = table.Column<DateTime>(nullable: false),
                    isActive = table.Column<bool>(nullable: false),
                    ts = table.Column<DateTime>(nullable: false),
                    Menu_Id = table.Column<long>(nullable: false),
                    Role_Id = table.Column<long>(nullable: false),
                    Action = table.Column<string>(nullable: false)

                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenuRole", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MenuRole_Menu_Menu_Id",
                        column: x => x.Menu_Id,
                        principalTable: "Menu",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MenuRole_Roles_Role_Id",
                        column: x => x.Role_Id,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    User_Name = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true),
                    isActive = table.Column<bool>(nullable: false),
                    Created_Date = table.Column<DateTime>(nullable: false),
                    Updated_Date = table.Column<DateTime>(nullable: false),
                    ts = table.Column<DateTime>(nullable: false),
                    Role_ID = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Roles_Role_ID",
                        column: x => x.Role_ID,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    User_Name = table.Column<string>(nullable: true),
                    Sub_Dept_Id = table.Column<long>(nullable: true),
                    Dept_Id = table.Column<long>(nullable: false),
                    User_Id = table.Column<long>(nullable: true),
                    Address = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    Phone = table.Column<string>(nullable: false),
                    PhotoName = table.Column<string>(nullable: true),
                    Created_Date = table.Column<DateTime>(nullable: false),
                    End_Date = table.Column<DateTime>(nullable: false),
                    isActive = table.Column<bool>(nullable: false),
                    ts = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Employees_Departments_Dept_Id",
                        column: x => x.Dept_Id,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Employees_SubDepartments_Sub_Dept_Id",
                        column: x => x.Sub_Dept_Id,
                        principalTable: "SubDepartments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    //table.ForeignKey(
                    //    name: "FK_Employees_Users_User_Id",
                    //    column: x => x.User_Id,
                    //    principalTable: "Users",
                    //    principalColumn: "Id",
                    //    onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Employees_Dept_Id",
                table: "Employees",
                column: "Dept_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_Sub_Dept_Id",
                table: "Employees",
                column: "Sub_Dept_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_User_Id",
                table: "Employees",
                column: "User_Id");

            migrationBuilder.CreateIndex(
                name: "IX_MenuRole_Menu_Id",
                table: "MenuRole",
                column: "Menu_Id");

            migrationBuilder.CreateIndex(
                name: "IX_MenuRole_Role_Id",
                table: "MenuRole",
                column: "Role_Id");

            migrationBuilder.CreateIndex(
                name: "IX_SubDepartments_Dept_Id",
                table: "SubDepartments",
                column: "Dept_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Role_ID",
                table: "Users",
                column: "Role_ID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "MenuRole");

            migrationBuilder.DropTable(
                name: "ThankCards");

            migrationBuilder.DropTable(
                name: "SubDepartments");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Menu");

            migrationBuilder.DropTable(
                name: "Departments");

            migrationBuilder.DropTable(
                name: "Roles");
        }
    }
}
