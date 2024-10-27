using Microsoft.EntityFrameworkCore;
using sistemaQuchooch.Data;
using sistemaQuchooch.Data.QuchoochModels;


namespace sistemaQuchooch.Sevices;

public class PromedioService 
{
    private readonly QuchoochContext _context;

    //constructor
    public PromedioService(QuchoochContext context)
    {
        _context = context;
    }

       public async Task<IEnumerable<Promedio>> SelectAll()
    {
       // return await _context.Carreras.ToListAsync();
        return  await _context.Promedios.ToListAsync();
    }


    //Metodo para obtener la información por ID.
    public async Task<Promedio?> GetById(int id) //Rol? = Indica que devuelve un objeto rol o un null
    {
        return await _context.Promedios.FindAsync(id);
    }


    //Metodo para actualizar datos de la ocmunidad
    public async Task Update (int id, Promedio model)
    {
        var existingPromedio = await GetById(id);

        if(existingPromedio is not null)
        {
            existingPromedio.ValorMinimo = model.ValorMinimo;
            existingPromedio.ValorMaximo = model.ValorMaximo;
            await _context.SaveChangesAsync();
        }
    }
}