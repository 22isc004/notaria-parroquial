using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NotariaParroquial.Data;
using NotariaParroquial.Models;

namespace NotariaParroquial.Controllers;

[Authorize]
public class MatrimoniosController : Controller
{
    private readonly ApplicationDbContext _context;

    public MatrimoniosController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index(string? buscar)
    {
        var query = _context.Matrimonios.AsQueryable();

        if (!string.IsNullOrWhiteSpace(buscar))
        {
            buscar = buscar.ToLower();
            query = query.Where(m =>
                m.NombreNovio.ToLower().Contains(buscar) ||
                m.NombreNovia.ToLower().Contains(buscar));
        }

        ViewData["Buscar"] = buscar;
        return View(await query.OrderByDescending(m => m.FechaMatrimonio).ToListAsync());
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();
        var matrimonio = await _context.Matrimonios.FirstOrDefaultAsync(m => m.Id == id);
        if (matrimonio == null) return NotFound();
        return View(matrimonio);
    }

    public IActionResult Create() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Matrimonio matrimonio)
    {
        if (!ModelState.IsValid) return View(matrimonio);
        matrimonio.FechaRegistro = DateTime.UtcNow;
        _context.Matrimonios.Add(matrimonio);
        await _context.SaveChangesAsync();
        TempData["Success"] = "Matrimonio registrado exitosamente.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();
        var matrimonio = await _context.Matrimonios.FindAsync(id);
        if (matrimonio == null) return NotFound();
        return View(matrimonio);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Matrimonio matrimonio)
    {
        if (id != matrimonio.Id) return NotFound();
        if (!ModelState.IsValid) return View(matrimonio);
        _context.Update(matrimonio);
        await _context.SaveChangesAsync();
        TempData["Success"] = "Matrimonio actualizado exitosamente.";
        return RedirectToAction(nameof(Index));
    }

    [Authorize(Roles = "Parroco")]
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();
        var matrimonio = await _context.Matrimonios.FirstOrDefaultAsync(m => m.Id == id);
        if (matrimonio == null) return NotFound();
        return View(matrimonio);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Parroco")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var matrimonio = await _context.Matrimonios.FindAsync(id);
        if (matrimonio != null) _context.Matrimonios.Remove(matrimonio);
        await _context.SaveChangesAsync();
        TempData["Success"] = "Matrimonio eliminado.";
        return RedirectToAction(nameof(Index));
    }
}
