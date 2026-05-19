using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NotariaParroquial.Data;
using NotariaParroquial.Models;

namespace NotariaParroquial.Controllers;

[Authorize]
public class BautismosController : Controller
{
    private readonly ApplicationDbContext _context;

    public BautismosController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index(string? buscar)
    {
        var query = _context.Bautismos.AsQueryable();

        if (!string.IsNullOrWhiteSpace(buscar))
        {
            buscar = buscar.ToLower();
            query = query.Where(b =>
                b.NombreBautizado.ToLower().Contains(buscar) ||
                b.ApellidoPaterno.ToLower().Contains(buscar) ||
                (b.ApellidoMaterno != null && b.ApellidoMaterno.ToLower().Contains(buscar)));
        }

        ViewData["Buscar"] = buscar;
        return View(await query.OrderByDescending(b => b.FechaBautismo).ToListAsync());
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();
        var bautismo = await _context.Bautismos.FirstOrDefaultAsync(b => b.Id == id);
        if (bautismo == null) return NotFound();
        return View(bautismo);
    }

    public IActionResult Create() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Bautismo bautismo)
    {
        if (!ModelState.IsValid) return View(bautismo);
        bautismo.FechaRegistro = DateTime.UtcNow;
        _context.Bautismos.Add(bautismo);
        await _context.SaveChangesAsync();
        TempData["Success"] = "Bautismo registrado exitosamente.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();
        var bautismo = await _context.Bautismos.FindAsync(id);
        if (bautismo == null) return NotFound();
        return View(bautismo);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Bautismo bautismo)
    {
        if (id != bautismo.Id) return NotFound();
        if (!ModelState.IsValid) return View(bautismo);

        _context.Update(bautismo);
        await _context.SaveChangesAsync();
        TempData["Success"] = "Bautismo actualizado exitosamente.";
        return RedirectToAction(nameof(Index));
    }

    [Authorize(Roles = "Parroco")]
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();
        var bautismo = await _context.Bautismos.FirstOrDefaultAsync(b => b.Id == id);
        if (bautismo == null) return NotFound();
        return View(bautismo);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Parroco")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var bautismo = await _context.Bautismos.FindAsync(id);
        if (bautismo != null) _context.Bautismos.Remove(bautismo);
        await _context.SaveChangesAsync();
        TempData["Success"] = "Bautismo eliminado.";
        return RedirectToAction(nameof(Index));
    }
}
