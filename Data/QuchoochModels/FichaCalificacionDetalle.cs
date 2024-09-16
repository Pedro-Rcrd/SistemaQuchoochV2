using System;
using System.Collections.Generic;

namespace sistemaQuchooch.Data.QuchoochModels;

public partial class FichaCalificacionDetalle
{
    public int CodigoFichaCalificacionDetalle { get; set; }

    public int? CodigoFichaCalificacion { get; set; }

    public int? Bloque { get; set; }

    public string? ImgEstudiante { get; set; }

    public string? ImgFichaCalificacion { get; set; }

    public string? ImgCarta { get; set; }

    public decimal? Promedio { get; set; }

    public string? Desempenio { get; set; }

    public virtual FichaCalificacion? CodigoFichaCalificacionNavigation { get; set; }

    public virtual ICollection<CursoFichaCalificacion> CursoFichaCalificacions { get; set; } = new List<CursoFichaCalificacion>();
}
