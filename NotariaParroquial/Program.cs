using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NotariaParroquial.Data;
using NotariaParroquial.Models;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 8;
    options.Password.RequireNonAlphanumeric = false;
    options.SignIn.RequireConfirmedAccount = false;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/AccessDenied";
    options.ExpireTimeSpan = TimeSpan.FromHours(8);
});

builder.Services.AddControllersWithViews();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<ApplicationDbContext>();

    // Si las tablas ya existen sin historial de migraciones (deploy previo con distinto esquema),
    // se elimina el esquema y se recrea correctamente.
    try
    {
        context.Database.Migrate();
    }
    catch (Exception ex) when (
        ex.ToString().Contains("42P07") ||
        ex.ToString().Contains("already exists"))
    {
        context.Database.EnsureDeleted();
        context.Database.Migrate();
    }

    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();

    string[] roles = { "Parroco", "Secretaria" };
    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
            await roleManager.CreateAsync(new IdentityRole(role));
    }

    var parrocoEmail = "parroco@notaria.com";
    if (await userManager.FindByEmailAsync(parrocoEmail) == null)
    {
        var parroco = new ApplicationUser
        {
            UserName = parrocoEmail,
            Email = parrocoEmail,
            NombreCompleto = "Párroco Principal",
            Rol = "Parroco",
            EmailConfirmed = true
        };
        await userManager.CreateAsync(parroco, "Notaria2024!");
        await userManager.AddToRoleAsync(parroco, "Parroco");
    }
}

app.Run();
