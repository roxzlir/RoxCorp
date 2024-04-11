using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RoxCorp.Data.Migrations
{
    /// <inheritdoc />
    public partial class _4TablesCreated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    EmployeeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeName = table.Column<string>(type: "nvarchar(65)", maxLength: 65, nullable: false),
                    EmployeeEmail = table.Column<string>(type: "nvarchar(175)", maxLength: 175, nullable: false),
                    EmployeePhone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.EmployeeId);
                });

            migrationBuilder.CreateTable(
                name: "Leaves",
                columns: table => new
                {
                    LeaveId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LeaveType = table.Column<string>(type: "nvarchar(35)", maxLength: 35, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Leaves", x => x.LeaveId);
                });

            migrationBuilder.CreateTable(
                name: "ApplyForLeaves",
                columns: table => new
                {
                    ApplyForLeaveId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FkLeaveId = table.Column<int>(type: "int", nullable: false),
                    FkEmployeeId = table.Column<int>(type: "int", nullable: false),
                    ApplyFromDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ApplyToDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ApplyNote = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ApplyRegisteredDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplyForLeaves", x => x.ApplyForLeaveId);
                    table.ForeignKey(
                        name: "FK_ApplyForLeaves_Employees_FkEmployeeId",
                        column: x => x.FkEmployeeId,
                        principalTable: "Employees",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApplyForLeaves_Leaves_FkLeaveId",
                        column: x => x.FkLeaveId,
                        principalTable: "Leaves",
                        principalColumn: "LeaveId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GrantLeaves",
                columns: table => new
                {
                    GrantLeaveId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FkApplyForLeaveId = table.Column<int>(type: "int", nullable: false),
                    Granted = table.Column<bool>(type: "bit", nullable: false),
                    DecisionDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GrantLeaves", x => x.GrantLeaveId);
                    table.ForeignKey(
                        name: "FK_GrantLeaves_ApplyForLeaves_FkApplyForLeaveId",
                        column: x => x.FkApplyForLeaveId,
                        principalTable: "ApplyForLeaves",
                        principalColumn: "ApplyForLeaveId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApplyForLeaves_FkEmployeeId",
                table: "ApplyForLeaves",
                column: "FkEmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplyForLeaves_FkLeaveId",
                table: "ApplyForLeaves",
                column: "FkLeaveId");

            migrationBuilder.CreateIndex(
                name: "IX_GrantLeaves_FkApplyForLeaveId",
                table: "GrantLeaves",
                column: "FkApplyForLeaveId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GrantLeaves");

            migrationBuilder.DropTable(
                name: "ApplyForLeaves");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "Leaves");
        }
    }
}
