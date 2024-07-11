using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EntityFrameworkExercise.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddUuid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "uuid",
                schema: "store",
                table: "seller",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid());

            migrationBuilder.AddColumn<Guid>(
                name: "uuid",
                schema: "store",
                table: "sale",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid());

            migrationBuilder.AddColumn<Guid>(
                name: "uuid",
                schema: "store",
                table: "product",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid());

            migrationBuilder.AddColumn<Guid>(
                name: "uuid",
                schema: "store",
                table: "customer",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid());
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "uuid",
                schema: "store",
                table: "seller");

            migrationBuilder.DropColumn(
                name: "uuid",
                schema: "store",
                table: "sale");

            migrationBuilder.DropColumn(
                name: "uuid",
                schema: "store",
                table: "product");

            migrationBuilder.DropColumn(
                name: "uuid",
                schema: "store",
                table: "customer");
        }
    }
}
