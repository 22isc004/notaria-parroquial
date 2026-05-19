using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NotariaParroquial.Data;
using NotariaParroquial.Models;

namespace NotariaParroquial.Controllers;

[Authorize]
public class ConfirmacionesController : Controller
{
    private readonly ApplicationDbContext _context;

    public ConfirmacionesController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index(string? buscar)
    {
        var query = _context.Confirmaciones.AsQueryable();

        if (!string.IsNullOrWhiteSpace(buscar))
        {
            buscar = buscar.ToLower();
            query = query.Where(c =>
                c.NombreConfirmado.ToLower().Contains(buscar) ||
                c.ApellidoPaterno.ToLower().Contains(buscar));
        }

        ViewData["Buscar"] = buscar;
        return View(await query.OrderByDescending(c => c.FechaConfirmacion).ToListAsync());
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();
        var conf = await _context.Confirmaciones.FirstOrDefaultAsync(c => c.Id == id);
        if (conf == null) return NotFound();
        return View(conf);
    }

    public IActionResult Create() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Confirmacion confirmacion)
    {
        if (!ModelState.IsValid) return View(confirmacion);
        confirmacion.FechaRegistro = DateTime.UtcNow;
        _context.Confirmaciones.Add(confirmacion);
        await _context.SaveChangesAsync();
        TempData["Success"] = "Confirmación registrada exitosamente.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();
        var conf = await _context.Confirmaciones.FindAsync(id);
        if (conf == null) return NotFound();
        return View(conf);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Confirmacion confirmacion)
    {
        if (id != confirmacion.Id) return NotFound();
        if (!ModelState.IsValid) return View(confirmacion);
        _context.Update(confirmacion);
        await _context.SaveChangesAsync();
        TempData["Success"] = "Confirmación actualizada exitosamente.";
        return RedirectToAction(nameof(Index));
    }

    [Authorize(Roles = "Parroco")]
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();
        var conf = await _context.Confirmaciones.FirstOrDefaultAsync(c => c.Id == id);
        if (conf == null) return NotFound();
        return View(conf);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Parroco")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var conf = await _context.Confirmaciones.FindAsync(id);
        if (conf != null) _context.Confirmaciones.Remove(conf);
        await _context.SaveChangesAsync();
        TempData["Success"] = "Confirmación eliminada.";
        return RedirectToAction(nameof(Index));
    }
}
