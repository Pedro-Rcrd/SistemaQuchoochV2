using Microsoft.EntityFrameworkCore;
using sistemaQuchooch.Data;
using sistemaQuchooch.Data.QuchoochModels;


namespace sistemaQuchooch.Sevices;

public class FichaCalificacionDetalleService
{
    private readonly QuchoochContext _context;

    //constructor
    public FichaCalificacionDetalleService(QuchoochContext context)
    {
        _context = context;
    }

    //Metodo para obtener toda la información de FichaCalificacionDetallees
    public async Task<IEnumerable<FichaCalificacionDetalle>> GetAll()
    {
        return await _context.FichaCalificacionDetalles.ToListAsync();
    }

    //Para gráfica
    public async Task<IEnumerable<FichaCalificacionDetalle>> GetByCodigoFichaCalificacion(int codigoFichaCalificacion)
    {
        return await _context.FichaCalificacionDetalles
            .Where(fcd => fcd.CodigoFichaCalificacion == codigoFichaCalificacion)
            .Select(fcd => new FichaCalificacionDetalle
            {
                Bloque = fcd.Bloque,
                Promedio = fcd.Promedio
            })
            .ToListAsync();
    }


    //¿Cúantos bloques tiene la ficha?
    public async Task<IEnumerable<int?>> GetCuantosBloquesTieneFicha(int codigoFichaCalificacion)
    {
        return await _context.FichaCalificacionDetalles
            .Where(f => f.CodigoFichaCalificacion == codigoFichaCalificacion)
            .Select(f => f.Bloque)
            .ToListAsync();
    }

    public async Task<IEnumerable<int>> GetBloquePorFicha(int codigoFichaCalificacion, int bloqueSeleccionado)
    {
        return await _context.FichaCalificacionDetalles
            .Where(detalle => detalle.CodigoFichaCalificacion == codigoFichaCalificacion
            && detalle.Bloque == bloqueSeleccionado)
            .Select(detalle => detalle.CodigoFichaCalificacionDetalle)
            .ToListAsync();
    }


    //Metodo para obtener la información por ID.
    public async Task<FichaCalificacionDetalle?> GetById(int id) //Rol? = Indica que devuelve un objeto rol o un null
    {
        return await _context.FichaCalificacionDetalles.FindAsync(id);
    }

    //Obtener bloque cuando el codigo del la ficha sea igual a ID y el bloque sea igual a IdBloque
    public async Task<FichaCalificacionDetalle?> GetByIdBloque(int codigoFichaCalificacion, int bloque)
    {
        return await _context.FichaCalificacionDetalles
            .Where(detalle => detalle.CodigoFichaCalificacion == codigoFichaCalificacion && detalle.Bloque == bloque)
            .FirstOrDefaultAsync();
    }

    //Obtener codigo de ficha
    public async Task<int?> CodigoFichaDetalle(int codigoFichaCalificacion, int bloque)
    {
        var codigoFichaCalificacionDetalle = await _context.FichaCalificacionDetalles
            .Where(f => f.CodigoFichaCalificacion == codigoFichaCalificacion && f.Bloque == bloque)
            .Select(f => f.CodigoFichaCalificacionDetalle)
            .FirstOrDefaultAsync();

        return codigoFichaCalificacionDetalle;
    }



    //OBTNER EL VALOR MAXIMO DEL CODIGO
    public async Task<int> GetMaxCodigoFichaCalificacionDetalle()
    {
        var maxCodigo = await _context.FichaCalificacionDetalles.MaxAsync(f => f.CodigoFichaCalificacionDetalle);
        return maxCodigo;
    }

    //Método para crear nueva FichaCalificacionDetalle
    public async Task<FichaCalificacionDetalle> Create(FichaCalificacionDetalle newFichaCalificacionDetalle)
    {
        _context.FichaCalificacionDetalles.Add(newFichaCalificacionDetalle);
        await _context.SaveChangesAsync();

        return newFichaCalificacionDetalle;
    }

    //Metodo para actualizar datos de la ocmunidad
    public async Task Update(int id, FichaCalificacionDetalle fichaCalificacionDetalle)
    {
        var existingFichaCalificacionDetalle = await GetById(id);
        Console.WriteLine("Si existe");

        if (existingFichaCalificacionDetalle is not null)
        {
            existingFichaCalificacionDetalle.CodigoFichaCalificacion = fichaCalificacionDetalle.CodigoFichaCalificacion;
            existingFichaCalificacionDetalle.Bloque = fichaCalificacionDetalle.Bloque;
            existingFichaCalificacionDetalle.ImgEstudiante = fichaCalificacionDetalle.ImgEstudiante;
            existingFichaCalificacionDetalle.ImgFichaCalificacion = fichaCalificacionDetalle.ImgFichaCalificacion;
            existingFichaCalificacionDetalle.ImgCarta = fichaCalificacionDetalle.ImgCarta;
            await _context.SaveChangesAsync();
        }
    }

    //Metodo para elminar una FichaCalificacionDetalle
    public async Task Delete(int id)
    {
        var fichaCalificacionDetalleToDelete = await GetById(id);

        if (fichaCalificacionDetalleToDelete is not null)
        {
            _context.FichaCalificacionDetalles.Remove(fichaCalificacionDetalleToDelete);
            await _context.SaveChangesAsync();
        }
    }
}