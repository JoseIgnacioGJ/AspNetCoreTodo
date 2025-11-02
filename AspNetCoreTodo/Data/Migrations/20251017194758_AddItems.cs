using AspNetCoreTodo.Models;
using Humanizer;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using Xunit.Abstractions;
using static System.Runtime.InteropServices.JavaScript.JSType;

#nullable disable

namespace AspNetCoreTodo.Data.Migrations
{
    // Esta migración hace un seguimiento de los cambios en la estructura de la BBDD
    // Items a lo largo del tiempo. Permite deshacer un conjunto de cambios o crear
    // una segunda BBDD con la misma estructura que la primera. Con las migraciones,
    // se tiene un historial completo de modificaciones, como agregar o eliminar
    // columnas (y tablas completas).

    /// <inheritdoc />
    public partial class AddItems : Migration
    {
        // El método "Up" se ejecuta cuando aplica la migración a la BBDD. Dado que se
        // agregó un "DbSet<TodoItem>" al contexto de la BBDD, Entity Framework Core
        // creará una tabla "Items" (con columnas que coinciden con un "TodoItem")
        // cuando aplique la migración.

        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Items",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    IsDone = table.Column<bool>(type: "INTEGER", nullable: false),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    DueAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Items", x => x.Id);
                });
        }

        // El método "Down" hace lo contrario: si se necesita deshacer (roll back) la
        // migración, la tabla "Items" se eliminará.

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Items");
        }
    }
}
