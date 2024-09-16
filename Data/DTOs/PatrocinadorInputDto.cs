using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace sistemaQuchooch.Data.QuchoochModels;

public partial class PatrocinadorInputDto
{
    public int? CodigoPais { get; set; }

    public string NombrePatrocinador { get; set; } = null!;

    public string? ApellidoPatrocinador { get; set; }

    public string? Profesion { get; set; }

    public string? Estado { get; set; }

    public DateTime? FechaNacimiento { get; set; }

    public DateTime? FechaCreacion { get; set; }

    public IFormFile? ImgPatrocinador { get; set; }

}
