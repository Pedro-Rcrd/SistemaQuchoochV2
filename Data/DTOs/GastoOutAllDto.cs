using System;
using System.Collections.Generic;

namespace sistemaQuchooch.Data.QuchoochModels;

public partial class GastoOutAllDto
{
    public int CodigoGasto { get; set; }
    public string Estudiante { get; set; }
    public string ApellidoEstudiante { get; set; }
    public string CodigoBecario { get; set; }

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
