using System;
using System.Collections.Generic;

namespace sistemaQuchooch.Data.QuchoochModels;

public partial class CursoNotaOutDto
{
    public int? CodigoFichaCalificacionDetalle { get; set; }
    public int CodigoCursoFichaCalificacion { get; set; }
    public int? Bloque { get; set; }
    public string Curso { get; set; }
    public int? CodigoCurso { get; set; }
    public int? Nota { get; set; }

}
