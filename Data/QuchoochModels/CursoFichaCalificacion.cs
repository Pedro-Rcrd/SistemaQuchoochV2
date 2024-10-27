using System;
using System.Collections.Generic;

namespace sistemaQuchooch.Data.QuchoochModels;

public partial class CursoFichaCalificacion
{
    public int CodigoCursoFichaCalificacion { get; set; }

    public int? CodigoFichaCalificacionDetalle { get; set; }

    public int? CodigoCurso { get; set; }

    public int? Nota { get; set; }

    public string? Estatus { get; set; }

    public virtual Curso? CodigoCursoNavigation { get; set; }

    public virtual FichaCalificacionDetalle? CodigoFichaCalificacionDetalleNavigation { get; set; }
}
