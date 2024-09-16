namespace sistemaQuchooch.Data.DTOs;

public class UsuarioFichaInformacionDto
{
    public string Nombre { get; set; } = null!;
    public string Rol { get; set; } = null!;
    public List<ModuloFichaDtoOut> Modulos { get; set; } = new List<ModuloFichaDtoOut>();
}

public class ModuloFichaDtoOut
{
    public int Id { get; set; }
    public string NombreModulo { get; set; } = null!;
    public List<string> Permisos { get; set; } = new List<string>();
}
