using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Test_Assigment_for_Codebridge.Migrations
{
    /// <inheritdoc />
    public partial class Initialize : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "dogs",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    name = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    color = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    tail_length = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    weight = table.Column<int>(type: "int", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dogs", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_dogs_name",
                table: "dogs",
                column: "name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "dogs");
        }
    }
}
