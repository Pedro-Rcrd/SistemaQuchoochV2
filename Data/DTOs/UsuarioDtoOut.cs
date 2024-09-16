
using sistemaQuchooch.Data.DTOs; 
namespace sistemaQuchooch.Data.DTOs;

public class UsuarioDtoOut
{
    public int CodigoUsuario { get; set; }

    public string NombreRol { get; set; } = null!;

    public int CodigoRol { get; set; }

    public string NombreUsuario { get; set; } = null!;
    public string Email { get; set; }

    public DateTime FechaCreacion { get; set; }
    public string Estatus { get; set; }

    // Nueva propiedad para almacenar la lista de permisos asociados
    public List<PermisoOutDto> Permisos { get; set; } = new List<PermisoOutDto>();
}