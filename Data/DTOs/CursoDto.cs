using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace sistemaQuchooch.Data.DTOs;

public partial class CursoDto
{
    public int CodigoCurso { get; set; }

    public int? CodigoNivelAcademico { get; set; }
    public string? NivelAcademico { get; set; }

    public string NombreCurso { get; set; } = null!;

    public string? Estatus { get; set; }


}