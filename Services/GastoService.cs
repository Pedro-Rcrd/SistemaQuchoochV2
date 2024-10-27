using Microsoft.EntityFrameworkCore;
using sistemaQuchooch.Data;
using sistemaQuchooch.Data.DTOs;
using sistemaQuchooch.Data.QuchoochModels;


namespace sistemaQuchooch.Sevices;

public class GastoService
{
    private readonly QuchoochContext _context;

    //constructor
    public GastoService(QuchoochContext context)
    {
        _context = context;
    }

    //Metodo para obtener toda la información de Gastos
    public async Task<IEnumerable<GastoOutAllDto>> GetAll(int pagina, int elementosPorPagina, int codigoEstudiante)
    {
        var gastos = await _context.Gastos
        .Where(a => codigoEstudiante == 0 || a.CodigoEstudiante == codigoEstudiante)
        .OrderByDescending(a => a.CodigoGasto)
        .Select(a => new GastoOutAllDto
        {
            CodigoGasto = a.CodigoGasto,
            NombreEstudiante = a.CodigoEstudianteNavigation != null ? a.CodigoEstudianteNavigation.NombreEstudiante : "",
            FechaEntrega = a.FechaEntrega,
            Titulo = a.Titulo,
            Estado = a.Estado,
            TipoPago = a.TipoPago,
            NumeroCheque = a.NumeroCheque,
            Monto = a.Monto,
            PersonaRecibe = a.PersonaRecibe,
            Descripcion = a.Descripcion,
            FechaRecibirComprobante = a.FechaRecibirComprobante,
            NumeroComprobante = a.NumeroComprobante,
            ImgCheque = a.ImgCheque,
            ImgComprobante = a.ImgComprobante,
            ImgEstudiante = a.ImgEstudiante
        })
            .Skip((pagina - 1) * elementosPorPagina)
            .Take(elementosPorPagina)
            .ToListAsync();
        return gastos;
    }

    public async Task<int> CantidadTotalRegistros()
    {
        return await _context.Gastos.CountAsync();
    }

    //Cantidad de registros activos
    public async Task<int> CantidadTotalRegistrosActivos ()
    {
        return await _context.Gastos
            .Where(g => g.Estado == "A")
            .CountAsync();
    }

       public async Task<IEnumerable<GastoOutAllDto>> SelectAll()
    {
        var gastos = await _context.Gastos
        .OrderByDescending(a => a.CodigoGasto)
        .Select(a => new GastoOutAllDto
        {
            CodigoGasto = a.CodigoGasto,
            CodigoBecario = a.CodigoEstudianteNavigation != null ? a.CodigoEstudianteNavigation.CodigoBecario : "",
            NombreEstudiante = a.CodigoEstudianteNavigation != null ? a.CodigoEstudianteNavigation.NombreEstudiante : "",
            ApellidoEstudiante = a.CodigoEstudianteNavigation != null ? a.CodigoEstudianteNavigation.ApellidoEstudiante : "",
            FechaEntrega = a.FechaEntrega,
            Titulo = a.Titulo,
            Estado = a.Estado,
            TipoPago = a.TipoPago,
            NumeroCheque = a.NumeroCheque,
            Monto = a.Monto,
            PersonaRecibe = a.PersonaRecibe,
            Descripcion = a.Descripcion,
            FechaRecibirComprobante = a.FechaRecibirComprobante,
            NumeroComprobante = a.NumeroComprobante,
        })
        .ToListAsync();

        return gastos;
    }
       public async Task<IEnumerable<GastoOutAllDto>> GastosPorRangoFecha(RangoFecha model)
    {
        var gastos = await _context.Gastos
        .Where(a => a.FechaEntrega >= model.FechaInicio && a.FechaEntrega <= model.FechaFin)
        .OrderByDescending(a => a.CodigoGasto)
        .Select(a => new GastoOutAllDto
        {
            CodigoGasto = a.CodigoGasto,
            CodigoBecario = a.CodigoEstudianteNavigation != null ? a.CodigoEstudianteNavigation.CodigoBecario : "",
            NombreEstudiante = a.CodigoEstudianteNavigation != null ? a.CodigoEstudianteNavigation.NombreEstudiante : "",
            ApellidoEstudiante = a.CodigoEstudianteNavigation != null ? a.CodigoEstudianteNavigation.ApellidoEstudiante : "",
            FechaEntrega = a.FechaEntrega,
            Titulo = a.Titulo,
            Estado = a.Estado,
            TipoPago = a.TipoPago,
            NumeroCheque = a.NumeroCheque,
            Monto = a.Monto,
            PersonaRecibe = a.PersonaRecibe,
            Descripcion = a.Descripcion,
            FechaRecibirComprobante = a.FechaRecibirComprobante,
            NumeroComprobante = a.NumeroComprobante,
        })
        .ToListAsync();

        return gastos;
    }

         public async Task<IEnumerable<GastoDetalle>> ObtenerListaProductos(int codigoGasto)
    {
        var productos = await _context.GastoDetalles
        .Where(a => a.CodigoGasto == codigoGasto && a.Estatus == "A")
        .ToArrayAsync();
        return productos;

    }

