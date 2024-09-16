using Microsoft.EntityFrameworkCore;
using sistemaQuchooch.Data;
using sistemaQuchooch.Data.DTOs;
using sistemaQuchooch.Data.QuchoochModels;


namespace sistemaQuchooch.Sevices;

public class CompraService
{
    private readonly QuchoochContext _context;

    //constructor
    public CompraService(QuchoochContext context)
    {
        _context = context;
    }

    //Metodo para obtener toda la información de Compras
    public async Task<IEnumerable<CompraOutAllDto>> GetAll(int pagina, int elementosPorPagina, int codigoEstudiante)
    {
        var compras = await _context.OrdenCompras
        .Where(a => codigoEstudiante == 0 || a.CodigoEstudiante == codigoEstudiante)
        .OrderByDescending(a => a.CodigoOrdenCompra)
        .Select(a => new CompraOutAllDto
        {
            CodigoOrdenCompra = a.CodigoOrdenCompra,
            Estudiante = a.CodigoEstudianteNavigation != null ? a.CodigoEstudianteNavigation.NombreEstudiante : "",
            Proveedor = a.CodigoProveedorNavigation != null ? a.CodigoProveedorNavigation.NombreProveedor : "",
            FechaCreacion = a.FechaCreacion,
            Titulo = a.Titulo,
            Estado = a.Estado,
            PersonaRecibe = a.PersonaCreacion,
            Descripcion = a.Descripcion,
            FechaEntrega = a.FechaEntrega,
            Total = a.Total,
            ImgEstudiante = a.ImgEstudiante
        })
            .Skip((pagina - 1) * elementosPorPagina)
            .Take(elementosPorPagina)
            .ToListAsync();
        return compras;
    }

    public async Task<int> CantidadTotalRegistros()
    {
        return await _context.OrdenCompras.CountAsync();
    }

    //Cantidad de registros activos
    public async Task<int> CantidadTotalRegistrosActivos()
    {
        return await _context.OrdenCompras
            .Where(g => g.Estado == "A")
            .CountAsync();
    }

    //SELECT ALL
    public async Task<IEnumerable<CompraOutAllDto>> SelectAll()
    {
        var compras = await _context.OrdenCompras
        .Where(a => a.Estado == "A")
        .OrderByDescending(a => a.CodigoOrdenCompra)
        .Select(a => new CompraOutAllDto
        {
            CodigoOrdenCompra = a.CodigoOrdenCompra,
            CodigoBecario = a.CodigoEstudianteNavigation != null ? a.CodigoEstudianteNavigation.CodigoBecario : "",
            Estudiante = a.CodigoEstudianteNavigation != null ? a.CodigoEstudianteNavigation.NombreEstudiante : "",
            ApellidoEstudiante = a.CodigoEstudianteNavigation != null ? a.CodigoEstudianteNavigation.ApellidoEstudiante : "",
            Proveedor = a.CodigoProveedorNavigation != null ? a.CodigoProveedorNavigation.NombreProveedor : "",
            FechaCreacion = a.FechaCreacion,
            Titulo = a.Titulo,
            Estado = a.Estado,
            PersonaRecibe = a.PersonaCreacion,
            Descripcion = a.Descripcion,
            FechaEntrega = a.FechaEntrega,
            Total = a.Total,
            ImgEstudiante = a.ImgEstudiante
        })
        .ToListAsync();

        return compras;
    }

