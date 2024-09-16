using System;
using System.Collections.Generic;

namespace sistemaQuchooch.Data.QuchoochModels;

public partial class OrdenCompraDetalle
{
    public int CodigoOrdenCompraDetalle { get; set; }

    public int? CodigoOrdenCompra { get; set; }

    public string? NombreProducto { get; set; }

    public decimal? Cantidad { get; set; }

    public decimal? Precio { get; set; }

    public virtual OrdenCompra? CodigoOrdenCompraNavigation { get; set; }
}
