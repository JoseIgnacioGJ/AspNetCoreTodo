using System.Runtime.ConstrainedExecution;

namespace AspNetCoreTodo.Models
{
    // La vista puede necesitar mostrar dos o cientos tareas pendientes.
    // Debido a esto, la vista modelo puede ser una clase separada que mantienen
    // un arreglo de TodoItem.
    public class TodoViewModel
    {
        public TodoItem[] Items { get; set; }
    }
}