using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace MyCoreDAL.Migrations
{
    public partial class Model2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatePeople",
                table: "CustomerAddress",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDelete",
                table: "CustomerAddress",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatePeople",
                table: "Customer",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDelete",
                table: "Customer",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatePeople",
                table: "CustomerAddress");

            migrationBuilder.DropColumn(
                name: "IsDelete",
                table: "CustomerAddress");

            migrationBuilder.DropColumn(
                name: "CreatePeople",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "IsDelete",
                table: "Customer");
        }
    }
}
