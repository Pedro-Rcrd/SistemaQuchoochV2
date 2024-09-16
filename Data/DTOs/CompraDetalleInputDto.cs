using System;
using System.Collections.Generic;

namespace sistemaQuchooch.Data.QuchoochModels;

public partial class CompraDetalleInputDto
{
    public string? NombreProducto { get; set; }

    public decimal? Cantidad { get; set; }

    public decimal? Precio { get; set; }

}
