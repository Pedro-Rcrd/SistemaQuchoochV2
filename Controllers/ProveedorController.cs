using Microsoft.AspNetCore.Mvc;
using sistemaQuchooch.Sevices;
using sistemaQuchooch.Data.QuchoochModels;
using Microsoft.AspNetCore.Authorization;


namespace sistemaQuchooch.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ProveedorController : ControllerBase
{
    private readonly ProveedorService _proveedorService;
    
    //Constructor
    public ProveedorController(ProveedorService proveedorService )
    {
        _proveedorService = proveedorService;
    }

    //Metodo para obtener la lista de proveedores
   [HttpGet("getall")]
      public async Task<IActionResult> GetAll(int pagina = 1, int elementosPorPagina = 10)
    {
        var proveedores = await _proveedorService.GetAll(pagina, elementosPorPagina);

        // Calcula la cantidad total de registros
        var totalRegistros = await _proveedorService.CantidadTotalRegistros(); // Debes implementar este método en tu servicio

        // Calcula la cantidad total de páginas
        var totalPaginas = (int)Math.Ceiling((double)totalRegistros / elementosPorPagina);

        // Construye un objeto que incluye la lista de países y la información de paginación
        var resultado = new
        {
            Proveedores = proveedores,
            PaginaActual = pagina,
            TotalPaginas = totalPaginas,
            TotalRegistros = totalRegistros
        };

        return Ok(resultado);
    }

     [HttpGet("selectall")]
    public async Task<IEnumerable<Proveedor>> Get()
    {
        return await _proveedorService.SelectAll();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Proveedor>> GetById(int id)
    {
        var proveedor = await _proveedorService.GetById(id);

        if(proveedor is null)
        {
            //Es un método para mostrar error explicito
            return ProveedorNotFound(id);
        }

        return proveedor;
    }

    [HttpPost("create")]
    public async Task <IActionResult> Create(Proveedor proveedor)
    {
        try{
        var newProveedor = await _proveedorService.Create(proveedor);

        return Ok(new{status = true, 
                      message = "El proveedor se creó correctamente"});
        }catch (Exception ex)
         {
            return StatusCode(500, new { status = false, message = "Ocurrió un error al crear el proveedor", error = ex.Message });
         }
    }

    [HttpPut("update/{id}")]
    public async Task<IActionResult> Update (int id, Proveedor proveedor)
    {
        
        var proveedorToUpdate = await _proveedorService.GetById(id);

        if(proveedorToUpdate is not null)
        {
            await _proveedorService.Update(id, proveedor);
           return Ok(new{status = true, 
                      message = "El proveedor fue modificado correctamente"});
        }
        else
        {
            return ProveedorNotFound(id);
        }
    }

    [HttpDelete("delete/{id}")]
    public async Task <IActionResult> Delete (int id)
    {
        var proveedorToDelete = await _proveedorService.GetById(id);

        if(proveedorToDelete is not null)
        {
            await _proveedorService.Delete(id);
           return Ok(new{status = true, 
                      message = "El proveedor fue eliminado correctamente"});
        }
        else
        {
            return ProveedorNotFound(id);
        }
    }

    //Definir un método para indicar el mensaje del NotFound
    public NotFoundObjectResult ProveedorNotFound (int id)
    {
        return NotFound(new {message = $"El Proveedor con el ID {id} no existe"});
    }
    
}