    public async Task<CompraOutAllDto?> GetByIdDto(int id) //Rol? = Indica que devuelve un objeto rol o un null
    {
        return await _context.OrdenCompras.Where(a => a.CodigoOrdenCompra == id).Select(a => new CompraOutAllDto
        {
            CodigoOrdenCompra = a.CodigoOrdenCompra,
            Estudiante = a.CodigoEstudianteNavigation != null ? a.CodigoEstudianteNavigation.NombreEstudiante : "",
            ApellidoEstudiante = a.CodigoEstudianteNavigation != null ? a.CodigoEstudianteNavigation.ApellidoEstudiante : "",
            CodigoBecario = a.CodigoEstudianteNavigation != null ? a.CodigoEstudianteNavigation.CodigoBecario : "",
            Proveedor = a.CodigoProveedorNavigation != null ? a.CodigoProveedorNavigation.NombreProveedor : "",
            FechaCreacion = a.FechaCreacion,
            Titulo = a.Titulo,
            Estado = a.Estado,
            PersonaRecibe = a.PersonaCreacion,
            Descripcion = a.Descripcion,
            FechaEntrega = a.FechaEntrega,
            Total = a.Total,
            ImgEstudiante = a.ImgEstudiante
        }).SingleOrDefaultAsync();
    }

    //Metodo para obtener la información por ID.
    public async Task<OrdenCompra?> GetById(int id) //Usuario? = Indica que devuelve un objeto usuario o un null
    {
        return await _context.OrdenCompras.FindAsync(id);
    }

    //Metodo para mostrar información cuando se actualiza los datos 
    public async Task<CompraUpdateOutDto?> GetByIdUpdateDto(int id) //Rol? = Indica que devuelve un objeto rol o un null
    {
        return await _context.OrdenCompras.Where(a => a.CodigoOrdenCompra == id).Select(a => new CompraUpdateOutDto
        {
            CodigoOrdenCompra = a.CodigoOrdenCompra,
            CodigoEstudiante = a.CodigoEstudiante,
            NombreEstudiante = a.CodigoEstudianteNavigation != null ? a.CodigoEstudianteNavigation.NombreEstudiante : "",
            CodigoProveedor = a.CodigoProveedor,
            FechaCreacion = a.FechaCreacion,
            Titulo = a.Titulo,
            Estado = a.Estado,
            PersonaCreacion = a.PersonaCreacion,
            Descripcion = a.Descripcion,
            FechaEntrega = a.FechaEntrega,
            Total = a.Total,
            ImgEstudiante = a.ImgEstudiante
        }).SingleOrDefaultAsync();
    }

    #region CREATE ordenCompra
    public async Task<OrdenCompra> Create(OrdenCompra newOrdenCompra)
    {
        _context.OrdenCompras.Add(newOrdenCompra);
        await _context.SaveChangesAsync();

        return newOrdenCompra;
    }
    public async Task<OrdenCompraDetalle> CreateDetalle (OrdenCompraDetalle newOrdenCompraDetalle)
    {
        _context.OrdenCompraDetalles.Add(newOrdenCompraDetalle);
        await _context.SaveChangesAsync();
        return newOrdenCompraDetalle;
    }
    #endregion

    //Metodo para actualizar datos del OrdenCompra
    public async Task Update(int id, OrdenCompra ordenCompra)
    {
        var existingOrdenCompra = await GetById(id);

        if (existingOrdenCompra is not null)
        {
            existingOrdenCompra.CodigoEstudiante = ordenCompra.CodigoEstudiante;
            existingOrdenCompra.CodigoProveedor = ordenCompra.CodigoProveedor;
            existingOrdenCompra.FechaCreacion = ordenCompra.FechaCreacion;
            existingOrdenCompra.Titulo = ordenCompra.Titulo;
            existingOrdenCompra.Estado = ordenCompra.Estado;
            existingOrdenCompra.PersonaCreacion = ordenCompra.PersonaCreacion;
            existingOrdenCompra.Descripcion = ordenCompra.Descripcion;
            existingOrdenCompra.FechaEntrega = ordenCompra.FechaEntrega;
            existingOrdenCompra.Total = ordenCompra.Total;
            if (ordenCompra != null)
            {
                existingOrdenCompra.ImgEstudiante = ordenCompra.ImgEstudiante;
            }

            await _context.SaveChangesAsync();
        }
    }

    //Metodo para elminar un Gasto
    public async Task Delete(int id)
    {
        var compraToDelete = await GetById(id);

        if (compraToDelete is not null)
        {
            _context.OrdenCompras.Remove(compraToDelete);
            await _context.SaveChangesAsync();
        }
    }
}