using AspNetCoreTodo.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace AspNetCoreTodo.Services
{
    /* Las interfaces hace fácil mantener tus clases desacopladas y fáciles de probar.
     Se usarán interfaces para representar el servicio que pueda interactuar con los
     elementos en la base de datos. */
    public interface ITodoItemService
    {
        Task<TodoItem[]> GetIncompleteItemsAsync(IdentityUser user);
        Task<bool> AddItemAsync(TodoItem newItem, IdentityUser user);
        Task<bool> MarkDoneAsync(Guid id, IdentityUser user);
    }
}
