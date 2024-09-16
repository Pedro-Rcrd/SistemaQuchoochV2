using Microsoft.AspNetCore.Mvc;
using sistemaQuchooch.Sevices;
using sistemaQuchooch.Data.QuchoochModels;
using Microsoft.AspNetCore.Authorization;
using sistemaQuchooch.Data.DTOs;


namespace sistemaQuchooch.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class CarreraController : ControllerBase
{
    private readonly CarreraService _carreraService;
    
    //Constructor
    public CarreraController(CarreraService carreraService )
    {
        _carreraService = carreraService;
    }

    //Metodo para obtener la lista de Carreraes
    [HttpGet("getall")]
      public async Task<IActionResult> GetAll(int pagina = 1, int elementosPorPagina = 10)
    {
        var carreras = await _carreraService.GetAll(pagina, elementosPorPagina);

        // Calcula la cantidad total de registros
        var totalRegistros = await _carreraService.CantidadTotalRegistros(); // Debes implementar este método en tu servicio

        // Calcula la cantidad total de páginas
        var totalPaginas = (int)Math.Ceiling((double)totalRegistros / elementosPorPagina);

        // Construye un objeto que incluye la lista de países y la información de paginación
        var resultado = new
        {
            Carreras = carreras,
            PaginaActual = pagina,
            TotalPaginas = totalPaginas,
            TotalRegistros = totalRegistros
        };

        return Ok(resultado);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CarreraDto>> GetByIdDto(int id)
    {
        var carrera = await _carreraService.GetByIdDto(id);

        if(carrera is null)
        {
            //Es un método para mostrar error explicito
            return CarreraNotFound(id);
        }

        return carrera;
    }

    [HttpPost("create")]
    public async Task <IActionResult> Create(Carrera carrera)
    {
        var newCarrera = await _carreraService.Create(carrera);

        return Ok(new{status = true, 
                      message = "La carrera se creo correctamente"});
    }

    [HttpPut("update/{id}")]
    public async Task<IActionResult> Update (int id, Carrera carrera)
    {
        var carreraToUpdate = await _carreraService.GetById(id);

        if(carreraToUpdate is not null)
        {
            await _carreraService.Update(id, carrera);
            return Ok(new{status = true, 
                      message = "La carrera fue modificado correctamente"});
        }
        else
        {
            return CarreraNotFound(id);
        }
    }

    [HttpDelete("delete/{id}")]
    public async Task <IActionResult> Delete (int id)
    {
        var carreraToDelete = await _carreraService.GetById(id);

        if(carreraToDelete is not null)
        {
            await _carreraService.Delete(id);
            return Ok();
        }
        else
        {
            return CarreraNotFound(id);
        }
    }

    //Definir un método para indicar el mensaje del NotFound
    public NotFoundObjectResult CarreraNotFound (int id)
    {
        return NotFound(new {message = $"El Carrera con el ID {id} no existe"});
    }
    
}