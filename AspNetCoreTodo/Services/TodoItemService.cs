using AspNetCoreTodo.Data;
using AspNetCoreTodo.Models;
using Humanizer;
using Humanizer.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
namespace AspNetCoreTodo.Services
{

    public class TodoItemService : ITodoItemService
    {
        private readonly ApplicationDbContext _context;

        public TodoItemService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Devuelve las tareas que el usuario aún no ha completado
        public async Task<TodoItem[]> GetIncompleteItemsAsync(IdentityUser user)
        {
            return await _context.Items
             .Where(x => x.IsDone == false && x.UserId == user.Id)
             .ToArrayAsync();
            // El método ToArrayAsync le dice a Entity Framework Core que obtenga todas
            // las entidades que coincidan con el filtro y las devuelva como una matriz.
            // El método ToArrayAsync es asíncrono (devuelve un Task), por lo que debe
            // estar esperando await para obtener su valor.

        }

        // Añade una tarea a las demás que tiene el usuario.
        public async Task<bool> AddItemAsync(TodoItem newItem, IdentityUser user)
        {
            // Se inicializan las propiedades que no se pasan desde el cliente.
            newItem.Id = Guid.NewGuid();
            newItem.IsDone = false;
            newItem.DueAt = DateTimeOffset.Now.AddDays(3);
            newItem.UserId = user.Id;

            // Se añade la nueva tarea al contexto y se guarda en la base de datos.
            _context.Items.Add(newItem);

            // La nueva tarea es agregada al contacto de base de datos.
            var saveResult = await _context.SaveChangesAsync();

            // Si la operación de guardar fue satisfactorio regresará 1.
            return saveResult == 1;
        }

        public async Task<bool> MarkDoneAsync(Guid id, IdentityUser user)
        {
            // Este método usa Entity Framework Core y Where() para encontrar una
            // tarea su ID y el que tiene el usuario que añadió la tarea en la BBDD. 
            var item = await _context.Items
            .Where(x => x.Id == id && x.UserId == user.Id)
            .SingleOrDefaultAsync();
            // El método SingleOrDefaultAsync() regresará una tarea o null si esta no es encontrada.

            if (item == null)
                return false;

            // Una vez que se aseguré que el item no es nulo, es una simple cuestión de
            // configurar la propiedad "IsDone".
            item.IsDone = true;

            // Cambiando la propiedad solo afecta a la copia local de la tarea hasta que el
            // método SaveChangesAsync() es llamada para guardar el cambio en la BBDD.
            // SaveChangesAsync() regresa un nº que indica cuántas entidades fueron actualizas
            // durante la operación de guardar.
            var saveResult = await _context.SaveChangesAsync();

            // En este caso, sera o 1 (la tarea fue actualizada) o (algo malo sucedió).
            return saveResult == 1; // Una entidad debería haber sido actualizada.
        }
    }
}
