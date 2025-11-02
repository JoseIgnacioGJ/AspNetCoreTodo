using System;
using System.IO;
using System.Net.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace AspNetCoreTodo.IntegrationTests
{
    // Hay algunas cosas que deben configurarse en el servidor de prueba antes de
    // cada prueba. En lugar de abarrotar la prueba con este código de
    // configuración, puede mantener esta configuración en una clase separada.

    // Esta clase se encarga de configurar un TestServer , y ayudará a mantener
    // las pruebas limpias y ordenadas.
    public class TestFixture : IDisposable
    {
        private readonly TestServer _server;
        public HttpClient Client { get; }

        public TestFixture()
        {
            // Configurar el entorno de prueba con la raíz del proyecto web
            var builder = WebApplication.CreateBuilder(new WebApplicationOptions
            {
                ContentRootPath = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "..\\..\\..\\..\\AspNetCoreTodo")
            });

            // Cargar configuración del proyecto principal
            builder.Configuration.AddJsonFile("appsettings.json", optional: true);

            // Registrar servicios y cargar controladores desde el ensamblado principal
            builder.Services.AddControllersWithViews()
                .AddApplicationPart(typeof(AspNetCoreTodo.Controllers.TodoController).Assembly); // 👈 Usa el ensamblado de la app real
            builder.Services.AddRazorPages();

            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
    });
            builder.Services.AddAuthorization();

            // Usar TestServer como host
            builder.WebHost.UseTestServer();

            var app = builder.Build();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            // Igual que en Program.cs
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages();

            // Iniciar el servidor de prueba
            app.StartAsync().GetAwaiter().GetResult();

            _server = app.GetTestServer();
            Client = app.GetTestClient();
            Client.BaseAddress = new Uri("http://localhost:8888");
        }

        public void Dispose()
        {
            Client?.Dispose();
            _server?.Dispose();
        }
    }
}
