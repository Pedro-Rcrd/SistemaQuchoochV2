using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace sistemaQuchooch.Data.QuchoochModels;

public partial class Promedio
{
    public int CodigoPromedio { get; set; }

    public int? ValorMinimo { get; set; }

    public int? ValorMaximo { get; set; }

    public string? Descripcion { get; set; }
    [JsonIgnore]
    public virtual ICollection<FichaCalificacionDetalle> FichaCalificacionDetalles { get; set; } = new List<FichaCalificacionDetalle>();
}
