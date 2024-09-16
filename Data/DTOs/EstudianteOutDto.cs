using System;
using System.Collections.Generic;

namespace sistemaQuchooch.Data.QuchoochModels;

public partial class EstudianteOutDto
{
    public int CodigoEstudiante { get; set; }
    public string NombreEstudiante { get; set; } = null!;

    public string ApellidoEstudiante { get; set; } = null!;
    public string? CodigoBecario { get; set; }

}
