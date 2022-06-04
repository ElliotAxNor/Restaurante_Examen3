using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Restaurante.Data;
using Restaurante.Models;

namespace Restaurante.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ApplicationDbContext _context;

    public HomeController(ILogger<HomeController> logger, ApplicationDbContext applicationDbContext)
    {
        _logger = logger;
        _context = applicationDbContext;
    }

    public async Task<IActionResult> Index()
    {
        return _context.Categorias != null ?
                    View(await _context.Categorias.ToListAsync()) :
                    Problem("Entity set 'ApplicationDbContext.Categorias'  is null.");
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
