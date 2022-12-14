using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    public partial class CreatedRewardSettingEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RewardSettings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MinSpentAmount = table.Column<decimal>(type: "numeric", nullable: false),
                    MinSpentAmountPoints = table.Column<int>(type: "integer", nullable: false),
                    UpperRangeSpentAmount = table.Column<decimal>(type: "numeric", nullable: false),
                    UpperRangeSpentPoints = table.Column<int>(type: "integer", nullable: false),
                    TaxRate = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RewardSettings", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RewardSettings");
        }
    }
}
