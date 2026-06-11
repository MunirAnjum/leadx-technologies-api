using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LeadXTechnologiesApi.Migrations
{
    /// <inheritdoc />
    public partial class AddConsent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Consent",
                table: "ContactMessages",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Consent",
                table: "ContactMessages");
        }
    }
}
