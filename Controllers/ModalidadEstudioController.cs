using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;
using sistemaQuchooch.Sevices;
using Microsoft.AspNetCore.Authorization;
using sistemaQuchooch.Data.QuchoochModels;
using System.Linq.Expressions;
using CloudinaryDotNet.Actions;
[Authorize]
[Route("api/[controller]")]
[ApiController]
public class ModalidadEstudioController : ControllerBase
{
    private readonly ModalidadEstudioService _modalidadEstudioService;

    public ModalidadEstudioController(ModalidadEstudioService modalidadEstudioService)
    {
        _modalidadEstudioService = modalidadEstudioService;
    }

    [HttpGet("selectAll")]
    public async Task<IEnumerable<ModalidadEstudio>> SelectAll()
    {
        var modalidadEstudio = await _modalidadEstudioService.SelectAll();
        return modalidadEstudio;
    }

    [HttpGet("GetById/{id}")]
    public async Task<ActionResult<ModalidadEstudio>> GetById(int id)
    {
        var modalidadEstudio = await _modalidadEstudioService.GetById(id);

        if (modalidadEstudio is null)
        {
            //Es un método para mostrar error explicito
            return ModalidadEstudioNotFound(id);
        }
        return modalidadEstudio;
    }

    [HttpPost("create")]
    public async Task<IActionResult> CrearModalidadEstudio(ModalidadEstudio modalidadEstudio)
    {
        try
        {
            if (modalidadEstudio == null)
            {
                return BadRequest(new { message = "Los datos del modalidad de estudio son inválidos." });
            }

            //Guardar datos del ModalidadEstudio

            await _modalidadEstudioService.Create(modalidadEstudio);

            return Ok(new { status = true, message = "La modalidad de estudio fue creado correctamente." });

        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = $"Se produjo un error al crear el modalidad de estudio: {ex.Message}" });
        }
    }

    //METODO PARA EDITAR
    [HttpPut("update/{id}")]
    public async Task<IActionResult> EditarModalidadEstudio(int id, ModalidadEstudio modalidad)
    {
        try
        {
            var modalidadEstudioToUpdate = await _modalidadEstudioService.GetById(id);
            if (modalidadEstudioToUpdate == null)
            {
                return BadRequest(new { message = $"La modalidad de estudio con el codigo {id} no existe." });
            }
            await _modalidadEstudioService.Update(id, modalidad);

            return Ok(new
            {
                status = true,
                message = "La modalidad de estudio fue modificado correctamente"
            });
        }catch{
          return  BadRequest(new{message = "Ocurrió un error al editar la modalidad de estudio"});
        }
    }

    
    [HttpDelete("delete/{id}")]
    public async Task <IActionResult> Delete (int id)
    {
        var modalidadEstudioToDelete = await _modalidadEstudioService.GetById(id);

        if(modalidadEstudioToDelete is not null)
        {
            await _modalidadEstudioService.Delete(id);
            return Ok(new{status = true, 
                      message = "La modalidad de estudio fue eliminado correctamente"});
        }
        else
        {
            return ModalidadEstudioNotFound(id);
        }
    }

    public NotFoundObjectResult ModalidadEstudioNotFound(int id)
    {
        return NotFound(new { message = $"El modalidad de estudio con el ID {id} no existe" });
    }

}
