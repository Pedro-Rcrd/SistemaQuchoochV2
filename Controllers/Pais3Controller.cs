using Microsoft.AspNetCore.Mvc;
using sistemaQuchooch.Sevices;
using sistemaQuchooch.Data.QuchoochModels;
using Microsoft.AspNetCore.Authorization;


namespace sistemaQuchooch.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class Pais3Controller : ControllerBase
{
    private readonly PaisService _paisService;
    
    //Constructor
    public Pais3Controller(PaisService paisService )
    {
        _paisService = paisService;
    }

    //Metodo para obtener la lista de Paises
    [HttpGet("getall")]
      public async Task<IEnumerable<Pai>> Get()
    {
        return await _paisService.GetAll();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Pai>> GetById(int id)
    {
        var pais = await _paisService.GetById(id);

        if(pais is null)
        {
            //Es un método para mostrar error explicito
            return PaisNotFound(id);
        }

        return pais;
    }

    [HttpPost("create")]
    public async Task <IActionResult> Create(Pai pais)
    {
        var newPais = await _paisService.Create(pais);

        return CreatedAtAction(nameof(GetById), new {id = newPais.CodigoPais}, newPais);
    }

    [HttpPut("update/{id}")]
    public async Task<IActionResult> Update (int id, Pai pais)
    {
        if(id != pais.CodigoPais)
        {
            return BadRequest(new {message = $"El ID {id} de la URL no coincide con el ID {pais.CodigoPais} del cuerpo de la solicitud"});
        }

        var paisToUpdate = await _paisService.GetById(id);

        if(paisToUpdate is not null)
        {
            await _paisService.Update(id, pais);
            return NoContent();
        }
        else
        {
            return PaisNotFound(id);
        }
    }

    [HttpDelete("delete/{id}")]
    public async Task <IActionResult> Delete (int id)
    {
        var paisToDelete = await _paisService.GetById(id);

        if(paisToDelete is not null)
        {
            await _paisService.Delete(id);
            return Ok();
        }
        else
        {
            return PaisNotFound(id);
        }
    }

    //Definir un método para indicar el mensaje del NotFound
    public NotFoundObjectResult PaisNotFound (int id)
    {
        return NotFound(new {message = $"El Pais con el ID {id} no existe"});
    }
    
}