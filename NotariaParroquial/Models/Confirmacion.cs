using System.ComponentModel.DataAnnotations;

namespace NotariaParroquial.Models;

public class Confirmacion
{
    public int Id { get; set; }

    [Required]
    [Display(Name = "Fecha de Confirmación")]
    [DataType(DataType.Date)]
    public DateOnly FechaConfirmacion { get; set; }

    [Required, MaxLength(100)]
    [Display(Name = "Nombre del Confirmado")]
    public string NombreConfirmado { get; set; } = string.Empty;

    [Required, MaxLength(100)]
    [Display(Name = "Apellido Paterno")]
    public string ApellidoPaterno { get; set; } = string.Empty;

    [MaxLength(100)]
    [Display(Name = "Apellido Materno")]
    public string? ApellidoMaterno { get; set; }

    [MaxLength(150)]
    [Display(Name = "Nombre del Padre")]
    public string? NombrePadre { get; set; }

    [MaxLength(150)]
    [Display(Name = "Nombre de la Madre")]
    public string? NombreMadre { get; set; }

    [MaxLength(150)]
    [Display(Name = "Padrino/Madrina")]
    public string? Padrino { get; set; }

    [MaxLength(150)]
    [Display(Name = "Ministro")]
    public string? Ministro { get; set; }

    [MaxLength(150)]
    [Display(Name = "Obispo")]
    public string? Obispo { get; set; }

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

    [Display(Name = "Nombre Completo")]
    public string NombreCompleto => $"{NombreConfirmado} {ApellidoPaterno} {ApellidoMaterno}".Trim();
}
