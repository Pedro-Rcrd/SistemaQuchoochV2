using Microsoft.EntityFrameworkCore;
using sistemaQuchooch.Data;
using sistemaQuchooch.Data.QuchoochModels;


namespace sistemaQuchooch.Sevices;

public class EstablecimientoService 
{
    private readonly QuchoochContext _context;

    //constructor
    public EstablecimientoService(QuchoochContext context)
    {
        _context = context;
    }

    //Metodo para obtener toda la información de Establecimiento
     public async Task<IEnumerable<Establecimiento>> GetAll(int pagina, int elementosPorPagina)
{
    var establecimientos = await _context.Establecimientos
        .Skip((pagina - 1) * elementosPorPagina)
        .Take(elementosPorPagina)
        .ToListAsync();
    return establecimientos;
}

    public async Task<int> CantidadTotalRegistros()
    {
        return await _context.Establecimientos.CountAsync();
    }
    //Metodo para obtener la información por ID.
    public async Task<Establecimiento?> GetById(int id) //Rol? = Indica que devuelve un objeto rol o un null
    {
        return await _context.Establecimientos.FindAsync(id);
    }

    //Método para crear nueva Establecimiento
    public async Task<Establecimiento> Create (Establecimiento newEstablecimiento)
    {
        _context.Establecimientos.Add(newEstablecimiento);
        await _context.SaveChangesAsync();

        return newEstablecimiento;
    }

    //Metodo para actualizar datos de la ocmunidad
    public async Task Update (int id, Establecimiento establecimiento)
    {
        var existingEstablecimiento = await GetById(id);

        if(existingEstablecimiento is not null)
        {
            existingEstablecimiento.NombreEstablecimiento = establecimiento.NombreEstablecimiento;
            await _context.SaveChangesAsync();
        }
    }

    //Metodo para elminar una Establecimiento
    public async Task Delete (int id)
    {
        var establecimientoToDelete = await GetById(id);

        if(establecimientoToDelete is not null)
        {
            _context.Establecimientos.Remove(establecimientoToDelete);
            await _context.SaveChangesAsync();
        }
    }

}