using AspNetCoreTodo.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Threading.Tasks;
namespace AspNetCoreTodo.Controllers
{
    // La configuración de la propiedad Roles en el atributo [Authorize] garantizará
    // que el usuario tenga que iniciar sesión y se le asigne el rol de Administrador
    // para poder ver la página.
    [Authorize(Roles = "Administrator")]
    public class ManageUsersController : Controller
    {
        // Esta variable sirve para obtener al usuario actual en la acción Index.
        private readonly UserManager<IdentityUser> _userManager;

        public ManageUsersController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            // Se obtiene la lista de usuarios administrativos de la capa de servicio
            var admins = (await _userManager
            .GetUsersInRoleAsync("Administrator"))
            .ToArray();

            // Se obtiene la lista de todos los usuarios de la capa de servicio
            var everyone = await _userManager.Users
            .ToArrayAsync();

            // Se coloca los usuarios (todos y solo los administrativos)
            // dentro del modelo de la vista.
            var model = new ManageUsersViewModel
            {
                Administrators = admins,
                Everyone = everyone
            };

            // Se enlaza este modelo a la vista y se visualiza.
            return View(model);
        }
    }
}