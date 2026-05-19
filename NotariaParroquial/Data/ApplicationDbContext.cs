using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NotariaParroquial.Models;

namespace NotariaParroquial.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

    public DbSet<Bautismo> Bautismos { get; set; }
    public DbSet<Matrimonio> Matrimonios { get; set; }
    public DbSet<Confirmacion> Confirmaciones { get; set; }
    public DbSet<Persona> Personas { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.HasDefaultSchema("public");
    }
}
