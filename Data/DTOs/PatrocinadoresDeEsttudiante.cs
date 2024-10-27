using System;
using System.Collections.Generic;

namespace sistemaQuchooch.Data.DTOs;

public partial class PatrocinadoresDeEstudiante
{
    public int CodigoEstudiantePatrocinador {get; set;}
    public string? CodigoBecario { get; set; }
    public string? Estudiante { get; set; }
    public string? NombrePatrocinador { get; set; }
    public string? ApellidoPatrocinador { get; set; }
    public string? Pais { get; set; }
}
