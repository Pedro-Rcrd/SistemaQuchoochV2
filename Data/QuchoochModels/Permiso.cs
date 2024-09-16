using System;
using System.Collections.Generic;

namespace sistemaQuchooch.Data.QuchoochModels;

public partial class Permiso
{
    public int CodigoPermiso { get; set; }

    public int CodigoRol { get; set; }

    public int CodigoModulo { get; set; }

    public int CodigoAccion { get; set; }

    public virtual Accion CodigoAccionNavigation { get; set; } = null!;

    public virtual Modulo CodigoModuloNavigation { get; set; } = null!;

    public virtual Rol CodigoRolNavigation { get; set; } = null!;
}
