using Microsoft.AspNetCore.Mvc;
using sistemaQuchooch.Sevices;
using sistemaQuchooch.Data.QuchoochModels;
using Microsoft.AspNetCore.Authorization;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class PaisController : ControllerBase
{
    //private readonly IPaisService _paisService;
    private readonly PaisService _paisService;

    public PaisController(PaisService paisService)
    {
        _paisService = paisService;
    }

     [HttpGet("getall")]
      public async Task<IEnumerable<Pai>> Get()
    {
        return await _paisService.GetAll();
    }


    /*[HttpGet]
    public async Task<IActionResult> ObtenerPaisPaginados(int pagina = 1, int elementosPorPagina = 10)
    {
        var paises = await _paisService.ObtenerPaisPaginadosAsync(pagina, elementosPorPagina);

        // Calcula la cantidad total de registros
        var totalRegistros = await _paisService.ObtenerCantidadTotalRegistrosAsync(); // Debes implementar este método en tu servicio

        // Calcula la cantidad total de páginas
        var totalPaginas = (int)Math.Ceiling((double)totalRegistros / elementosPorPagina);

        // Construye un objeto que incluye la lista de países y la información de paginación
        var resultado = new
        {
            Paises = paises,
            PaginaActual = pagina,
            TotalPaginas = totalPaginas,
            TotalRegistros = totalRegistros
        };

        return Ok(resultado);
    } */

     [HttpGet("selectall")]
    public async Task<IEnumerable<Pai>> SelectAll()
    {
        return await _paisService.SelectAll();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Pai>> GetById(int id)
    {
        var pais = await _paisService.GetById(id);

        if (pais is null)
        {
            //Es un método para mostrar error explicito
            return PaisNotFound(id);
        }

        return pais;
    }

    [HttpPost("create")]
    public async Task<IActionResult> Create(Pai pais)
    {
        try{

        
        var newPais = await _paisService.Create(pais);

        //return CreatedAtAction(nameof(GetById), new {id = newPais.CodigoPais}, newPais);
        return Ok(new
        {
            status = true,
            message = "El Pais se creó correctamente"
        });

        }catch (Exception ex)
         {
            return StatusCode(500, new { status = false, message = "Ocurrió un error al crear el pais", error = ex.Message });
         }
    }

    [HttpPut("update/{id}")]
    public async Task<IActionResult> Update(int id, Pai pais)
    {
        var paisToUpdate = await _paisService.GetById(id);

        if (paisToUpdate is not null)
        {
            await _paisService.Update(id, pais);
             return Ok(new{status = true, 
                      message = "El Pais fue modificado correctamente"});
        }
        else
        {
            return PaisNotFound(id);
        }
    }

    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var paisToDelete = await _paisService.GetById(id);

        if (paisToDelete is not null)
        {
            await _paisService.Delete(id);
           return Ok(new{status = true, 
                      message = "El Pais fue eliminado correctamente"});
        }
        else
        {
            return PaisNotFound(id);
        }
    }

    //Definir un método para indicar el mensaje del NotFound
    public NotFoundObjectResult PaisNotFound(int id)
    {
        return NotFound(new { message = $"El Pais con el ID {id} no existe" });
    }


}
