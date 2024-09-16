using Microsoft.AspNetCore.Mvc;
using sistemaQuchooch.Sevices;
using sistemaQuchooch.Data.QuchoochModels;
using Microsoft.AspNetCore.Authorization;


namespace sistemaQuchooch.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class EstablecimientoController : ControllerBase
{
    private readonly EstablecimientoService _establecimientoService;

    //Constructor
    public EstablecimientoController(EstablecimientoService establecimientoService)
    {
        _establecimientoService = establecimientoService;
    }

    //Metodo para obtener la lista de Establecimientoes
     [HttpGet("getall")]
      public async Task<IActionResult> GetAll(int pagina = 1, int elementosPorPagina = 10)
    {
        var establecimientos = await _establecimientoService.GetAll(pagina, elementosPorPagina);

        // Calcula la cantidad total de registros
        var totalRegistros = await _establecimientoService.CantidadTotalRegistros(); // Debes implementar este método en tu servicio

        // Calcula la cantidad total de páginas
        var totalPaginas = (int)Math.Ceiling((double)totalRegistros / elementosPorPagina);

        // Construye un objeto que incluye la lista de países y la información de paginación
        var resultado = new
        {
            Establecimientos = establecimientos,
            PaginaActual = pagina,
            TotalPaginas = totalPaginas,
            TotalRegistros = totalRegistros
        };

        return Ok(resultado);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Establecimiento>> GetById(int id)
    {
        var establecimiento = await _establecimientoService.GetById(id);

        if (establecimiento is null)
        {
            //Es un método para mostrar error explicito
            return EstablecimientoNotFound(id);
        }

        return establecimiento;
    }

    [HttpPost("create")]
    public async Task<IActionResult> Create(Establecimiento establecimiento)
    {
        var newEstablecimiento = await _establecimientoService.Create(establecimiento);

        return Ok(new{status = true, 
                      message = "El establecimiento se creó correctamente"});
    }

    [HttpPut("update/{id}")]
    public async Task<IActionResult> Update(int id, Establecimiento establecimiento)
    {
       
        var establecimientoToUpdate = await _establecimientoService.GetById(id);

        if (establecimientoToUpdate is not null)
        {
            await _establecimientoService.Update(id, establecimiento);
            
            return Ok(new{status = true, 
                      message = "El establecimiento se modificó correctamente"});
        }
        else
        {
            return EstablecimientoNotFound(id);
        }
    }

    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var establecimientoToDelete = await _establecimientoService.GetById(id);

        if (establecimientoToDelete is not null)
        {
            await _establecimientoService.Delete(id);
            return Ok();
        }
        else
        {
            return EstablecimientoNotFound(id);
        }
    }

    //Definir un método para indicar el mensaje del NotFound
    public NotFoundObjectResult EstablecimientoNotFound(int id)
    {
        return NotFound(new { message = $"La establecimiento con el ID {id} no existe" });
    }

}