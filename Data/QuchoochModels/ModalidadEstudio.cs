using System;
using System.Collections.Generic;

namespace sistemaQuchooch.Data.QuchoochModels;

public partial class ModalidadEstudio
{
    public int CodigoModalidadEstudio { get; set; }

    public string? NombreModalidadEstudio { get; set; }

    public string? Estatus { get; set; }

    public virtual ICollection<Estudiante> Estudiantes { get; set; } = new List<Estudiante>();

    public virtual ICollection<FichaCalificacion> FichaCalificacions { get; set; } = new List<FichaCalificacion>();
}
