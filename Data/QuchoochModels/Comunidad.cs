using System;
using System.Collections.Generic;

namespace sistemaQuchooch.Data.QuchoochModels;

public partial class Comunidad
{
    public int CodigoComunidad { get; set; }

    public string NombreComunidad { get; set; } = null!;

    public string? Estatus { get; set; }

    public virtual ICollection<Estudiante> Estudiantes { get; set; } = new List<Estudiante>();
}
