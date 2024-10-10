using Microsoft.AspNetCore.Mvc;
using sistemaQuchooch.Sevices;
using sistemaQuchooch.Data.QuchoochModels;
using Microsoft.AspNetCore.Authorization;
using sistemaQuchooch.Data.DTOs;


namespace sistemaQuchooch.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class NivelAcademicoController : ControllerBase
{
    private readonly NivelAcademicoService _nivelAcademicoService;
    
    //Constructor
    public NivelAcademicoController(NivelAcademicoService nivelAcademicoService )
    {
        _nivelAcademicoService = nivelAcademicoService;
    }

    //Metodo para obtener la lista de NivelAcademicoes
    [HttpGet("getall")]
      public async Task<IActionResult> GetAll(int pagina = 1, int elementosPorPagina = 10)
    {
        var nivelesAcademicos = await _nivelAcademicoService.GetAll(pagina, elementosPorPagina);

        // Calcula la cantidad total de registros
        var totalRegistros = await _nivelAcademicoService.CantidadTotalRegistros(); // Debes implementar este método en tu servicio

        // Calcula la cantidad total de páginas
        var totalPaginas = (int)Math.Ceiling((double)totalRegistros / elementosPorPagina);

        // Construye un objeto que incluye la lista de países y la información de paginación
        var resultado = new
        {
            NivelesAcademicos = nivelesAcademicos,
            PaginaActual = pagina,
            TotalPaginas = totalPaginas,
            TotalRegistros = totalRegistros
        };

        return Ok(resultado);
    }

      //Metodo para obtener la lista de niveles academicos
    [HttpGet("selectall")]
    public async Task<IEnumerable<NivelAcademico>> Get()
    {
        return await _nivelAcademicoService.SelectAll();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<NivelAcademico>> GetById(int id)
    {
        var nivelAcademico = await _nivelAcademicoService.GetById(id);

        if(nivelAcademico is null)
        {
            //Es un método para mostrar error explicito
            return NivelAcademicoNotFound(id);
        }

        return nivelAcademico;
    }

    [HttpPost("create")]
    public async Task <IActionResult> Create(NivelAcademico nivelAcademico)
    {
        try{

        var newNivelAcademico = await _nivelAcademicoService.Create(nivelAcademico);

        return Ok(new{status = true, 
                      message = "El Nivel academico se creó correctamente"});
        }catch (Exception ex)
         {
            return StatusCode(500, new { status = false, message = "Ocurrió un error al crear el nivel academico", error = ex.Message });
         }
    }

    [HttpPut("update/{id}")]
    public async Task<IActionResult> Update (int id, NivelAcademicoDto nivelAcademicoDto)
    {
        var nivelAcademicoToUpdate = await _nivelAcademicoService.GetById(id);

        if(nivelAcademicoToUpdate is not null)
        {
            await _nivelAcademicoService.Update(id, nivelAcademicoDto);
            return Ok(new{status = true, 
                      message = "El nivel academico fue modificado correctamente"});
        }
        else
        {
            return NivelAcademicoNotFound(id);
        }
    }

    [HttpDelete("delete/{id}")]
    public async Task <IActionResult> Delete (int id)
    {
        var nivelAcademicoToDelete = await _nivelAcademicoService.GetById(id);

        if(nivelAcademicoToDelete is not null)
        {
            await _nivelAcademicoService.Delete(id);
            return Ok(new{status = true, 
                      message = "El nivel academico fue eliminado correctamente"});
        }
        else
        {
            return NivelAcademicoNotFound(id);
        }
    }

    //Definir un método para indicar el mensaje del NotFound
    public NotFoundObjectResult NivelAcademicoNotFound (int id)
    {
        return NotFound(new {message = $"El NivelAcademico con el ID {id} no existe"});
    }
    
}