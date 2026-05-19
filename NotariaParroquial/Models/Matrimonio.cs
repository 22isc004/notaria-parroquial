using System.ComponentModel.DataAnnotations;

namespace NotariaParroquial.Models;

public class Matrimonio
{
    public int Id { get; set; }

    [Required]
    [Display(Name = "Fecha de Matrimonio")]
    [DataType(DataType.Date)]
    public DateOnly FechaMatrimonio { get; set; }

    [Required, MaxLength(150)]
    [Display(Name = "Nombre del Novio")]
    public string NombreNovio { get; set; } = string.Empty;

    [Required, MaxLength(150)]
    [Display(Name = "Nombre de la Novia")]
    public string NombreNovia { get; set; } = string.Empty;

    [MaxLength(150)]
    [Display(Name = "Padre del Novio")]
    public string? PadreNovio { get; set; }

    [MaxLength(150)]
    [Display(Name = "Madre del Novio")]
    public string? MadreNovio { get; set; }

    [MaxLength(150)]
    [Display(Name = "Padre de la Novia")]
    public string? PadreNovia { get; set; }

    [MaxLength(150)]
    [Display(Name = "Madre de la Novia")]
    public string? MadreNovia { get; set; }

    [MaxLength(150)]
    [Display(Name = "Testigo 1")]
    public string? Testigo1 { get; set; }

    [MaxLength(150)]
    [Display(Name = "Testigo 2")]
    public string? Testigo2 { get; set; }

    [MaxLength(150)]
    [Display(Name = "Ministro")]
    public string? Ministro { get; set; }

    [MaxLength(50)]
    [Display(Name = "Libro")]
    public string? Libro { get; set; }

    [MaxLength(50)]
    [Display(Name = "Folio")]
    public string? Folio { get; set; }

    [MaxLength(50)]
    [Display(Name = "Acta")]
    public string? Acta { get; set; }

    [MaxLength(500)]
    [Display(Name = "Observaciones")]
    public string? Observaciones { get; set; }

    [Display(Name = "Fecha de Registro")]
    public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;
}
