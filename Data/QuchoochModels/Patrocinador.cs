using System;
using System.Collections.Generic;

namespace sistemaQuchooch.Data.QuchoochModels;

public partial class Patrocinador
{
    public int CodigoPatrocinador { get; set; }

    public int? CodigoPais { get; set; }

    public string NombrePatrocinador { get; set; } = null!;

    public string? ApellidoPatrocinador { get; set; }

    public string? Profesion { get; set; }

    public string? Estado { get; set; }

    public DateTime? FechaNacimiento { get; set; }

    public DateTime? FechaCreacion { get; set; }

    public string? FotoPerfil { get; set; }

    public virtual Pai? CodigoPaisNavigation { get; set; }

    public virtual ICollection<EstudiantePatrocinador> EstudiantePatrocinadors { get; set; } = new List<EstudiantePatrocinador>();
}
