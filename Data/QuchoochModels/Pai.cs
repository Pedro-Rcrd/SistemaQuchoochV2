using System;
using System.Collections.Generic;

namespace sistemaQuchooch.Data.QuchoochModels;

public partial class Pai
{
    public int CodigoPais { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Estatus { get; set; }

    public virtual ICollection<Patrocinador> Patrocinadors { get; set; } = new List<Patrocinador>();
}
