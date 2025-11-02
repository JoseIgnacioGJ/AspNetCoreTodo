using AspNetCoreTodo.Data;
using AspNetCoreTodo.Models;
using AspNetCoreTodo.Services;
using Humanizer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks;
using Xunit;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AspNetCoreTodo.UnitTests
{
    // Imagínese si usted o alguien más reformuló el método AddItemAsync() y se
    // olvidó de parte de esta lógica de negocios. ¡El comportamiento de su
    // aplicación podría cambiar sin que usted se dé cuenta! Puede evitar esto
    // escribiendo una prueba que verifique que esta lógica de negocios no haya
    // cambiado (incluso si la implementación interna del método cambia).
    public class TodoItemServiceShould
    {
        [Fact] // [Fact] proviene del paquete xUnit.NET, y marca este método como un método de prueba.
        public async Task AddNewItemAsIncompleteWithDueDate()
        {
            // Se usa un DbContextOptionsBuilder para configurar el proveedor de la base de datos en memoria
               var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "Test_AddNewItem").Options;

            // Configurar un contexto (conexión a la "DB") para escribir
            using (var context = new ApplicationDbContext(options))
            {
                var service = new TodoItemService(context);
                var fakeUser = new IdentityUser
                {
                    Id = "fake-000",
                    UserName = "fake@example.com"
                };
                // Se hace una llamada a AddItemAsync() y crea un tarea
                // llamada "¿Pruebas?". Y le dice al servicio que
                // lo guarde en la base de datos(en memoria).
                await service.AddItemAsync(new TodoItem
                {
                    Title = "Testing?"
                }, fakeUser);
            }

            // Este bloque "using" es para verificar que la lógica de negocio funcionó correctamente.

            // Utilice un contexto separado para leer datos desde la "DB"
            using (var context = new ApplicationDbContext(options))
            {
                var itemsInDatabase = await context.Items.CountAsync();

                // Esto es una comprobación de validez: nunca debe haber
                // más de un elemento guardado en la base de datos en memoria.
                Assert.Equal(1, itemsInDatabase);

                // Suponiendo que eso sea cierto, la prueba recupera el elemento
                // guardado con FirstAsync...
                var item = await context.Items.FirstAsync();

                // ...y luego afirma que las propiedades están establecidas en
                // los valores esperados.
                Assert.Equal("Testing?", item.Title);
                Assert.Equal(false, item.IsDone);


                // El artículo debería entregarse dentro de 3 días (más o menos un segundo)
                var difference = DateTimeOffset.Now.AddDays(3) - item.DueAt;

                // Confirmar un valor de fecha y hora es un poco complicado, ya que la
                // comparación de dos fechas para la igualdad fallará incluso si los
                // componentes de milisegundos son diferentes. En su lugar, la prueba verifica
                // que el valor DueAt esté a menos de un segundo del valor esperado.
                Assert.True(difference < TimeSpan.FromSeconds(1));
            }

        }

    }
}
