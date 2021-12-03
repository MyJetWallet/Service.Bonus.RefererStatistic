using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Service.BonusRefererStatistic.Postgres.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "referrerstats");

            migrationBuilder.CreateTable(
                name: "profiles",
                schema: "referrerstats",
                columns: table => new
                {
                    ClientId = table.Column<string>(type: "text", nullable: false),
                    ReferralInvited = table.Column<int>(type: "integer", nullable: false),
                    ReferralActivated = table.Column<int>(type: "integer", nullable: false),
                    BonusEarned = table.Column<decimal>(type: "numeric", nullable: false),
                    CommissionEarned = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_profiles", x => x.ClientId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "profiles",
                schema: "referrerstats");
        }
    }
}
