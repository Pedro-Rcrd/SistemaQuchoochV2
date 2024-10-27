using System;
using System.Collections.Generic;

namespace sistemaQuchooch.Data.QuchoochModels;

public partial class CompraDetalle
{
    public int CodigoCompraDetalle { get; set; }

    public int? CodigoOrdenCompra { get; set; }

    public string? NombreProducto { get; set; }

    public int? Cantidad { get; set; }

    public decimal? Precio { get; set; }

    public string? Estatus { get; set; }

    public virtual OrdenCompra? CodigoOrdenCompraNavigation { get; set; }
}
