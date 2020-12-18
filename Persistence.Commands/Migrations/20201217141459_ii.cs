using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Persistence.Commands.Migrations
{
    public partial class ii : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "active_listings",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    expiration_date = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    owner = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    title = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    material_type_id = table.Column<int>(type: "int", maxLength: 2, nullable: true),
                    weight_value = table.Column<float>(type: "real", nullable: true),
                    weight_unit = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: true),
                    description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    first_name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    last_name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    phone_number = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    company = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    country_code = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    city = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    post_code = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    address = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    state = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    latitude = table.Column<double>(type: "float", nullable: true),
                    longitude = table.Column<double>(type: "float", nullable: true),
                    created_date = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_active_listings", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "active_profiles",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    introduction_seen_on = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    user_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    first_name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    last_name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    phone_number = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    company = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    country_code = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    city = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    post_code = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    address = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    state = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    latitude = table.Column<double>(type: "float", nullable: true),
                    longitude = table.Column<double>(type: "float", nullable: true),
                    distance_unit = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: true),
                    mass_unit = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: true),
                    currency_code = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: true),
                    created_date = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_active_profiles", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "closed_listings",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    closed_on = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    accepted_offer_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    accepted_offer_owner = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    accepted_offer_monetary_value = table.Column<decimal>(type: "money", nullable: true),
                    accepted_offer_currency_code = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: true),
                    accepted_offer_created_date = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    owner = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    title = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    material_type_id = table.Column<int>(type: "int", maxLength: 2, nullable: true),
                    weight_value = table.Column<float>(type: "real", nullable: true),
                    weight_unit = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: true),
                    description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    first_name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    last_name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    phone_number = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    company = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    country_code = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    city = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    post_code = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    address = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    state = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    latitude = table.Column<double>(type: "float", nullable: true),
                    longitude = table.Column<double>(type: "float", nullable: true),
                    created_date = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_closed_listings", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "listing_image_references",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    parent_reference = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    file_name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    file_size = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_listing_image_references", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "messages",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    recipient = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    subject = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    body = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    created_date = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    seen_date = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_messages", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "new_listings",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    owner = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    title = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    material_type_id = table.Column<int>(type: "int", maxLength: 2, nullable: true),
                    weight_value = table.Column<float>(type: "real", nullable: true),
                    weight_unit = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: true),
                    description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    first_name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    last_name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    phone_number = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    company = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    country_code = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    city = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    post_code = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    address = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    state = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    latitude = table.Column<double>(type: "float", nullable: true),
                    longitude = table.Column<double>(type: "float", nullable: true),
                    created_date = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_new_listings", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "passive_listings",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    deactivation_date = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    reason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    owner = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    title = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    material_type_id = table.Column<int>(type: "int", maxLength: 2, nullable: true),
                    weight_value = table.Column<float>(type: "real", nullable: true),
                    weight_unit = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: true),
                    description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    first_name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    last_name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    phone_number = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    company = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    country_code = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    city = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    post_code = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    address = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    state = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    latitude = table.Column<double>(type: "float", nullable: true),
                    longitude = table.Column<double>(type: "float", nullable: true),
                    created_date = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_passive_listings", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "passive_profiles",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    deactivation_date = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    deactivation_reason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    user_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    first_name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    last_name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    phone_number = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    company = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    country_code = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    city = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    post_code = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    address = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    state = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    latitude = table.Column<double>(type: "float", nullable: true),
                    longitude = table.Column<double>(type: "float", nullable: true),
                    distance_unit = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: true),
                    mass_unit = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: true),
                    currency_code = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: true),
                    created_date = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_passive_profiles", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "suspicious_listings",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    marked_as_suspicious_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    reason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    owner = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    title = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    material_type_id = table.Column<int>(type: "int", maxLength: 2, nullable: true),
                    weight_value = table.Column<float>(type: "real", nullable: true),
                    weight_unit = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: true),
                    description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    first_name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    last_name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    phone_number = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    company = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    country_code = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    city = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    post_code = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    address = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    state = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    latitude = table.Column<double>(type: "float", nullable: true),
                    longitude = table.Column<double>(type: "float", nullable: true),
                    created_date = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_suspicious_listings", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "favorites",
                columns: table => new
                {
                    favorite_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    favored_by = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    marked_as_favorite_on = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    active_listing_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_favorites", x => x.favorite_id);
                    table.ForeignKey(
                        name: "FK_favorites_active_listings_active_listing_id",
                        column: x => x.active_listing_id,
                        principalTable: "active_listings",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "leads",
                columns: table => new
                {
                    lead_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_interested = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    details_seen_on = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    active_listing_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_leads", x => x.lead_id);
                    table.ForeignKey(
                        name: "FK_leads_active_listings_active_listing_id",
                        column: x => x.active_listing_id,
                        principalTable: "active_listings",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "received_offers",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    seen_date = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    active_listing_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    owner = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    monetary_value = table.Column<decimal>(type: "money", nullable: true),
                    currency_code = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: true),
                    created_date = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_received_offers", x => x.id);
                    table.ForeignKey(
                        name: "FK_received_offers_active_listings_active_listing_id",
                        column: x => x.active_listing_id,
                        principalTable: "active_listings",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "rejected_offers",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    closed_listing_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    owner = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    monetary_value = table.Column<decimal>(type: "money", nullable: true),
                    currency_code = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: true),
                    created_date = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rejected_offers", x => x.id);
                    table.ForeignKey(
                        name: "FK_rejected_offers_closed_listings_closed_listing_id",
                        column: x => x.closed_listing_id,
                        principalTable: "closed_listings",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "index_active_listing_owner",
                table: "active_listings",
                column: "owner");

            migrationBuilder.CreateIndex(
                name: "index_active_profile_user_id",
                table: "active_profiles",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "index_closed_listing_owner",
                table: "closed_listings",
                column: "owner");

            migrationBuilder.CreateIndex(
                name: "IX_favorites_active_listing_id",
                table: "favorites",
                column: "active_listing_id");

            migrationBuilder.CreateIndex(
                name: "IX_leads_active_listing_id",
                table: "leads",
                column: "active_listing_id");

            migrationBuilder.CreateIndex(
                name: "index_message_recipient",
                table: "messages",
                column: "recipient");

            migrationBuilder.CreateIndex(
                name: "index_new_listing_owner",
                table: "new_listings",
                column: "owner");

            migrationBuilder.CreateIndex(
                name: "index_passive_listing_owner",
                table: "passive_listings",
                column: "owner");

            migrationBuilder.CreateIndex(
                name: "index_passive_profile_user_id",
                table: "passive_profiles",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_received_offers_active_listing_id",
                table: "received_offers",
                column: "active_listing_id");

            migrationBuilder.CreateIndex(
                name: "IX_rejected_offers_closed_listing_id",
                table: "rejected_offers",
                column: "closed_listing_id");

            migrationBuilder.CreateIndex(
                name: "index_suspicious_listing_owner",
                table: "suspicious_listings",
                column: "owner");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "active_profiles");

            migrationBuilder.DropTable(
                name: "favorites");

            migrationBuilder.DropTable(
                name: "leads");

            migrationBuilder.DropTable(
                name: "listing_image_references");

            migrationBuilder.DropTable(
                name: "messages");

            migrationBuilder.DropTable(
                name: "new_listings");

            migrationBuilder.DropTable(
                name: "passive_listings");

            migrationBuilder.DropTable(
                name: "passive_profiles");

            migrationBuilder.DropTable(
                name: "received_offers");

            migrationBuilder.DropTable(
                name: "rejected_offers");

            migrationBuilder.DropTable(
                name: "suspicious_listings");

            migrationBuilder.DropTable(
                name: "active_listings");

            migrationBuilder.DropTable(
                name: "closed_listings");
        }
    }
}
