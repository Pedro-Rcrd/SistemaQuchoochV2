using System;
using System.Collections.Generic;

namespace sistemaQuchooch.Data.DTOs;

public partial class CursosFichaEscolarDto
{

    public int? Bloque { get; set; }

    public string? Curso { get; set; }

    public int? Nota { get; set; }
    public decimal? Promedio { get; set; }

   
}
