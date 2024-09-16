
using System;
using System.Collections.Generic;

namespace sistemaQuchooch.Data.DTOs;

public class RolPermisoDto
{
    public string NombreRol { get; set; } = null!;
     public string Estatus { get; set; }


    public List<PermisoDto> Permisos { get; set; } = new List<PermisoDto>();
}

public class PermisoDto
{
    public int CodigoModulo { get; set; }
    public int CodigoAccion { get; set; }
}
