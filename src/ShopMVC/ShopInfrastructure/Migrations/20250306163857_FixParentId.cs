using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShopInfrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixParentId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "categories",
                columns: table => new
                {
                    cg_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    cg_name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    cg_description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    parent_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_categories", x => x.cg_id);
                    table.ForeignKey(
                        name: "FK_categories_categories_parent_id",
                        column: x => x.parent_id,
                        principalTable: "categories",
                        principalColumn: "cg_id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "countries",
                columns: table => new
                {
                    co_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    co_name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_countries", x => x.co_id);
                });

            migrationBuilder.CreateTable(
                name: "shipping_companies",
                columns: table => new
                {
                    sc_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    sc_name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    sc_pricing = table.Column<decimal>(type: "decimal(18,0)", nullable: true),
                    sc_avg_time_need = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_shipping_companies", x => x.sc_id);
                });

            migrationBuilder.CreateTable(
                name: "// users",
                columns: table => new
                {
                    ur_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ur_nickname = table.Column<string>(type: "char(10)", unicode: false, fixedLength: true, maxLength: 10, nullable: true),
                    ur_birthdate = table.Column<DateTime>(type: "datetime", nullable: true),
                    ur_email = table.Column<string>(type: "char(10)", unicode: false, fixedLength: true, maxLength: 10, nullable: true),
                    ur_role = table.Column<string>(type: "char(10)", unicode: false, fixedLength: true, maxLength: 10, nullable: true),
                    ur_country_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.ur_id);
                    table.ForeignKey(
                        name: "FK_users_countries",
                        column: x => x.ur_country_id,
                        principalTable: "countries",
                        principalColumn: "co_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "manufacturers",
                columns: table => new
                {
                    mn_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    mn_name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    mn_contact_info = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    country_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_manufacturers", x => x.mn_id);
                    table.ForeignKey(
                        name: "FK_manufacturers_countries",
                        column: x => x.country_id,
                        principalTable: "countries",
                        principalColumn: "co_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "shipings",
                columns: table => new
                {
                    sh_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    sh_adress = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    country_id = table.Column<int>(type: "int", nullable: true),
                    shipping_company_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_shipings", x => x.sh_id);
                    table.ForeignKey(
                        name: "FK_shipings_countries",
                        column: x => x.country_id,
                        principalTable: "countries",
                        principalColumn: "co_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_shipings_shipping_companies",
                        column: x => x.shipping_company_id,
                        principalTable: "shipping_companies",
                        principalColumn: "sc_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "carts",
                columns: table => new
                {
                    ct_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ct_quantity = table.Column<int>(type: "int", nullable: true),
                    ct_price = table.Column<int>(type: "int", nullable: true),
                    user_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_carts", x => x.ct_id);
                    table.ForeignKey(
                        name: "FK_carts_users",
                        column: x => x.user_id,
                        principalTable: "// users",
                        principalColumn: "ur_id");
                });

            migrationBuilder.CreateTable(
                name: "products",
                columns: table => new
                {
                    pd_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    pd_name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    pd_price = table.Column<decimal>(type: "money", nullable: true),
                    pd_measurements = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    pd_quantity = table.Column<int>(type: "int", nullable: true),
                    pd_discount = table.Column<double>(type: "float", nullable: true),
                    pd_about = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    manufacturer_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_product", x => x.pd_id);
                    table.ForeignKey(
                        name: "FK_products_manufacturers",
                        column: x => x.manufacturer_id,
                        principalTable: "manufacturers",
                        principalColumn: "mn_id");
                });

            migrationBuilder.CreateTable(
                name: "receipts",
                columns: table => new
                {
                    rp_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    rp_date_created = table.Column<DateTime>(type: "datetime", nullable: true),
                    rp_quantity = table.Column<int>(type: "int", nullable: true),
                    rp_total = table.Column<decimal>(type: "decimal(18,0)", nullable: true),
                    rp_payment = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    rp_discount = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    rp_about = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    shipping_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_receipts", x => x.rp_id);
                    table.ForeignKey(
                        name: "FK_receipts_shipings",
                        column: x => x.shipping_id,
                        principalTable: "shipings",
                        principalColumn: "sh_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "product_carts",
                columns: table => new
                {
                    pc_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    product_id = table.Column<int>(type: "int", nullable: true),
                    cart_id = table.Column<int>(type: "int", nullable: true),
                    pc_quantity = table.Column<int>(type: "int", nullable: true),
                    pc_price = table.Column<decimal>(type: "decimal(18,0)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_product_carts", x => x.pc_id);
                    table.ForeignKey(
                        name: "FK_product_carts_carts",
                        column: x => x.cart_id,
                        principalTable: "carts",
                        principalColumn: "ct_id");
                    table.ForeignKey(
                        name: "FK_product_carts_products",
                        column: x => x.product_id,
                        principalTable: "products",
                        principalColumn: "pd_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "product_categories",
                columns: table => new
                {
                    pct_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    product_id = table.Column<int>(type: "int", nullable: true),
                    category_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_product_categories", x => x.pct_id);
                    table.ForeignKey(
                        name: "FK_product_categories_categories_1",
                        column: x => x.category_id,
                        principalTable: "categories",
                        principalColumn: "cg_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_product_categories_products",
                        column: x => x.product_id,
                        principalTable: "products",
                        principalColumn: "pd_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "orders",
                columns: table => new
                {
                    od_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    od_user = table.Column<int>(type: "int", nullable: true),
                    od_total = table.Column<decimal>(type: "decimal(18,0)", nullable: true),
                    od_discount = table.Column<double>(type: "float", nullable: true),
                    od_payment = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    od_notes = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    receipt_id = table.Column<int>(type: "int", nullable: true),
                    product_id = table.Column<int>(type: "int", nullable: true),
                    shipping_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_orders", x => x.od_id);
                    table.ForeignKey(
                        name: "FK_orders_products",
                        column: x => x.product_id,
                        principalTable: "products",
                        principalColumn: "pd_id");
                    table.ForeignKey(
                        name: "FK_orders_receipts",
                        column: x => x.receipt_id,
                        principalTable: "receipts",
                        principalColumn: "rp_id");
                    table.ForeignKey(
                        name: "FK_orders_shipings",
                        column: x => x.shipping_id,
                        principalTable: "shipings",
                        principalColumn: "sh_id");
                    table.ForeignKey(
                        name: "FK_orders_users",
                        column: x => x.od_user,
                        principalTable: "// users",
                        principalColumn: "ur_id");
                });

            migrationBuilder.CreateTable(
                name: "product_orders",
                columns: table => new
                {
                    po_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    product_id = table.Column<int>(type: "int", nullable: true),
                    order_id = table.Column<int>(type: "int", nullable: true),
                    po_price = table.Column<decimal>(type: "decimal(18,0)", nullable: true),
                    po_quantity = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_product_orders", x => x.po_id);
                    table.ForeignKey(
                        name: "FK_product_orders_orders_1",
                        column: x => x.order_id,
                        principalTable: "orders",
                        principalColumn: "od_id");
                    table.ForeignKey(
                        name: "FK_product_orders_products",
                        column: x => x.product_id,
                        principalTable: "products",
                        principalColumn: "pd_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_// users_ur_country_id",
                table: "// users",
                column: "ur_country_id");

            migrationBuilder.CreateIndex(
                name: "unique_user_cart",
                table: "carts",
                column: "user_id",
                unique: true,
                filter: "[user_id] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_categories_parent_id",
                table: "categories",
                column: "parent_id");

            migrationBuilder.CreateIndex(
                name: "IX_manufacturers_country_id",
                table: "manufacturers",
                column: "country_id");

            migrationBuilder.CreateIndex(
                name: "IX_orders_od_user",
                table: "orders",
                column: "od_user");

            migrationBuilder.CreateIndex(
                name: "IX_orders_product_id",
                table: "orders",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "IX_orders_receipt_id",
                table: "orders",
                column: "receipt_id");

            migrationBuilder.CreateIndex(
                name: "IX_orders_shipping_id",
                table: "orders",
                column: "shipping_id");

            migrationBuilder.CreateIndex(
                name: "IX_product_carts_cart_id",
                table: "product_carts",
                column: "cart_id");

            migrationBuilder.CreateIndex(
                name: "IX_product_carts_product_id",
                table: "product_carts",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "IX_product_categories_category_id",
                table: "product_categories",
                column: "category_id");

            migrationBuilder.CreateIndex(
                name: "IX_product_categories_product_id",
                table: "product_categories",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "IX_product_orders_order_id",
                table: "product_orders",
                column: "order_id");

            migrationBuilder.CreateIndex(
                name: "IX_product_orders_product_id",
                table: "product_orders",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "IX_products_manufacturer_id",
                table: "products",
                column: "manufacturer_id");

            migrationBuilder.CreateIndex(
                name: "IX_receipts_shipping_id",
                table: "receipts",
                column: "shipping_id");

            migrationBuilder.CreateIndex(
                name: "IX_shipings_country_id",
                table: "shipings",
                column: "country_id");

            migrationBuilder.CreateIndex(
                name: "IX_shipings_shipping_company_id",
                table: "shipings",
                column: "shipping_company_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "product_carts");

            migrationBuilder.DropTable(
                name: "product_categories");

            migrationBuilder.DropTable(
                name: "product_orders");

            migrationBuilder.DropTable(
                name: "carts");

            migrationBuilder.DropTable(
                name: "categories");

            migrationBuilder.DropTable(
                name: "orders");

            migrationBuilder.DropTable(
                name: "products");

            migrationBuilder.DropTable(
                name: "receipts");

            migrationBuilder.DropTable(
                name: "// users");

            migrationBuilder.DropTable(
                name: "manufacturers");

            migrationBuilder.DropTable(
                name: "shipings");

            migrationBuilder.DropTable(
                name: "countries");

            migrationBuilder.DropTable(
                name: "shipping_companies");
        }
    }
}
