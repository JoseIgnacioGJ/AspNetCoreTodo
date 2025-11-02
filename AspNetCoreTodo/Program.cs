using AspNetCoreTodo;
using AspNetCoreTodo.Data;
using AspNetCoreTodo.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Añadir servicios al contenedor.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = false; // <= tiene que estar a "true"
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultUI()
.AddDefaultTokenProviders();


builder.Services.AddControllersWithViews();

// Esta linea le especifica a ASP.NET Core que cada vez que se solicite ITodoItemService
// en un constructor deberá usar la clase TodoItemService.
builder.Services.AddScoped<ITodoItemService, TodoItemService>(); // código que iría en Startup.cs, pero que se eliminó

var app = builder.Build();

// Llama al método de inicialización de la BBDD. Dicho método se encuentra al final de este archivo.
InitializeDatabase(app);

// Configurar la canalización de solicitudes HTTP.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapRazorPages()
   .WithStaticAssets();

app.Run();


static void InitializeDatabase(IHost host)
{
    using (var scope = host.Services.CreateScope())
    {
        // Se obtiene la colección de servicios que necesita SeedData.InitializeAsync()...
        var services = scope.ServiceProvider;

        // ... y luego ejecuta el método para inicializar la BBDD.
        try
        {
            SeedData.InitializeAsync(services).Wait();
        }

        //  Si algo sale mal, se registra un error.
        catch (Exception ex)
        {
            var logger = services
            .GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "Error occurred seeding the DB.");
        }
    }
}