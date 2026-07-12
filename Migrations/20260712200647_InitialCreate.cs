using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace API.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "sports",
                columns: table => new
                {
                    sport_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    sport_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sports", x => x.sport_id);
                });

            migrationBuilder.CreateTable(
                name: "events",
                columns: table => new
                {
                    event_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    sport_id = table.Column<int>(type: "integer", nullable: false),
                    team1 = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    team2 = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    score = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false, defaultValue: "0:0"),
                    status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false, defaultValue: "upcoming"),
                    start_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_events", x => x.event_id);
                    table.CheckConstraint("CK_Event_Status", "status IN ('upcoming', 'live', 'finished', 'cancelled')");
                    table.ForeignKey(
                        name: "FK_events_sports_sport_id",
                        column: x => x.sport_id,
                        principalTable: "sports",
                        principalColumn: "sport_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "markets",
                columns: table => new
                {
                    market_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    event_id = table.Column<int>(type: "integer", nullable: false),
                    market_type = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    market_value = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    is_available = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_markets", x => x.market_id);
                    table.ForeignKey(
                        name: "FK_markets_events_event_id",
                        column: x => x.event_id,
                        principalTable: "events",
                        principalColumn: "event_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "outcomes",
                columns: table => new
                {
                    outcome_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    event_id = table.Column<int>(type: "integer", nullable: false),
                    outcome_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    outcome_score = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_outcomes", x => x.outcome_id);
                    table.ForeignKey(
                        name: "FK_outcomes_events_event_id",
                        column: x => x.event_id,
                        principalTable: "events",
                        principalColumn: "event_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "selections",
                columns: table => new
                {
                    selection_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    market_id = table.Column<int>(type: "integer", nullable: false),
                    selection_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    coefficient = table.Column<decimal>(type: "numeric(10,6)", nullable: false),
                    is_won = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_selections", x => x.selection_id);
                    table.ForeignKey(
                        name: "FK_selections_markets_market_id",
                        column: x => x.market_id,
                        principalTable: "markets",
                        principalColumn: "market_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_events_sport_id",
                table: "events",
                column: "sport_id");

            migrationBuilder.CreateIndex(
                name: "IX_markets_event_id",
                table: "markets",
                column: "event_id");

            migrationBuilder.CreateIndex(
                name: "IX_outcomes_event_id",
                table: "outcomes",
                column: "event_id");

            migrationBuilder.CreateIndex(
                name: "IX_selections_market_id",
                table: "selections",
                column: "market_id");

            migrationBuilder.CreateIndex(
                name: "IX_sports_sport_name",
                table: "sports",
                column: "sport_name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "outcomes");

            migrationBuilder.DropTable(
                name: "selections");

            migrationBuilder.DropTable(
                name: "markets");

            migrationBuilder.DropTable(
                name: "events");

            migrationBuilder.DropTable(
                name: "sports");
        }
    }
}
