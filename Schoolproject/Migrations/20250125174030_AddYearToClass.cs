using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Schoolproject.Migrations
{
    /// <inheritdoc />
    public partial class AddYearToClass : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SubjectSpecialization",
                table: "Teachers",
                newName: "Subject");

            migrationBuilder.AddColumn<int>(
                name: "Year",
                table: "Classes",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Year",
                table: "Classes");

            migrationBuilder.RenameColumn(
                name: "Subject",
                table: "Teachers",
                newName: "SubjectSpecialization");
        }
    }
}
