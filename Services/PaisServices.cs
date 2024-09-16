using Microsoft.EntityFrameworkCore;
using sistemaQuchooch.Data;
using sistemaQuchooch.Data.QuchoochModels;

public class PaisServices : IPaisService
{
    private readonly QuchoochContext _context;

    public PaisServices(QuchoochContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Pai>> ObtenerPaisPaginadosAsync(int pagina, int elementosPorPagina)
    {
        var paises = await _context.Pais
            .Skip((pagina - 1) * elementosPorPagina)
            .Take(elementosPorPagina)
            .ToListAsync();

        return paises;
    }

    public async Task<int> ObtenerCantidadTotalRegistrosAsync()
    {
        return await _context.Pais.CountAsync();
    }

      public async Task<Pai?> GetById(int id) //Rol? = Indica que devuelve un objeto rol o un null
    {
        return await _context.Pais.FindAsync(id);
    }

    //Método para crear nueva Pai
    public async Task<Pai> Create (Pai newPais)
    {
        _context.Pais.Add(newPais);
        await _context.SaveChangesAsync();

        return newPais;
    }

  //Metodo para actualizar datos de la ocmunidad
    public async Task Update (int id, Pai pais)
    {
        var existingPais = await GetById(id);

        if(existingPais is not null)
        {
            existingPais.Nombre = pais.Nombre;
            await _context.SaveChangesAsync();
        }
    }

      //Metodo para elminar una Pais
    public async Task Delete (int id)
    {
        var paisToDelete = await GetById(id);

        if(paisToDelete is not null)
        {
            _context.Pais.Remove(paisToDelete);
            await _context.SaveChangesAsync();
        }
    }


}
