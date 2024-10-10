using System;
using System.Collections.Generic;

namespace sistemaQuchooch.Data.QuchoochModels;

public partial class Carrera
{
    public int CodigoCarrera { get; set; }

    public int? CodigoNivelAcademico { get; set; }

    public string NombreCarrera { get; set; } = null!;

    public string? Estatus { get; set; }

    public virtual NivelAcademico? CodigoNivelAcademicoNavigation { get; set; }

    public virtual ICollection<Estudiante> Estudiantes { get; set; } = new List<Estudiante>();

    public virtual ICollection<FichaCalificacion> FichaCalificacions { get; set; } = new List<FichaCalificacion>();
}
