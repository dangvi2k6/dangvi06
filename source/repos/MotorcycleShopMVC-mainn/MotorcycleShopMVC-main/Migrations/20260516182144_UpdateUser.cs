using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MotorcycleShopMVC.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "brand",
                columns: table => new
                {
                    brand_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    brand_name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    country = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_brand", x => x.brand_id);
                });

            migrationBuilder.CreateTable(
                name: "part_category",
                columns: table => new
                {
                    category_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    category_name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_part_category", x => x.category_id);
                });

            migrationBuilder.CreateTable(
                name: "promotion",
                columns: table => new
                {
                    promotion_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    discount_percent = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    start_date = table.Column<DateTime>(type: "datetime", nullable: false),
                    end_date = table.Column<DateTime>(type: "datetime", nullable: false),
                    is_active = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_promotion", x => x.promotion_id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    full_name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    password_hash = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    phone_number = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    address = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    role = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    is_active = table.Column<bool>(type: "bit", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: true),
                    updated_at = table.Column<DateTime>(type: "datetime", nullable: true),
                    gender = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    birth_day = table.Column<int>(type: "int", nullable: true),
                    birth_month = table.Column<int>(type: "int", nullable: true),
                    birth_year = table.Column<int>(type: "int", nullable: true),
                    avatar_path = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "vehicle_type",
                columns: table => new
                {
                    type_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    type_name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vehicle_type", x => x.type_id);
                });

            migrationBuilder.CreateTable(
                name: "part",
                columns: table => new
                {
                    part_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    part_name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    category_id = table.Column<int>(type: "int", nullable: true),
                    brand_id = table.Column<int>(type: "int", nullable: true),
                    price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    stock_quantity = table.Column<int>(type: "int", nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    image_path = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    warranty_months = table.Column<int>(type: "int", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: true),
                    updated_at = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_part", x => x.part_id);
                    table.ForeignKey(
                        name: "FK_part_brand_brand_id",
                        column: x => x.brand_id,
                        principalTable: "brand",
                        principalColumn: "brand_id");
                    table.ForeignKey(
                        name: "FK_part_part_category_category_id",
                        column: x => x.category_id,
                        principalTable: "part_category",
                        principalColumn: "category_id");
                });

            migrationBuilder.CreateTable(
                name: "cart",
                columns: table => new
                {
                    cart_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_id = table.Column<int>(type: "int", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    updated_at = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cart", x => x.cart_id);
                    table.ForeignKey(
                        name: "FK_cart_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "notification",
                columns: table => new
                {
                    notification_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_id = table.Column<int>(type: "int", nullable: false),
                    message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    is_read = table.Column<bool>(type: "bit", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_notification", x => x.notification_id);
                    table.ForeignKey(
                        name: "FK_notification_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "orders",
                columns: table => new
                {
                    order_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_id = table.Column<int>(type: "int", nullable: false),
                    order_date = table.Column<DateTime>(type: "datetime", nullable: true),
                    total_amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    shipping_address = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    payment_status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    payment_method = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: true),
                    updated_at = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_orders", x => x.order_id);
                    table.ForeignKey(
                        name: "FK_orders_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "motorcycle",
                columns: table => new
                {
                    motorcycle_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    model_name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    brand_id = table.Column<int>(type: "int", nullable: false),
                    type_id = table.Column<int>(type: "int", nullable: true),
                    engine_capacity = table.Column<int>(type: "int", nullable: true),
                    year_from = table.Column<int>(type: "int", nullable: true),
                    year_to = table.Column<int>(type: "int", nullable: true),
                    price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    color = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    warranty_policy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    image_path = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    stock_qty = table.Column<int>(type: "int", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    updated_at = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_motorcycle", x => x.motorcycle_id);
                    table.ForeignKey(
                        name: "FK_motorcycle_brand_brand_id",
                        column: x => x.brand_id,
                        principalTable: "brand",
                        principalColumn: "brand_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_motorcycle_vehicle_type_type_id",
                        column: x => x.type_id,
                        principalTable: "vehicle_type",
                        principalColumn: "type_id");
                });

            migrationBuilder.CreateTable(
                name: "cart_item",
                columns: table => new
                {
                    cart_item_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    cart_id = table.Column<int>(type: "int", nullable: false),
                    motorcycle_id = table.Column<int>(type: "int", nullable: true),
                    part_id = table.Column<int>(type: "int", nullable: true),
                    quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cart_item", x => x.cart_item_id);
                    table.ForeignKey(
                        name: "FK_cart_item_cart_cart_id",
                        column: x => x.cart_id,
                        principalTable: "cart",
                        principalColumn: "cart_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_cart_item_motorcycle_motorcycle_id",
                        column: x => x.motorcycle_id,
                        principalTable: "motorcycle",
                        principalColumn: "motorcycle_id");
                    table.ForeignKey(
                        name: "FK_cart_item_part_part_id",
                        column: x => x.part_id,
                        principalTable: "part",
                        principalColumn: "part_id");
                });

            migrationBuilder.CreateTable(
                name: "order_item",
                columns: table => new
                {
                    order_item_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    order_id = table.Column<int>(type: "int", nullable: false),
                    motorcycle_id = table.Column<int>(type: "int", nullable: true),
                    part_id = table.Column<int>(type: "int", nullable: true),
                    quantity = table.Column<int>(type: "int", nullable: false),
                    price = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_order_item", x => x.order_item_id);
                    table.ForeignKey(
                        name: "FK_order_item_motorcycle_motorcycle_id",
                        column: x => x.motorcycle_id,
                        principalTable: "motorcycle",
                        principalColumn: "motorcycle_id");
                    table.ForeignKey(
                        name: "FK_order_item_orders_order_id",
                        column: x => x.order_id,
                        principalTable: "orders",
                        principalColumn: "order_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_order_item_part_part_id",
                        column: x => x.part_id,
                        principalTable: "part",
                        principalColumn: "part_id");
                });

            migrationBuilder.CreateTable(
                name: "part_motorcycle",
                columns: table => new
                {
                    PartId = table.Column<int>(type: "int", nullable: false),
                    MotorcycleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_part_motorcycle", x => new { x.PartId, x.MotorcycleId });
                    table.ForeignKey(
                        name: "FK_part_motorcycle_motorcycle_MotorcycleId",
                        column: x => x.MotorcycleId,
                        principalTable: "motorcycle",
                        principalColumn: "motorcycle_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_part_motorcycle_part_PartId",
                        column: x => x.PartId,
                        principalTable: "part",
                        principalColumn: "part_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "review",
                columns: table => new
                {
                    review_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_id = table.Column<int>(type: "int", nullable: false),
                    motorcycle_id = table.Column<int>(type: "int", nullable: true),
                    part_id = table.Column<int>(type: "int", nullable: true),
                    rating = table.Column<int>(type: "int", nullable: false),
                    comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_review", x => x.review_id);
                    table.ForeignKey(
                        name: "FK_review_motorcycle_motorcycle_id",
                        column: x => x.motorcycle_id,
                        principalTable: "motorcycle",
                        principalColumn: "motorcycle_id");
                    table.ForeignKey(
                        name: "FK_review_part_part_id",
                        column: x => x.part_id,
                        principalTable: "part",
                        principalColumn: "part_id");
                    table.ForeignKey(
                        name: "FK_review_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "wishlist",
                columns: table => new
                {
                    wishlist_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_id = table.Column<int>(type: "int", nullable: false),
                    motorcycle_id = table.Column<int>(type: "int", nullable: true),
                    part_id = table.Column<int>(type: "int", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_wishlist", x => x.wishlist_id);
                    table.ForeignKey(
                        name: "FK_wishlist_motorcycle_motorcycle_id",
                        column: x => x.motorcycle_id,
                        principalTable: "motorcycle",
                        principalColumn: "motorcycle_id");
                    table.ForeignKey(
                        name: "FK_wishlist_part_part_id",
                        column: x => x.part_id,
                        principalTable: "part",
                        principalColumn: "part_id");
                    table.ForeignKey(
                        name: "FK_wishlist_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "review_image",
                columns: table => new
                {
                    image_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    review_id = table.Column<int>(type: "int", nullable: false),
                    image_path = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_review_image", x => x.image_id);
                    table.ForeignKey(
                        name: "FK_review_image_review_review_id",
                        column: x => x.review_id,
                        principalTable: "review",
                        principalColumn: "review_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_cart_user_id",
                table: "cart",
                column: "user_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_cart_item_cart_id",
                table: "cart_item",
                column: "cart_id");

            migrationBuilder.CreateIndex(
                name: "IX_cart_item_motorcycle_id",
                table: "cart_item",
                column: "motorcycle_id");

            migrationBuilder.CreateIndex(
                name: "IX_cart_item_part_id",
                table: "cart_item",
                column: "part_id");

            migrationBuilder.CreateIndex(
                name: "IX_motorcycle_brand_id",
                table: "motorcycle",
                column: "brand_id");

            migrationBuilder.CreateIndex(
                name: "IX_motorcycle_type_id",
                table: "motorcycle",
                column: "type_id");

            migrationBuilder.CreateIndex(
                name: "IX_notification_user_id",
                table: "notification",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_order_item_motorcycle_id",
                table: "order_item",
                column: "motorcycle_id");

            migrationBuilder.CreateIndex(
                name: "IX_order_item_order_id",
                table: "order_item",
                column: "order_id");

            migrationBuilder.CreateIndex(
                name: "IX_order_item_part_id",
                table: "order_item",
                column: "part_id");

            migrationBuilder.CreateIndex(
                name: "IX_orders_user_id",
                table: "orders",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_part_brand_id",
                table: "part",
                column: "brand_id");

            migrationBuilder.CreateIndex(
                name: "IX_part_category_id",
                table: "part",
                column: "category_id");

            migrationBuilder.CreateIndex(
                name: "IX_part_motorcycle_MotorcycleId",
                table: "part_motorcycle",
                column: "MotorcycleId");

            migrationBuilder.CreateIndex(
                name: "IX_review_motorcycle_id",
                table: "review",
                column: "motorcycle_id");

            migrationBuilder.CreateIndex(
                name: "IX_review_part_id",
                table: "review",
                column: "part_id");

            migrationBuilder.CreateIndex(
                name: "IX_review_user_id",
                table: "review",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_review_image_review_id",
                table: "review_image",
                column: "review_id");

            migrationBuilder.CreateIndex(
                name: "IX_wishlist_motorcycle_id",
                table: "wishlist",
                column: "motorcycle_id");

            migrationBuilder.CreateIndex(
                name: "IX_wishlist_part_id",
                table: "wishlist",
                column: "part_id");

            migrationBuilder.CreateIndex(
                name: "IX_wishlist_user_id",
                table: "wishlist",
                column: "user_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "cart_item");

            migrationBuilder.DropTable(
                name: "notification");

            migrationBuilder.DropTable(
                name: "order_item");

            migrationBuilder.DropTable(
                name: "part_motorcycle");

            migrationBuilder.DropTable(
                name: "promotion");

            migrationBuilder.DropTable(
                name: "review_image");

            migrationBuilder.DropTable(
                name: "wishlist");

            migrationBuilder.DropTable(
                name: "cart");

            migrationBuilder.DropTable(
                name: "orders");

            migrationBuilder.DropTable(
                name: "review");

            migrationBuilder.DropTable(
                name: "motorcycle");

            migrationBuilder.DropTable(
                name: "part");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "vehicle_type");

            migrationBuilder.DropTable(
                name: "brand");

            migrationBuilder.DropTable(
                name: "part_category");
        }
    }
}
