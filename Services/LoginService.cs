using Microsoft.EntityFrameworkCore;
using sistemaQuchooch.Data;
using sistemaQuchooch.Data.DTOs;
using sistemaQuchooch.Data.QuchoochModels;


namespace sistemaQuchooch.Sevices;

public class LoginService
{
    private readonly QuchoochContext _context;
    //constructor
    public LoginService(QuchoochContext context)
    {
        _context = context;
    }

    //Metodo para obtener toda la información de usuarios
    public async Task<Usuario?> GetAdmin(AdminDto admin)
    {
        return await _context.Usuarios.
        SingleOrDefaultAsync(x => x.Email == admin.Email && x.Contrasenia == admin.Contrasenia && x.Estatus == "A");
    }
    public async Task<Dictionary<int, int[]>> GetInfoRol(int rol)
    {
        var permisos = await _context.Permisos
            .Where(x => x.CodigoRol == rol)
            .GroupBy(x => x.CodigoModulo)
            .ToDictionaryAsync(
                g => g.Key,
                g => g.Select(x => x.CodigoAccion).ToArray()
            );

        return permisos;
    }

}