using System;
using System.Collections.Generic;

namespace sistemaQuchooch.Data.QuchoochModels;

public partial class Usuario
{
    public int CodigoUsuario { get; set; }

    public int? CodigoRol { get; set; }

    public string? NombreUsuario { get; set; }

    public DateTime FechaCreacion { get; set; }

    public string Contrasenia { get; set; } = null!;

    public string? Email { get; set; }

    public string? Estatus { get; set; }

    public virtual Rol? CodigoRolNavigation { get; set; }
}
