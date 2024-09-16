using System;
using System.Collections.Generic;

namespace sistemaQuchooch.Data.QuchoochModels;

public partial class CompraOutAllDto
{
    public int CodigoOrdenCompra { get; set; }
    public string Estudiante { get; set; }
    public string ApellidoEstudiante { get; set; }
    public string CodigoBecario { get; set; }

    public string Proveedor { get; set; }

    public DateTime? FechaCreacion { get; set; }

    public string? Titulo { get; set; }

    public string? Estado { get; set; }

    public string? PersonaRecibe { get; set; }

    public string? Descripcion { get; set; }

    public DateTime? FechaEntrega { get; set; }

    public decimal? Total { get; set; }

    public string? ImgEstudiante { get; set; }
}
