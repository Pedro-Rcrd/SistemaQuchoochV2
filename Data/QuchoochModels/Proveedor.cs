using System;
using System.Collections.Generic;

namespace sistemaQuchooch.Data.QuchoochModels;

public partial class Proveedor
{
    public int CodigoProveedor { get; set; }

    public string NombreProveedor { get; set; } = null!;

    public string? NombreEncargado { get; set; }

    public string? Telefono { get; set; }

    public string Descripcion { get; set; } = null!;

    public virtual ICollection<OrdenCompra> OrdenCompras { get; set; } = new List<OrdenCompra>();
}
