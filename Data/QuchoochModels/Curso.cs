using System;
using System.Collections.Generic;

namespace sistemaQuchooch.Data.QuchoochModels;

public partial class Curso
{
    public int CodigoCurso { get; set; }

    public int? CodigoNivelAcademico { get; set; }

    public string NombreCurso { get; set; } = null!;

    public virtual NivelAcademico? CodigoNivelAcademicoNavigation { get; set; }

    public virtual ICollection<CursoFichaCalificacion> CursoFichaCalificacions { get; set; } = new List<CursoFichaCalificacion>();
}
