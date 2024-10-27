using System;
using System.Collections.Generic;

namespace sistemaQuchooch.Data.DTOs;

public class UsuarioDtoInput
{
    public int? CodigoRol { get; set; }

    public string? NombreUsuario { get; set; }

    public string? Email { get; set; }

    public DateTime FechaCreacion { get; set; }
    public string Contrasenia { get; set; }

    public string ContraseniaNueva { get; set; }
    public string? Estatus { get; set; }

}