    public async Task<GastoOutAllDto?> GetByIdDto(int id) //Rol? = Indica que devuelve un objeto rol o un null
    {
        return await _context.Gastos.Where(a => a.CodigoGasto == id).Select(a => new GastoOutAllDto
        {
            CodigoGasto = a.CodigoGasto,
            CodigoBecario = a.CodigoEstudianteNavigation != null ? a.CodigoEstudianteNavigation.CodigoBecario : "",
            codigoEstudiante = a.CodigoEstudiante,
            NombreEstudiante = a.CodigoEstudianteNavigation != null ? a.CodigoEstudianteNavigation.NombreEstudiante : "",
            ApellidoEstudiante = a.CodigoEstudianteNavigation != null ? a.CodigoEstudianteNavigation.ApellidoEstudiante : "",
            FechaEntrega = a.FechaEntrega,
            Titulo = a.Titulo,
            Estado = a.Estado,
            TipoPago = a.TipoPago,
            NumeroCheque = a.NumeroCheque,
            Monto = a.Monto,
            PersonaRecibe = a.PersonaRecibe,
            Descripcion = a.Descripcion,
            FechaRecibirComprobante = a.FechaRecibirComprobante,
            NumeroComprobante = a.NumeroComprobante,
            ImgCheque = a.ImgCheque,
            ImgComprobante = a.ImgComprobante,
            ImgEstudiante = a.ImgEstudiante
        }).SingleOrDefaultAsync();
    }

    //Metodo para obtener la información por ID.
    public async Task<Gasto?> GetById(int id) //Usuario? = Indica que devuelve un objeto usuario o un null
    {
        return await _context.Gastos.FindAsync(id);
    }
      public async Task<GastoDetalle?> GetByIdProducto(int id) //Usuario? = Indica que devuelve un objeto usuario o un null
    {
        return await _context.GastoDetalles.FindAsync(id);
    }


    public async Task<GastoUpdateOutDto?> GetByIdUpdateDto(int id) //Rol? = Indica que devuelve un objeto rol o un null
    {
        return await _context.Gastos.Where(a => a.CodigoGasto == id).Select(a => new GastoUpdateOutDto
        {
            CodigoGasto = a.CodigoGasto,
            CodigoEstudiante = a.CodigoEstudiante,
            NombreEstudiante = a.CodigoEstudianteNavigation != null ? a.CodigoEstudianteNavigation.NombreEstudiante : "",
            FechaEntrega = a.FechaEntrega,
            Titulo = a.Titulo,
            Estado = a.Estado,
            TipoPago = a.TipoPago,
            NumeroCheque = a.NumeroCheque,
            Monto = a.Monto,
            PersonaRecibe = a.PersonaRecibe,
            Descripcion = a.Descripcion,
            FechaRecibirComprobante = a.FechaRecibirComprobante,
            NumeroComprobante = a.NumeroComprobante,
            ImgCheque = a.ImgCheque,
            ImgComprobante = a.ImgComprobante,
            ImgEstudiante = a.ImgEstudiante
        }).SingleOrDefaultAsync();
    }


    //Método para crear nuevo gasto
    public async Task<Gasto> Create(Gasto newGasto)
    {
        _context.Gastos.Add(newGasto);
        await _context.SaveChangesAsync();

        return newGasto;
    }

     public async Task<GastoDetalle> CreateProducto(GastoDetalle producto)
    {
        _context.GastoDetalles.Add(producto);
        await _context.SaveChangesAsync();

        return producto;
    }
     public async Task ActualizarProducto(int codigoGastoDetalle, GastoDetalle producto)
    {
            var productoExistente = await _context.GastoDetalles.FindAsync(codigoGastoDetalle);
            if(productoExistente != null){
                productoExistente.NombreProducto = producto.NombreProducto;
                productoExistente.Cantidad = producto.Cantidad;
                productoExistente.Precio = producto.Precio;
            }
            await _context.SaveChangesAsync();
       
    }

    //Metodo para actualizar datos del Gasto
    public async Task Update(int id, Gasto gasto)
    {
        var existingGasto = await GetById(id);

        if (existingGasto is not null)
        {
            existingGasto.CodigoEstudiante = gasto.CodigoEstudiante;
            existingGasto.FechaEntrega = gasto.FechaEntrega;
            existingGasto.Titulo = gasto.Titulo;
            existingGasto.Estado = gasto.Estado;
            existingGasto.TipoPago = gasto.TipoPago;
            existingGasto.NumeroCheque = gasto.NumeroCheque;
            existingGasto.Monto = gasto.Monto;
            existingGasto.PersonaRecibe = gasto.PersonaRecibe;
            existingGasto.Descripcion = gasto.Descripcion;
            existingGasto.FechaRecibirComprobante = gasto.FechaRecibirComprobante;
            existingGasto.NumeroComprobante = gasto.NumeroComprobante;
            if (gasto.ImgCheque != null)
            {
                existingGasto.ImgCheque = gasto.ImgCheque;
            }
            if (gasto.ImgComprobante != null)
            {
                existingGasto.ImgComprobante = gasto.ImgComprobante;
            }
            if (gasto.ImgEstudiante != null)
            {
                existingGasto.ImgEstudiante = gasto.ImgEstudiante;
            }
            await _context.SaveChangesAsync();
        }
    }

    //Metodo para elminar un Gasto
    public async Task Delete(int id)
    {
        var productoToDelete = await GetByIdProducto(id);

        if (productoToDelete is not null)
        {
            productoToDelete.Estatus = "I";
            await _context.SaveChangesAsync();
        }
    }
}