using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace sistemaQuchooch.Data.QuchoochModels;

public partial class CursoFichaCalificacion
{
    public int CodigoCursoFichaCalificacion { get; set; }

    public int? CodigoFichaCalificacionDetalle { get; set; }

    public int? CodigoCurso { get; set; }

    public int? Nota { get; set; }

    public string? Estatus { get; set; }
    [JsonIgnore]
    public virtual Curso? CodigoCursoNavigation { get; set; }
    [JsonIgnore]
    public virtual FichaCalificacionDetalle? CodigoFichaCalificacionDetalleNavigation { get; set; }
}
