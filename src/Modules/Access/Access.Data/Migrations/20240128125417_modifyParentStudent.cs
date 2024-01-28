using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Access.Data.Migrations
{
    /// <inheritdoc />
    public partial class modifyParentStudent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ParentStudent",
                table: "ParentStudent");

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "ParentStudent",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "ParentStudent",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "ParentStudent",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Deleted",
                table: "ParentStudent",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "ParentStudent",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "ParentStudent",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedBy",
                table: "ParentStudent",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Modified",
                table: "ParentStudent",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ParentStudent",
                table: "ParentStudent",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_ParentStudent_IsDeleted",
                table: "ParentStudent",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_ParentStudent_ParentsId",
                table: "ParentStudent",
                column: "ParentsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ParentStudent",
                table: "ParentStudent");

            migrationBuilder.DropIndex(
                name: "IX_ParentStudent_IsDeleted",
                table: "ParentStudent");

            migrationBuilder.DropIndex(
                name: "IX_ParentStudent_ParentsId",
                table: "ParentStudent");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "ParentStudent");

            migrationBuilder.DropColumn(
                name: "Created",
                table: "ParentStudent");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "ParentStudent");

            migrationBuilder.DropColumn(
                name: "Deleted",
                table: "ParentStudent");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "ParentStudent");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "ParentStudent");

            migrationBuilder.DropColumn(
                name: "LastModifiedBy",
                table: "ParentStudent");

            migrationBuilder.DropColumn(
                name: "Modified",
                table: "ParentStudent");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ParentStudent",
                table: "ParentStudent",
                columns: new[] { "ParentsId", "StudentsId" });
        }
    }
}
