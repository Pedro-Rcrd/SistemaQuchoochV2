using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace sistemaQuchooch.Data.QuchoochModels;

public partial class Grado
{
    public int CodigoGrado { get; set; }

    public string NombreGrado { get; set; } = null!;

    public string? Estatus { get; set; }

    [JsonIgnore]
    public virtual ICollection<Estudiante> Estudiantes { get; set; } = new List<Estudiante>();
    [JsonIgnore]
    public virtual ICollection<FichaCalificacion> FichaCalificacions { get; set; } = new List<FichaCalificacion>();
}
