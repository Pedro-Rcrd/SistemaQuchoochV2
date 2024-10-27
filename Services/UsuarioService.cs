using Microsoft.EntityFrameworkCore;
using sistemaQuchooch.Data;
using sistemaQuchooch.Data.DTOs;
using sistemaQuchooch.Data.QuchoochModels;


namespace sistemaQuchooch.Sevices;

public class UsuarioService
{
    private readonly QuchoochContext _context;

    //constructor
    public UsuarioService(QuchoochContext context)
    {
        _context = context;
    }

    //Metodo para obtener toda la información de usuarios
    public async Task<IEnumerable<UsuarioDtoOut>> GetAll(int pagina, int elementosPorPagina)
    {
        var usuarios = await _context.Usuarios.Select(a => new UsuarioDtoOut
        {
            CodigoUsuario = a.CodigoUsuario,
            NombreRol = a.CodigoRolNavigation != null ? a.CodigoRolNavigation.NombreRol : "",
            CodigoRol = a.CodigoRolNavigation.CodigoRol,
            NombreUsuario = a.NombreUsuario != null ? a.NombreUsuario : "",
            Email = a.Email != null ? a.Email : "",
            FechaCreacion = a.FechaCreacion
        })
            .Skip((pagina - 1) * elementosPorPagina)
            .Take(elementosPorPagina)
            .ToListAsync();
        return usuarios;
    }

    public async Task<int> CantidadTotalRegistros()
    {
        return await _context.Usuarios.CountAsync();
    }

public async Task<IEnumerable<UsuarioDtoOut>> SelectAll()
    {
        return await _context.Usuarios.Select(a => new UsuarioDtoOut
        {
            CodigoUsuario = a.CodigoUsuario,
            NombreRol = a.CodigoRolNavigation != null ? a.CodigoRolNavigation.NombreRol : "",
            CodigoRol = a.CodigoRolNavigation.CodigoRol,
            Email = a.Email,
            NombreUsuario = a.NombreUsuario != null ? a.NombreUsuario : "",
            FechaCreacion = a.FechaCreacion,
            Estatus = a.Estatus
        }).ToListAsync();
    }

//Ficha de Informacion del usuario
public async Task<UsuarioFichaInformacionDto> FichaUsuario(int codigoUsuario)
{
    var usuarioData = await _context.Usuarios
        .Where(u => u.CodigoUsuario == codigoUsuario)
        .Include(u => u.CodigoRolNavigation) // Incluye la tabla de Rol
            .ThenInclude(r => r.Permisos)     // Incluye la tabla de Permisos a través del Rol
                .ThenInclude(p => p.CodigoModuloNavigation) // Incluye la tabla de Modulo a través de Permiso
        .Include(u => u.CodigoRolNavigation)
            .ThenInclude(r => r.Permisos)
                .ThenInclude(p => p.CodigoAccionNavigation) // Incluye la tabla de Accion a través de Permiso
        .Select(u => new UsuarioFichaInformacionDto
        {
            Nombre = u.NombreUsuario ?? "", // Nombre del usuario
            Rol = u.CodigoRolNavigation.NombreRol, // Nombre del Rol
            Modulos = u.CodigoRolNavigation.Permisos
                        .GroupBy(p => p.CodigoModuloNavigation) // Agrupa los permisos por módulo
                        .Select(g => new ModuloFichaDtoOut
                        {
                            Id = g.Key.CodigoModulo,
                            NombreModulo = g.Key.NombreModulo,
                            Permisos = g.Select(p => p.CodigoAccionNavigation.NombreAccion).Distinct().ToList()
                        }).ToList()
        }).FirstOrDefaultAsync();

    return usuarioData;
}



    public async Task<UsuarioDtoOut?> GetDtoById(int id)
    {
        return await _context.Usuarios.Where(a => a.CodigoUsuario == id).Select(a => new UsuarioDtoOut
        {
            CodigoUsuario = a.CodigoUsuario,
            NombreRol = a.CodigoRolNavigation != null ? a.CodigoRolNavigation.NombreRol : "",
            NombreUsuario = a.NombreUsuario != null ? a.NombreUsuario : "",
            FechaCreacion = a.FechaCreacion
        }).SingleOrDefaultAsync();
    }

    //Metodo para obtener la información por ID.
    public async Task<Usuario?> GetById(int id) //Usuario? = Indica que devuelve un objeto usuario o un null
    {
        return await _context.Usuarios.FindAsync(id);
    }

    //Método para crear nuevo Usuario
    public async Task<Usuario> Create(Usuario newUsuario)
    {
        _context.Usuarios.Add(newUsuario);
        await _context.SaveChangesAsync();

        return newUsuario;
    }

    //Metodo para actualizar datos del Usuario
    public async Task Update(int id, UsuarioDtoInput usuario)
    {
        var existingUsuario = await GetById(id);

        if (existingUsuario is not null)
        {
            existingUsuario.CodigoRol = usuario.CodigoRol;
            existingUsuario.NombreUsuario = usuario.NombreUsuario;
            existingUsuario.FechaCreacion = usuario.FechaCreacion;
            existingUsuario.Email = usuario.Email;
            existingUsuario.Contrasenia = !string.IsNullOrWhiteSpace(usuario.ContraseniaNueva) ? usuario.ContraseniaNueva : existingUsuario.Contrasenia;
            existingUsuario.Estatus = usuario.Estatus;
            await _context.SaveChangesAsync();
        }
    }

    //Metodo para elminar un rol
    public async Task Delete(int id)
    {
        var usuarioToDelete = await GetById(id);

        if (usuarioToDelete is not null)
        {
            usuarioToDelete.Estatus = "I";
            await _context.SaveChangesAsync();
        }
    }
}