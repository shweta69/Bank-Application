using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bank_App_DB_Context_Repo.Migrations
{
    /// <inheritdoc />
    public partial class RemoveCustomerIdIdentity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // 1. Drop the existing CustomerId column
            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "Users");

            // 2. Recreate CustomerId column without IDENTITY
            migrationBuilder.AddColumn<int>(
                name: "CustomerId",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                column: "CustomerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "CustomerId",
                table: "Users",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1");
        }
    }
}
