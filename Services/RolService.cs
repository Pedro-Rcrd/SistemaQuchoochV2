using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sistemaQuchooch.Data;
using sistemaQuchooch.Data.QuchoochModels;


namespace sistemaQuchooch.Sevices;

public class RolService
{
    private readonly QuchoochContext _context;

    //constructor
    public RolService(QuchoochContext context)
    {
        _context = context;
    }

    //Metodo para obtener toda la información de roles
    public async Task<IEnumerable<Rol>> SelectAll()
    {
        return await _context.Rols.ToListAsync();
    }

    public async Task<IEnumerable<Permiso>> PermisosRol(int codigoRol)
    {
        return await _context.Permisos.Where(p => p.CodigoRol == codigoRol)
        .ToListAsync();
    }


    //Permiso de roles
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


    //Metodo para obtener la información por ID.
    public async Task<Rol?> GetById(int id) //Rol? = Indica que devuelve un objeto rol o un null
    {
        return await _context.Rols.FindAsync(id);
    }

    //Método para crear nuevo Rol
    public async Task<Rol> Create(Rol newRol)
    {
        _context.Rols.Add(newRol);
        await _context.SaveChangesAsync();

        return newRol;
    }

    public async Task<Permiso> CreatePermission(Permiso permiso)
    {
        _context.Permisos.Add(permiso);
        await _context.SaveChangesAsync();

        return permiso;
    }

    //Metodo para actualizar datos del Rol
    public async Task Update(int id, Rol rol)
    {
        var existingRol = await GetById(id);

        if (existingRol is not null)
        {
            existingRol.NombreRol = rol.NombreRol;
            existingRol.Estatus = rol.Estatus;
            await _context.SaveChangesAsync();
        }
    }

    //Metodo para elminar un rol
    public async Task Delete(int id)
    {
        var rolToDelete = await GetById(id);

        if (rolToDelete is not null)
        {
            _context.Rols.Remove(rolToDelete);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<List<Permiso>> GetByIdPermisosRol(int codigoRol)
    {
        return await _context.Permisos
            .Where(p => p.CodigoRol == codigoRol)
            .ToListAsync();
    }

    public async Task DeletePermisosRol(int codigoRol)
    {
        var permisosToDelete = await GetByIdPermisosRol(codigoRol);

        // Si hay permisos asociados, proceder a eliminarlos
    if (permisosToDelete.Any())
    {
        _context.Permisos.RemoveRange(permisosToDelete); // Eliminar todos los permisos asociados
        await _context.SaveChangesAsync(); // Guardar los cambios en la base de datos
    }
    }




}