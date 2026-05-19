using System.ComponentModel.DataAnnotations;

namespace NotariaParroquial.Models;

public class Persona
{
    public int Id { get; set; }

    [Required, MaxLength(100)]
    [Display(Name = "Nombre")]
    public string Nombre { get; set; } = string.Empty;

    [Required, MaxLength(100)]
    [Display(Name = "Apellido Paterno")]
    public string ApellidoPaterno { get; set; } = string.Empty;

    [MaxLength(100)]
    [Display(Name = "Apellido Materno")]
    public string? ApellidoMaterno { get; set; }

    [Required]
    [Display(Name = "Fecha de Nacimiento")]
    [DataType(DataType.Date)]
    public DateOnly FechaNacimiento { get; set; }

    [MaxLength(200)]
    [Display(Name = "Lugar de Nacimiento")]
    public string? LugarNacimiento { get; set; }

    [MaxLength(200)]
    [Display(Name = "Domicilio")]
    public string? Domicilio { get; set; }

    [Display(Name = "Nombre Completo")]
    public string NombreCompleto => $"{Nombre} {ApellidoPaterno} {ApellidoMaterno}".Trim();
}
