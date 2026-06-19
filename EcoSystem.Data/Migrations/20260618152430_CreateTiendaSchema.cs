using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EcoSystem.Data.Migrations
{
    /// <inheritdoc />
    public partial class CreateTiendaSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Eliminar tablas preexistentes (creadas manualmente en Supabase)
            migrationBuilder.Sql("DROP TABLE IF EXISTS public.detalle_ordenes CASCADE;");
            migrationBuilder.Sql("DROP TABLE IF EXISTS public.ordenes CASCADE;");
            migrationBuilder.Sql("DROP TABLE IF EXISTS public.productos CASCADE;");
            migrationBuilder.Sql("DROP TABLE IF EXISTS public.clientes CASCADE;");
            migrationBuilder.Sql("DROP TABLE IF EXISTS public.categorias CASCADE;");

            migrationBuilder.EnsureSchema(
                name: "public");

            migrationBuilder.CreateTable(
                name: "categorias",
                schema: "public",
                columns: table => new
                {
                    id_categoria = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nombre_categoria = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    descripcion = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_categorias", x => x.id_categoria);
                });

            migrationBuilder.CreateTable(
                name: "clientes",
                schema: "public",
                columns: table => new
                {
                    id_cliente = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    ciudad = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    fecha_registro = table.Column<DateOnly>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_clientes", x => x.id_cliente);
                });

            migrationBuilder.CreateTable(
                name: "productos",
                schema: "public",
                columns: table => new
                {
                    id_producto = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    precio = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    stock = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    id_categoria = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_productos", x => x.id_producto);
                    table.ForeignKey(
                        name: "FK_productos_categorias_id_categoria",
                        column: x => x.id_categoria,
                        principalSchema: "public",
                        principalTable: "categorias",
                        principalColumn: "id_categoria",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "ordenes",
                schema: "public",
                columns: table => new
                {
                    id_orden = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_cliente = table.Column<int>(type: "integer", nullable: true),
                    fecha_orden = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    estado = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ordenes", x => x.id_orden);
                    table.ForeignKey(
                        name: "FK_ordenes_clientes_id_cliente",
                        column: x => x.id_cliente,
                        principalSchema: "public",
                        principalTable: "clientes",
                        principalColumn: "id_cliente",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "detalle_ordenes",
                schema: "public",
                columns: table => new
                {
                    id_detalle = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_orden = table.Column<int>(type: "integer", nullable: true),
                    id_producto = table.Column<int>(type: "integer", nullable: true),
                    cantidad = table.Column<int>(type: "integer", nullable: false),
                    precio_unitario = table.Column<decimal>(type: "numeric(10,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_detalle_ordenes", x => x.id_detalle);
                    table.ForeignKey(
                        name: "FK_detalle_ordenes_ordenes_id_orden",
                        column: x => x.id_orden,
                        principalSchema: "public",
                        principalTable: "ordenes",
                        principalColumn: "id_orden",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_detalle_ordenes_productos_id_producto",
                        column: x => x.id_producto,
                        principalSchema: "public",
                        principalTable: "productos",
                        principalColumn: "id_producto",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.InsertData(
                schema: "public",
                table: "categorias",
                columns: new[] { "id_categoria", "descripcion", "nombre_categoria" },
                values: new object[,]
                {
                    { 1, "Gadgets y dispositivos", "Electrónica" },
                    { 2, "Prendas de vestir", "Ropa" },
                    { 3, "Artículos para el hogar", "Hogar" },
                    { 4, "Literatura y técnicos", "Libros" }
                });

            migrationBuilder.InsertData(
                schema: "public",
                table: "clientes",
                columns: new[] { "id_cliente", "ciudad", "email", "fecha_registro", "nombre" },
                values: new object[,]
                {
                    { 1, "CDMX", "ana@email.com", new DateOnly(2023, 1, 15), "Ana García" },
                    { 2, "Guadalajara", "luis@email.com", new DateOnly(2023, 3, 22), "Luis Martínez" },
                    { 3, "Monterrey", "maria@email.com", new DateOnly(2023, 6, 10), "María López" },
                    { 4, "CDMX", "carlos@email.com", new DateOnly(2024, 1, 5), "Carlos Ruiz" },
                    { 5, "Puebla", null, new DateOnly(2024, 2, 20), "Sofía Torres" }
                });

            migrationBuilder.InsertData(
                schema: "public",
                table: "ordenes",
                columns: new[] { "id_orden", "estado", "fecha_orden", "id_cliente" },
                values: new object[,]
                {
                    { 1, "Entregado", new DateTime(2024, 1, 10, 10, 30, 0, 0, DateTimeKind.Utc), 1 },
                    { 2, "Enviado", new DateTime(2024, 2, 15, 14, 0, 0, 0, DateTimeKind.Utc), 1 },
                    { 3, "Entregado", new DateTime(2024, 1, 20, 9, 0, 0, 0, DateTimeKind.Utc), 2 },
                    { 4, "Pendiente", new DateTime(2024, 3, 1, 16, 45, 0, 0, DateTimeKind.Utc), 3 },
                    { 5, "Cancelado", new DateTime(2024, 3, 10, 11, 20, 0, 0, DateTimeKind.Utc), 2 }
                });

            migrationBuilder.InsertData(
                schema: "public",
                table: "productos",
                columns: new[] { "id_producto", "id_categoria", "nombre", "precio", "stock" },
                values: new object[,]
                {
                    { 1, 1, "Laptop Pro 15", 18500.00m, 10 },
                    { 2, 1, "Teclado Mecánico", 950.00m, 25 },
                    { 3, 2, "Playera Casual", 280.00m, 100 },
                    { 4, 3, "Cafetera Espresso", 1200.00m, 15 },
                    { 5, 1, "Cámara Mirrorless", 22000.00m, 5 },
                    { 6, 4, "SQL para Todos", 350.00m, 40 }
                });

            migrationBuilder.InsertData(
                schema: "public",
                table: "productos",
                columns: new[] { "id_producto", "id_categoria", "nombre", "precio" },
                values: new object[] { 7, 1, "Auriculares BT", 1500.00m });

            migrationBuilder.InsertData(
                schema: "public",
                table: "detalle_ordenes",
                columns: new[] { "id_detalle", "cantidad", "id_orden", "id_producto", "precio_unitario" },
                values: new object[,]
                {
                    { 1, 1, 1, 1, 18500.00m },
                    { 2, 2, 1, 2, 950.00m },
                    { 3, 3, 2, 3, 280.00m },
                    { 4, 1, 3, 4, 1200.00m },
                    { 5, 2, 3, 6, 350.00m },
                    { 6, 1, 4, 5, 22000.00m },
                    { 7, 1, 5, 2, 950.00m }
                });

            migrationBuilder.CreateIndex(
                name: "IX_clientes_email",
                schema: "public",
                table: "clientes",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_detalle_ordenes_id_orden",
                schema: "public",
                table: "detalle_ordenes",
                column: "id_orden");

            migrationBuilder.CreateIndex(
                name: "IX_detalle_ordenes_id_producto",
                schema: "public",
                table: "detalle_ordenes",
                column: "id_producto");

            migrationBuilder.CreateIndex(
                name: "IX_ordenes_id_cliente",
                schema: "public",
                table: "ordenes",
                column: "id_cliente");

            migrationBuilder.CreateIndex(
                name: "IX_productos_id_categoria",
                schema: "public",
                table: "productos",
                column: "id_categoria");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "detalle_ordenes",
                schema: "public");

            migrationBuilder.DropTable(
                name: "ordenes",
                schema: "public");

            migrationBuilder.DropTable(
                name: "productos",
                schema: "public");

            migrationBuilder.DropTable(
                name: "clientes",
                schema: "public");

            migrationBuilder.DropTable(
                name: "categorias",
                schema: "public");
        }
    }
}
