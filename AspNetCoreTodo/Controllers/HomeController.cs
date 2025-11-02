using AspNetCoreTodo.Models;
using Humanizer;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace AspNetCoreTodo.Controllers;

// Este controlador genera la pantalla de bienvenida por defecto
// cuando visitas http://localhost:5000.
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    // Las rutas que son manejadas por el controlador son llamadas acciones, y son
    // representadas por métodos en la clase controlador. Por ejemplo, el "HomeController"
    // incluye tres métodos de acción ("Index", "Privacy" y "Error") las cuales son mapeadas
    // por ASP.NET Core a estas rutas URLs.
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
