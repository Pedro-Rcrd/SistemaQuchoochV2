using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace sistemaQuchooch.Data.QuchoochModels;

public partial class EstudiantePatrocinador
{
    public int CodigoEstudiantePatrocinador { get; set; }

    public int CodigoEstudiante { get; set; }

    public int CodigoPatrocinador { get; set; }

    public string? Estatus { get; set; }
    [JsonIgnore]
    public virtual Estudiante CodigoEstudianteNavigation { get; set; } = null!;
    [JsonIgnore]
    public virtual Patrocinador CodigoPatrocinadorNavigation { get; set; } = null!;
}
