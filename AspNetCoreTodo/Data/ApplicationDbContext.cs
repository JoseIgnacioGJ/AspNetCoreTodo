using AspNetCoreTodo.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace AspNetCoreTodo.Data;

// Es el contexto de la base de datos.
public class ApplicationDbContext: IdentityDbContext<IdentityUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options)
    {

    }

    // Un DbSet representa una tabla o colección en la BBDD. Al crear una propiedad
    // DbSet<TodoItem> llamada Items le está diciendo a Entity Framework Core que
    // SE desea almacenar las entidades TodoItem en una tabla llamada Items.
    public DbSet<TodoItem> Items { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
    }
}