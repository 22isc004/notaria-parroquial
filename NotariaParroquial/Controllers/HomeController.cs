using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NotariaParroquial.Data;
using NotariaParroquial.Models;

namespace NotariaParroquial.Controllers;

[Authorize]
public class HomeController : Controller
{
    private readonly ApplicationDbContext _context;

    public HomeController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        ViewData["TotalBautismos"] = await _context.Bautismos.CountAsync();
        ViewData["TotalMatrimonios"] = await _context.Matrimonios.CountAsync();
        ViewData["TotalConfirmaciones"] = await _context.Confirmaciones.CountAsync();
        ViewData["BautismosAnio"] = await _context.Bautismos
            .Where(b => b.FechaBautismo.Year == DateTime.UtcNow.Year)
            .CountAsync();
        ViewData["MatrimoniosAnio"] = await _context.Matrimonios
            .Where(m => m.FechaMatrimonio.Year == DateTime.UtcNow.Year)
            .CountAsync();
        return View();
    }

    [AllowAnonymous]
    public IActionResult Privacy() => View();

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
