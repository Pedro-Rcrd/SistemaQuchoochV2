using System;
using System.Collections.Generic;

namespace sistemaQuchooch.Data.QuchoochModels;

public partial class OrdenCompra
{
    public int CodigoOrdenCompra { get; set; }

    public int CodigoEstudiante { get; set; }

    public int CodigoProveedor { get; set; }

    public DateTime? FechaCreacion { get; set; }

    public string? Titulo { get; set; }

    public string? Estado { get; set; }

    public string? PersonaCreacion { get; set; }

    public string? Descripcion { get; set; }

    public DateTime? FechaEntrega { get; set; }

    public decimal? Total { get; set; }

    public string? ImgEstudiante { get; set; }

    public virtual Estudiante CodigoEstudianteNavigation { get; set; } = null!;

    public virtual Proveedor CodigoProveedorNavigation { get; set; } = null!;

    public virtual ICollection<CompraDetalle> CompraDetalles { get; set; } = new List<CompraDetalle>();

    public virtual ICollection<OrdenCompraDetalle> OrdenCompraDetalles { get; set; } = new List<OrdenCompraDetalle>();
}
