using System;
using System.Collections.Generic;

namespace sistemaQuchooch.Data.QuchoochModels;

public partial class CompraInputImgDto
{
    public int CodigoEstudiante { get; set; }

    public int CodigoProveedor { get; set; }

    public DateTime? FechaCreacion { get; set; }

    public string? Titulo { get; set; }

    public string? Estado { get; set; }

    public string? PersonaCreacion { get; set; }

    public string? Descripcion { get; set; }

    public DateTime? FechaEntrega { get; set; }

    public decimal? Total { get; set; }

    public IFormFile? ImgEstudiante { get; set; }

     public List<CompraDetalle>? Productos { get; set; }

}
