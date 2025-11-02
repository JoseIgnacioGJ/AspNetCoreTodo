using AspNetCoreTodo.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualBasic;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreTodo
{
    // Por razones obvias de seguridad, no es posible que nadie registre una nueva
    // cuenta de administrador. De hecho, el rol de administrador ni siquiera existe
    // en la BBDD todavía.

    // Puede agregar el rol de administrador más una cuenta de administrador de prueba
    // a la BBDD la primera vez que se inicie la aplicación. Eso se llama inicializar o
    // sembrar la BBDD.
    public static class SeedData
    {
        // Este método usa un IServiceProvider (la colección de servicios que se
        // configura en el método Startup.ConfigureServices() ) para obtener el
        // RoleManager y el UserManager de ASP.NET Core Identity.
        public static async Task InitializeAsync(IServiceProvider services)
        {
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
            await EnsureRolesAsync(roleManager);

            var userManager = services.GetRequiredService<UserManager<IdentityUser>>();
            await EnsureTestAdminAsync(userManager);
        }

        // Este método verifica si existe un rol de Administrador en la BBDD. Si no, crea uno.
        private static async Task EnsureRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            var alreadyExists = await roleManager.RoleExistsAsync(Constants.AdministratorRole);

            if (alreadyExists)
                return;

            await roleManager.CreateAsync(new IdentityRole(Constants.AdministratorRole));
        }

        // Si no hay un usuario con el nombre de usuario admin@todo.local en la BBDD,
        // este método creará uno y le asignará una contraseña temporal.
        private static async Task EnsureTestAdminAsync(UserManager<IdentityUser> userManager)
        {
            var testAdmin = await userManager.Users
            .Where(x => x.UserName == "admin@todo.local")
            .SingleOrDefaultAsync();

            if (testAdmin != null)
                return;

            testAdmin = new IdentityUser
            {
                UserName = "admin@todo.local",
                Email = "admin@todo.local"
            };

            await userManager.CreateAsync(testAdmin, "NotSecure123!!");
            await userManager.AddToRoleAsync(testAdmin, Constants.AdministratorRole);
        }

    }

}