using Microsoft.EntityFrameworkCore;
using sistemaQuchooch.Data;
using sistemaQuchooch.Data.QuchoochModels;


namespace sistemaQuchooch.Sevices;

public class GradoService 
{
    private readonly QuchoochContext _context;

    //constructor
    public GradoService(QuchoochContext context)
    {
        _context = context;
    }

    //Metodo para obtener toda la información de Grados
    public async Task<IEnumerable<Grado>> SelectAll()
    {
        return await _context.Grados.ToListAsync();
    }

    //Metodo para obtener la información por ID.
    public async Task<Grado?> GetById(int id) //Rol? = Indica que devuelve un objeto rol o un null
    {
        return await _context.Grados.FindAsync(id);
    }

    //Método para crear nueva Grado
    public async Task<Grado> Create (Grado newGrado)
    {
        _context.Grados.Add(newGrado);
        await _context.SaveChangesAsync();

        return newGrado;
    }

    //Metodo para actualizar datos de grado
    public async Task Update (int id, Grado grado)
    {
        var existingGrado = await GetById(id);

        if(existingGrado is not null)
        {
            existingGrado.NombreGrado = grado.NombreGrado;
            await _context.SaveChangesAsync();
        }
    }

    //Metodo para elminar una Grado
    public async Task Delete (int id)
    {
        var gradoToDelete = await GetById(id);

        if(gradoToDelete is not null)
        {
            _context.Grados.Remove(gradoToDelete);
            await _context.SaveChangesAsync();
        }
    }
}