using Microsoft.EntityFrameworkCore;
using sistemaQuchooch.Data;
using sistemaQuchooch.Data.QuchoochModels;


namespace sistemaQuchooch.Sevices;

public class PaisService 
{
    private readonly QuchoochContext _context;

    //constructor
    public PaisService(QuchoochContext context)
    {
        _context = context;
    }

    //Metodo para obtener toda la información de Pais
    public async Task<IEnumerable<Pai>> GetAll()
    {
        return await _context.Pais.ToListAsync();
    }

     public async Task<IEnumerable<Pai>> SelectAll()
    {
        return await _context.Pais.ToListAsync();
    }

    //Metodo para obtener la información por ID.
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
            existingPais.Estatus = pais.Estatus;
            await _context.SaveChangesAsync();
        }
    }

    //Metodo para elminar una Pais
    public async Task Delete (int id)
    {
        var paisToDelete = await GetById(id);

        if(paisToDelete is not null)
        {
            paisToDelete.Estatus = "I";
            await _context.SaveChangesAsync();
        }
    }

}