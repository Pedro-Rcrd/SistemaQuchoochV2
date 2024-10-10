using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace sistemaQuchooch.Data.QuchoochModels;

public partial class Curso
{
    public int CodigoCurso { get; set; }

    public int? CodigoNivelAcademico { get; set; }

    public string NombreCurso { get; set; } = null!;

    public string? Estatus { get; set; }
    [JsonIgnore]
    public virtual NivelAcademico? CodigoNivelAcademicoNavigation { get; set; }
    [JsonIgnore]
    public virtual ICollection<CursoFichaCalificacion> CursoFichaCalificacions { get; set; } = new List<CursoFichaCalificacion>();
}
