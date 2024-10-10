using System;
using System.Collections.Generic;

namespace sistemaQuchooch.Data.QuchoochModels;

public partial class Establecimiento
{
    public int CodigoEstablecimiento { get; set; }

    public string NombreEstablecimiento { get; set; } = null!;

    public string? Estatus { get; set; }

    public virtual ICollection<Estudiante> Estudiantes { get; set; } = new List<Estudiante>();

    public virtual ICollection<FichaCalificacion> FichaCalificacions { get; set; } = new List<FichaCalificacion>();
}
