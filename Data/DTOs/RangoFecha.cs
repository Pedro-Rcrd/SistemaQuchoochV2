using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace sistemaQuchooch.Data.DTOs;

public partial class RangoFecha
{
    public DateTime FechaInicio { get; set; }
    public DateTime FechaFin { get; set; }
}
