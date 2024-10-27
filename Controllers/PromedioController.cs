using Microsoft.AspNetCore.Mvc;
using sistemaQuchooch.Sevices;
using sistemaQuchooch.Data.QuchoochModels;
using Microsoft.AspNetCore.Authorization;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class PromedioController : ControllerBase
{
    //private readonly IpromedioService _promedioService;
    private readonly PromedioService _promedioService;

    public PromedioController(PromedioService promedioService)
    {
        _promedioService = promedioService;
    }


     [HttpGet("selectall")]
    public async Task<IEnumerable<Promedio>> SelectAll()
    {
        return await _promedioService.SelectAll();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Promedio>> GetById(int id)
    {
        var Promedios = await _promedioService.GetById(id);

        if (Promedios is null)
        {
            //Es un método para mostrar error explicito
            return PromediosNotFound(id);
        }

        return Promedios;
    }


    [HttpPut("update/{codigoPromedio}")]
    public async Task<IActionResult> Update(int codigoPromedio, Promedio promedio)
    {
        var promedioToUpdate = await _promedioService.GetById(codigoPromedio);

        if (promedioToUpdate is not null)
        {
            await _promedioService.Update(codigoPromedio, promedio);
             return Ok(new{status = true, 
                      message = "El Promedio fue actualizado correctamente"});
        }
        else
        {
            return PromediosNotFound(codigoPromedio);
        }
    }


    //Definir un método para indicar el mensaje del NotFound
    public NotFoundObjectResult PromediosNotFound(int id)
    {
        return NotFound(new { message = $"El Promedio con el ID {id} no existe" });
    }


}
