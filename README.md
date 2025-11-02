# AspNetCoreTodo
En esta pequeña web, el usuario debe registrarse y luego iniciar sesión para poder acceder. Luego, en la sección "My to-dos", el usuario puede crear, leer y/o eliminar las tareas que tiene asignadas. El usuario administrador, además de disponer la opción "My to-dos", también puede consultar todos los usuarios registrados junto con sus respectivos Ids.

Durante el desarrollo de este proyecto ASP.NET Core, se pudo aprender:
- Los fundamentos del patrón MVC (Modelo-Vista-Controlador).
- Cómo funciona el código del lado del cliente (HTML, CSS y Javascript) junto con el código del lado del servidor.
- Qué es la inyección de dependencias y porque es tan útil.
- Cómo leer y escribir datos a una base de datos.
- Cómo agregar inicio de sesión, registro y seguridad.
- Cómo desplegar la aplicación en la web.

# UnitTests
Se trata de una pueba unitaria. Verifica que la lógica de negocios del método "AddItemAsync()" no haya cambiado (incluso si la implementación interna del método cambia).

# IntegrationTests
Se trata de una pueba de integración. Realiza una solicitud anónima (sin iniciar sesión) a la ruta "/todo" y verifica que el navegador se redirige a la página de inicio de sesión.
