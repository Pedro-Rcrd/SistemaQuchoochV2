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
    [HttpGet("selectAll")]
      public async Task<IEnumerable<Grado>> SelectAll()
    {
        return await _gradoService.SelectAll();
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
    try{

   
    await _gradoService.Create(grado);

        return Ok(new{status = true, message = "El grado fue creado correctamente."});
    }catch{
        return BadRequest(new{status = false, message = "Ocurrió un error al intentar crear el grado."});
    }
     }

    [HttpPut("update/{id}")]
    public async Task<IActionResult> Update (int id, Grado grado)
    {
        var gradoToUpdate = await _gradoService.GetById(id);

        if(gradoToUpdate is not null)
        {
            await _gradoService.Update(id, grado);
            return Ok(new{status = true, message = "El grado fue catualizado correctamente."});
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
            return Ok(new{status = true, message ="El grado fue eliminado correctamente."});
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