using Microsoft.EntityFrameworkCore;
using sistemaQuchooch.Data;
using sistemaQuchooch.Data.QuchoochModels;


namespace sistemaQuchooch.Sevices;

public class ComunidadService 
{
    private readonly QuchoochContext _context;

    //constructor
    public ComunidadService(QuchoochContext context)
    {
        _context = context;
    }

    //Metodo para obtener toda la información de comunidades
     public async Task<IEnumerable<Comunidad>> GetAll(int pagina, int elementosPorPagina)
{
    var comunidades = await _context.Comunidads
        .Skip((pagina - 1) * elementosPorPagina)
        .Take(elementosPorPagina)
        .ToListAsync();
    return comunidades;
}

    public async Task<int> CantidadTotalRegistros()
    {
        return await _context.Comunidads.CountAsync();
    }

    //Metodo para obtener la información por ID.
    public async Task<Comunidad?> GetById(int id) //Rol? = Indica que devuelve un objeto rol o un null
    {
        return await _context.Comunidads.FindAsync(id);
    }

    //Método para crear nueva comunidad
    public async Task<Comunidad> Create (Comunidad newComunidad)
    {
        _context.Comunidads.Add(newComunidad);
        await _context.SaveChangesAsync();

        return newComunidad;
    }

    //Metodo para actualizar datos de la ocmunidad
    public async Task Update (int id, Comunidad comunidad)
    {
        var existingComunidad = await GetById(id);

        if(existingComunidad is not null)
        {
            existingComunidad.NombreComunidad = comunidad.NombreComunidad;
            await _context.SaveChangesAsync();
        }
    }

    //Metodo para elminar una comunidad
    public async Task Delete (int id)
    {
        var comunidadToDelete = await GetById(id);

        if(comunidadToDelete is not null)
        {
            _context.Comunidads.Remove(comunidadToDelete);
            await _context.SaveChangesAsync();
        }
    }
}