using System;
using System.Collections.Generic;

namespace sistemaQuchooch.Data.QuchoochModels;

public partial class GastoDetalle
{
    public int CodigoGastoDetalle { get; set; }

    public int? CodigoGasto { get; set; }

    public string? NombreProducto { get; set; }

    public decimal? Cantidad { get; set; }

    public decimal? Precio { get; set; }

    public string? Estatus { get; set; }

    public virtual Gasto? CodigoGastoNavigation { get; set; }
}
