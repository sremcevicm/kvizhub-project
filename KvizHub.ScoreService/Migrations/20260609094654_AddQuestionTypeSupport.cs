using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KvizHub.ScoreService.Migrations
{
    /// <inheritdoc />
    public partial class AddQuestionTypeSupport : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SelectedAnswerIdsCsv",
                table: "AttemptAnswers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TextAnswer",
                table: "AttemptAnswers",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SelectedAnswerIdsCsv",
                table: "AttemptAnswers");

            migrationBuilder.DropColumn(
                name: "TextAnswer",
                table: "AttemptAnswers");
        }
    }
}
