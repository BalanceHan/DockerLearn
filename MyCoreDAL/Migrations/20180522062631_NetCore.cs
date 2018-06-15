using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace MyCoreDAL.Migrations
{
    public partial class NetCore : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Customer",
                columns: table => new
                {
                    CustomerGuid = table.Column<string>(maxLength: 50, nullable: false),
                    AdditionDate = table.Column<DateTime>(nullable: true),
                    AdditionUnix = table.Column<string>(maxLength: 50, nullable: true),
                    CustomerName = table.Column<string>(maxLength: 50, nullable: true),
                    CustomerPhone = table.Column<string>(maxLength: 50, nullable: true),
                    ID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customer", x => x.CustomerGuid);
                });

            migrationBuilder.CreateTable(
                name: "CustomerAddress",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("MySQL:AutoIncrement", true),
                    AdditionDate = table.Column<DateTime>(nullable: true),
                    AdditionUnix = table.Column<string>(maxLength: 50, nullable: true),
                    Address = table.Column<string>(maxLength: 200, nullable: true),
                    AddressGuid = table.Column<string>(maxLength: 50, nullable: true),
                    City = table.Column<string>(maxLength: 50, nullable: true),
                    CustomerGuid = table.Column<string>(maxLength: 50, nullable: true),
                    District = table.Column<string>(maxLength: 50, nullable: true),
                    Province = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerAddress", x => x.ID);
                    table.ForeignKey(
                        name: "FK_CustomerAddress_Customer_CustomerGuid",
                        column: x => x.CustomerGuid,
                        principalTable: "Customer",
                        principalColumn: "CustomerGuid",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CustomerAddress_CustomerGuid",
                table: "CustomerAddress",
                column: "CustomerGuid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomerAddress");

            migrationBuilder.DropTable(
                name: "Customer");
        }
    }
}
