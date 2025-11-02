using Humanizer;
using System;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AspNetCoreTodo.Models
{
    // Este modelo representa a un único elemento de la BBDD.
    public class TodoItem
    {
        // "Id" es un GUID o un Identificador Global Único.
        public Guid Id { get; set; } 

        // De forma predeterminada, "IsDone" será falso para todos los nuevos elementos.
        public bool IsDone { get; set; }

        // "Title" mantendrá el nombre o descripción de la tarea pendiente.
        // El atributo "[Required]" le dice ASP.NET Core que esta cadena no
        // puede ser nula o vacia.
        [Required]
        public string Title { get; set; }

        //  "DueAt" es un DateTimeOffset, que almacena un fecha/hora con la diferencia de
        //  horario con el UTC.
        public DateTimeOffset? DueAt { get; set; }

        // "UserId" se usa para que cada elemento pueda "recordar" al usuario que lo posee.
        public string UserId { get; set; }
    }
}