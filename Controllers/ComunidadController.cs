using Microsoft.AspNetCore.Mvc;
using sistemaQuchooch.Sevices;
using sistemaQuchooch.Data.QuchoochModels;
using Microsoft.AspNetCore.Authorization;


namespace sistemaQuchooch.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ComunidadController : ControllerBase
{
    private readonly ComunidadService _comunidadService;

    //Constructor
    public ComunidadController(ComunidadService comunidadService)
    {
        _comunidadService = comunidadService;
    }

    //Metodo para obtener la lista de Comunidades
    [HttpGet("getall")]
      public async Task<IActionResult> GetAll(int pagina = 1, int elementosPorPagina = 10)
    {
        var comunidades = await _comunidadService.GetAll(pagina, elementosPorPagina);

        // Calcula la cantidad total de registros
        var totalRegistros = await _comunidadService.CantidadTotalRegistros(); // Debes implementar este método en tu servicio

        // Calcula la cantidad total de páginas
        var totalPaginas = (int)Math.Ceiling((double)totalRegistros / elementosPorPagina);

        // Construye un objeto que incluye la lista de países y la información de paginación
        var resultado = new
        {
            Comunidades = comunidades,
            PaginaActual = pagina,
            TotalPaginas = totalPaginas,
            TotalRegistros = totalRegistros
        };

        return Ok(resultado);
    }
    [HttpGet("{id}")]
    public async Task<ActionResult<Comunidad>> GetById(int id)
    {
        var comunidad = await _comunidadService.GetById(id);

        if (comunidad is null)
        {
            //Es un método para mostrar error explicito
            return ComunidadNotFound(id);
        }

        return comunidad;
    }

    [HttpPost("create")]

    public async Task<IActionResult> Create(Comunidad comunidad)
    {
        var newComunidad = await _comunidadService.Create(comunidad);

          return Ok(new{status = true, 
                      message = "La comunidad se creo correctamente"});
    }

    [HttpPut("update/{id}")]
    public async Task<IActionResult> Update(int id, Comunidad comunidad)
    {
      
        var comunidadToUpdate = await _comunidadService.GetById(id);

        if (comunidadToUpdate is not null)
        {
            await _comunidadService.Update(id, comunidad);
              return Ok(new{status = true, 
                      message = "La comunidad se modificó correctamente"});
        }
        else
        {
            return ComunidadNotFound(id);
        }
    }

    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var comunidadToDelete = await _comunidadService.GetById(id);

        if (comunidadToDelete is not null)
        {
            await _comunidadService.Delete(id);
            return Ok();
        }
        else
        {
            return ComunidadNotFound(id);
        }
    }

    //Definir un método para indicar el mensaje del NotFound
    public NotFoundObjectResult ComunidadNotFound(int id)
    {
        return NotFound(new { message = $"La comunidad con el ID {id} no existe" });
    }

}