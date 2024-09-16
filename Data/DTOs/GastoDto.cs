using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace sistemaQuchooch.Data.QuchoochModels;

public partial class GastoDto
{
    public int CodigoGasto { get; set; }

    public int CodigoEstudiante { get; set; }

    public DateTime? FechaEntrega { get; set; }

    public string? Titulo { get; set; }

    public string? Estado { get; set; }

    public string? TipoPago { get; set; }

    public int? NumeroCheque { get; set; }

    public decimal? Monto { get; set; }

    public string? PersonaRecibe { get; set; }

    public string? Descripcion { get; set; }

    public DateTime? FechaRecibirComprobante { get; set; }

    public string? NumeroComprobante { get; set; }

    public string? ImgCheque { get; set; }

    public string? ImgComprobante { get; set; }

    public string? ImgEstudiante { get; set; }
}
