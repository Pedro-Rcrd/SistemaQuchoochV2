using Microsoft.EntityFrameworkCore;
using sistemaQuchooch.Data;
using sistemaQuchooch.Data.QuchoochModels;


namespace sistemaQuchooch.Sevices;

public class ProveedorService 
{
    private readonly QuchoochContext _context;

    //constructor
    public ProveedorService(QuchoochContext context)
    {
        _context = context;
    }

    //Metodo para obtener toda la información de Proveedor
         public async Task<IEnumerable<Proveedor>> GetAll(int pagina, int elementosPorPagina)
{
    var proveedores = await _context.Proveedors
        .Skip((pagina - 1) * elementosPorPagina)
        .Take(elementosPorPagina)
        .ToListAsync();
    return proveedores;
}

    public async Task<int> CantidadTotalRegistros()
    {
        return await _context.Proveedors.CountAsync();
    }
     public async Task<IEnumerable<Proveedor>> SelectAll()
    {
        return await _context.Proveedors.ToListAsync();
    }

    //Metodo para obtener la información por ID.
    public async Task<Proveedor?> GetById(int id) //Rol? = Indica que devuelve un objeto rol o un null
    {
        return await _context.Proveedors.FindAsync(id);
    }

    //Método para crear nueva Proveedor
    public async Task<Proveedor> Create (Proveedor newProveedor)
    {
        _context.Proveedors.Add(newProveedor);
        await _context.SaveChangesAsync();
        return newProveedor;
    }

    //Metodo para actualizar datos de la ocmunidad
    public async Task Update (int id, Proveedor proveedor)
    {
        var existingProveedor = await GetById(id);

        if(existingProveedor is not null)
        {
            existingProveedor.NombreProveedor = proveedor.NombreProveedor;
            existingProveedor.NombreEncargado = proveedor.NombreEncargado;
            existingProveedor.Telefono = proveedor.Telefono;
            existingProveedor.Descripcion = proveedor.Descripcion;
            existingProveedor.Estatus =  proveedor.Estatus;
            await _context.SaveChangesAsync();
        }
    }

    //Metodo para elminar una Proveedor
    public async Task Delete (int id)
    {
        var proveedorToDelete = await GetById(id);

        if(proveedorToDelete is not null)
        {
            proveedorToDelete.Estatus = "I";
            await _context.SaveChangesAsync();
        }
    }

}