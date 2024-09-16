using Microsoft.AspNetCore.Mvc;
using sistemaQuchooch.Sevices;
using sistemaQuchooch.Data.QuchoochModels;
using Microsoft.AspNetCore.Authorization;


namespace sistemaQuchooch.Controllers;


[Authorize]
[ApiController]
[Route("api/[controller]")]
public class GradoController : ControllerBase
{
    private readonly GradoService _gradoService;
    
    //Constructor
    public GradoController(GradoService gradoService )
    {
        _gradoService = gradoService;
    }

    //Metodo para obtener la lista de grados
    [HttpGet("getall")]
      public async Task<IEnumerable<Grado>> Get()
    {
        return await _gradoService.GetAll();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Grado>> GetById(int id)
    {
        var grado = await _gradoService.GetById(id);

        if(grado is null)
        {
            //Es un método para mostrar error explicito
            return GradoNotFound(id);
        }

        return grado;
    }

   [HttpPost("create")]
    public async Task <IActionResult> Create(Grado grado)
    {
        var newGrado = await _gradoService.Create(grado);

        return CreatedAtAction(nameof(GetById), new {id = newGrado.CodigoGrado}, newGrado);
    }

    [HttpPut("update/{id}")]
    public async Task<IActionResult> Update (int id, Grado grado)
    {
        if(id != grado.CodigoGrado)
        {
            return BadRequest(new {message = $"El ID {id} de la URL no coincide con el ID {grado.CodigoGrado} del cuerpo de la solicitud"});
        }

        var gradoToUpdate = await _gradoService.GetById(id);

        if(gradoToUpdate is not null)
        {
            await _gradoService.Update(id, grado);
            return NoContent();
        }
        else
        {
            return GradoNotFound(id);
        }
    }

    [HttpDelete("delete/{id}")]
    public async Task <IActionResult> Delete (int id)
    {
        var gradoToDelete = await _gradoService.GetById(id);

        if(gradoToDelete is not null)
        {
            await _gradoService.Delete(id);
            return Ok();
        }
        else
        {
            return GradoNotFound(id);
        }
    }

    //Definir un método para indicar el mensaje del NotFound
    public NotFoundObjectResult GradoNotFound (int id)
    {
        return NotFound(new {message = $"El grado academico con el ID {id} no existe"});
    }
    
}