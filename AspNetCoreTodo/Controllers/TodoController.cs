using AspNetCoreTodo.Models;
using AspNetCoreTodo.Services;
using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;
using Xunit.Abstractions;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AspNetCoreTodo.Controllers
{
    // Este controlador es para la funcionalidad de la lista de tareas

    // El atributo [Authorize] en ASP.NET Core sirve para requerir autenticación
    // (no de autorización) para todas las acciones del "TodoController".
    [Authorize]
    public class TodoController : Controller
    {
        // Esta variable te deja usar el servicio desde el método de acción Index después.
        private readonly ITodoItemService _todoItemService;

        // Esta variable sirve para obtener al usuario actual en la acción Index.
        private readonly UserManager<IdentityUser> _userManager;

        public TodoController(ITodoItemService todoItemService, UserManager<IdentityUser> userManager)
        {
            _todoItemService = todoItemService;
            _userManager = userManager;
        }

        // El tipo de retorno "IActionResult" te da la flexibilidad de regresar cualquiera
        // de estos desde una acción. En este caso, el controlador será responsable
        // de obtener la lista de tareas desde la BBDD, poniendo estas tareas en
        // un modelo que la vista pueda entender, y enviara la vista de regreso al
        // navegador del usuario.

        public async Task<IActionResult> Index()
        {
            // Si hay un usuario que ha iniciado sesión, la propiedad "User" contiene un
            // objeto ligero con algo de la info del usuario. El "UserManager" usa esto
            // para buscar los detalles completos del usuario en la BBDD a través del
            // método GetUserAsync().
            var currentUser = await _userManager.GetUserAsync(User);

            // El valor de "currentUser" nunca debe ser nulo, porque el atributo
            // [Authorize] está presente en el controlador. Sin embargo, es una buena
            // idea hacer un control de cordura, por si acaso. El método "Challenge()"
            // para forzar al usuario a iniciar sesión nuevamente si falta su info.
            if (currentUser == null) 
                return Challenge();

            // Se obtiene la lista de tareas de la capa de servicio.
            var items = await _todoItemService.GetIncompleteItemsAsync(currentUser);

            // Se coloca las tareas dentro del modelo de la vista.
            var model = new TodoViewModel()
            {
                Items = items
            };

            // Se enlaza este modelo a la vista y se visualiza.
            return View(model);
        }

        // El atributo [ValidateAntiForgeryToken] antes de la acción le dice a ASP.NET
        // Core que este debe buscar (y verificar) el código oculto de verificación que
        // fue agregado al formulario. El código de verificación se asegura que la aplicación
        // es actualmente la única que muestra el formulario y recibe los datos del formulario.

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddItem(TodoItem newItem)
        {
            // Comprueba si el "ModelState" es válido (el resultado de la validación del modelo).

            // Es opcional validar el modelo
            /*if (!ModelState.IsValid)
            {
                return RedirectToAction("Index");
            }*/

            // Si hay un usuario que ha iniciado sesión, la propiedad "User" contiene un
            // objeto ligero con algo de la info del usuario. El "UserManager" usa esto
            // para buscar los detalles completos del usuario en la BBDD a través del
            // método GetUserAsync().
            var currentUser = await _userManager.GetUserAsync(User);

            // El valor de "currentUser" nunca debe ser nulo, porque el atributo
            // [Authorize] está presente en el controlador. Sin embargo, es una buena
            // idea hacer un control de cordura, por si acaso. El método "Challenge()"
            // para forzar al usuario a iniciar sesión nuevamente si falta su info.
            if (currentUser == null)
                return Challenge();

            // Después, el controlador llama a la capa de servicio para realizar la operación
            // de BBDD actual de guardar la nueva tarea junto con su usuario. El método
            // "AddItemAsync" regresa "true" or "false" dependiendo de si la tarea fue agregada
            // satisfactoriamente a la base.
            var successful = await _todoItemService.AddItemAsync(newItem, currentUser);

            // Si este falla por alguna razón, la acción regresará un error "HTTP 400 Bad
            // Request" junto con el objeto que contiene un mensaje de error.
            if (!successful)
            {
                return BadRequest("Could not add item.");
            }

            // Finalmente, si todo es completado sin errores, la acción redirige el
            // navegador a la ruta "/Todo/Index", la cual refresca la página y muestra
            // la lista de tareas actualizada de cada usuario.
            return RedirectToAction("Index");
        }

        // El atributo [ValidateAntiForgeryToken] antes de la acción le dice a ASP.NET
        // Core que este debe buscar (y verificar) el código oculto de verificación que
        // fue agregado al formulario. El código de verificación se asegura que la aplicación
        // es actualmente la única que muestra el formulario y recibe los datos del formulario.


        /**/
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarkDone(Guid id)
        {
            // Si la variable "id" está vacía, la acción le dice al navegador que redirija
            // a /Todo/Index y actualice la página.
            if (id == Guid.Empty)
            {
                return RedirectToAction("Index");
            }

            // Si hay un usuario que ha iniciado sesión, la propiedad "User" contiene un
            // objeto ligero con algo de la info del usuario. El "UserManager" usa esto
            // para buscar los detalles completos del usuario en la BBDD a través del
            // método GetUserAsync().
            var currentUser = await _userManager.GetUserAsync(User);

            // El valor de "currentUser" nunca debe ser nulo, porque el atributo
            // [Authorize] está presente en el controlador. Sin embargo, es una buena
            // idea hacer un control de cordura, por si acaso. El método "Challenge()"
            // para forzar al usuario a iniciar sesión nuevamente si falta su info.
            if (currentUser == null) 
                return Challenge();

            // A continuación, el controlador debe llamar a la capa de servicio para
            // actualizar la BBDD. Esto será manejado por un nuevo método llamado
            // "MarkDoneAsync" en la interfaz "ITodoItemService", que devolverá
            // verdadero o falso dependiendo de si la actualización tuvo éxito.
            var successful = await _todoItemService.MarkDoneAsync(id, currentUser);

            // Si este falla por alguna razón, la acción regresará un error "HTTP 400 Bad
            // Request" junto con el objeto que contiene un mensaje de error.
            if (!successful)
            {
                return BadRequest("Could not mark item as done.");
            }

            // Finalmente, si todo se ve bien, el navegador se redirige a la acción
            // "/Todo/Index" y la página se actualiza.
            return RedirectToAction("Index");
        }

    }
}
