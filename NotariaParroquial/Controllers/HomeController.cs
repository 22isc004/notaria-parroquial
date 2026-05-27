using System.Diagnostics;
using System.Text.Json;
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
        var year  = DateTime.UtcNow.Year;
        var month = DateTime.UtcNow.Month;

        ViewData["TotalBautismos"]      = await _context.Bautismos.CountAsync();
        ViewData["TotalMatrimonios"]    = await _context.Matrimonios.CountAsync();
        ViewData["TotalConfirmaciones"] = await _context.Confirmaciones.CountAsync();

        ViewData["BautismosAnio"]      = await _context.Bautismos.Where(b => b.FechaBautismo.Year == year).CountAsync();
        ViewData["MatrimoniosAnio"]    = await _context.Matrimonios.Where(m => m.FechaMatrimonio.Year == year).CountAsync();
        ViewData["ConfirmacionesAnio"] = await _context.Confirmaciones.Where(c => c.FechaConfirmacion.Year == year).CountAsync();

        var bHoy = await _context.Bautismos.Where(b => b.FechaBautismo.Year == year && b.FechaBautismo.Month == month).CountAsync();
        var mHoy = await _context.Matrimonios.Where(m => m.FechaMatrimonio.Year == year && m.FechaMatrimonio.Month == month).CountAsync();
        var cHoy = await _context.Confirmaciones.Where(c => c.FechaConfirmacion.Year == year && c.FechaConfirmacion.Month == month).CountAsync();
        ViewData["TotalEsteMes"] = bHoy + mHoy + cHoy;
        ViewData["MesNombre"]    = new System.Globalization.CultureInfo("es-MX").DateTimeFormat.GetMonthName(month);

        var bMes = await _context.Bautismos
            .Where(b => b.FechaBautismo.Year == year)
            .GroupBy(b => b.FechaBautismo.Month)
            .Select(g => new { Mes = g.Key, Total = g.Count() })
            .ToListAsync();

        var mMes = await _context.Matrimonios
            .Where(m => m.FechaMatrimonio.Year == year)
            .GroupBy(m => m.FechaMatrimonio.Month)
            .Select(g => new { Mes = g.Key, Total = g.Count() })
            .ToListAsync();

        var cMes = await _context.Confirmaciones
            .Where(c => c.FechaConfirmacion.Year == year)
            .GroupBy(c => c.FechaConfirmacion.Month)
            .Select(g => new { Mes = g.Key, Total = g.Count() })
            .ToListAsync();

        ViewData["BautismosData"] = JsonSerializer.Serialize(
            Enumerable.Range(1, 12).Select(m => bMes.FirstOrDefault(x => x.Mes == m)?.Total ?? 0).ToArray());
        ViewData["MatrimoniosData"] = JsonSerializer.Serialize(
            Enumerable.Range(1, 12).Select(m => mMes.FirstOrDefault(x => x.Mes == m)?.Total ?? 0).ToArray());
        ViewData["ConfirmacionesData"] = JsonSerializer.Serialize(
            Enumerable.Range(1, 12).Select(m => cMes.FirstOrDefault(x => x.Mes == m)?.Total ?? 0).ToArray());

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
