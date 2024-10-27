using Microsoft.AspNetCore.Mvc;
using sistemaQuchooch.Sevices;
using sistemaQuchooch.Data.QuchoochModels;
using sistemaQuchooch.Data.DTOs;
using Microsoft.AspNetCore.Authorization;
using System.IO;




namespace sistemaQuchooch.Controllers;


[Authorize]
[ApiController]
[Route("api/[controller]")]
public class EstudiantePatrocinadorController : ControllerBase
{
    private readonly FileUploadService _fileUploadService;
    private readonly EstudiantePatrocinadorService _estudiantePatrocinadorService;



    //Constructor
    public EstudiantePatrocinadorController(EstudiantePatrocinadorService estudiantePatrocinadorService,
                                FileUploadService fileUploadService)
    {
        _estudiantePatrocinadorService = estudiantePatrocinadorService;
        _fileUploadService = fileUploadService;
    }


    //Metodo para obtener la lista de Estudiantes
    [HttpGet("getall/{id}")]
    public async Task<IActionResult> GetAll(int id, int pagina = 1, int elementosPorPagina = 10)
    {
        var estudiantePatrocinadores = await _estudiantePatrocinadorService.GetAll(id, pagina, elementosPorPagina);

        // Calcula la cantidad total de registros
        var totalRegistros = await _estudiantePatrocinadorService.CantidadTotalRegistros(); // Debes implementar este método en tu servicio

        // Calcula la cantidad total de páginas
        var totalPaginas = (int)Math.Ceiling((double)totalRegistros / elementosPorPagina);

        // Construye un objeto que incluye la lista de países y la información de paginación
        var resultado = new
        {
            EstudiantePatrocinadores = estudiantePatrocinadores,
            PaginaActual = pagina,
            TotalPaginas = totalPaginas,
            TotalRegistros = totalRegistros
        };

        return Ok(resultado);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<EstudiantePatrocinadorOutAllDto>> GetByIdDto(int id)
    {
        var estudiantePatrocinador = await _estudiantePatrocinadorService.GetByIdDto(id);

        if (estudiantePatrocinador is null)
        {
            //Es un método para mostrar error explicito
            return EstudiantePatrocinadorNotFound(id);
        }

        return estudiantePatrocinador;
    }

    [HttpGet("getbyid/{id}")]
    public async Task<ActionResult<EstudiantePatrocinador>> GetById(int id)
    {
        var estudiantePatrocinador = await _estudiantePatrocinadorService.GetById(id);

        if (estudiantePatrocinador is null)
        {
            //Es un método para mostrar error explicito
            return EstudiantePatrocinadorNotFound(id);
        }

        return estudiantePatrocinador;
    }



    //Solo el administrador puede crear estudiantePatrocinadors
[HttpPost("create")]
public async Task<IActionResult> Create(List<EstudiantePatrocinadorInputDto> model)
{
    var estudiantesPatrocinadores = model.Select(asignacion => new EstudiantePatrocinador
    {
        CodigoEstudiante = asignacion.CodigoEstudiante,
        CodigoPatrocinador = asignacion.CodigoPatrocinador,
        Estatus = "A"
    }).ToList();

    // Inserta todos los registros en una sola operación
    await _estudiantePatrocinadorService.CreateMultiple(estudiantesPatrocinadores);

    return Ok(new
    {
        status = true,
        message = "Los patrocinadores fueron asignados correctamente."
    });
}


    [HttpGet("selectAll")]
    public async Task<IEnumerable<PatrocinadorOutAllDto>> SelectAll()
    {
        var patrocinadores = await _estudiantePatrocinadorService.SelectAll();
        return patrocinadores;
    }

    [HttpGet("listaEstudiantes")]
    public async Task<IEnumerable<ListaEstudiantePatrocinador>> ObtenerListaEstudiantes()
    {
        var estudiantes = await _estudiantePatrocinadorService.ObtenerListaEstudiantes();
        return estudiantes;
    }
    [HttpGet("listaPatrocinadores/{codigoEstudiante}")]
    public async Task<IEnumerable<PatrocinadoresDeEstudiante>> ObtenerListaPatrocinadores(int codigoEstudiante)
    {
        var patrocinadores = await _estudiantePatrocinadorService.ObtenerListaPatrocinadores(codigoEstudiante);
        return patrocinadores;
    }




    //Solo el administrador puede actualizar Estudiantes
    //[Authorize(Policy = "Administrador")]
    [HttpPut("update/{id}")]
    public async Task<IActionResult> Update(int id, EstudiantePatrocinador estudiantePatrocinador)
    {
        var estPatrocinadorToUpdate = await _estudiantePatrocinadorService.GetById(id);

        if (estPatrocinadorToUpdate is not null)
        {

            await _estudiantePatrocinadorService.Update(id, estPatrocinadorToUpdate);
            return Ok(new
            {
                status = true,
                message = "Estudiante y Patrocinador modificado correctamente"
            });
        }
        else
        {
            return EstudiantePatrocinadorNotFound(id);
        }
    }


    //Solo el administrador puede eliminar Estudiantes
    //[Authorize(Policy = "Administrador")]
    [HttpDelete("delete/{codigoEstudiantePatrocinador}")]
    public async Task<IActionResult> Delete(int codigoEstudiantePatrocinador)
    {
        var estudiantePatrocinadorToDelete = await _estudiantePatrocinadorService.GetById(codigoEstudiantePatrocinador);

        if (estudiantePatrocinadorToDelete is not null)
        {
            await _estudiantePatrocinadorService.Delete(codigoEstudiantePatrocinador);
            return Ok(new{status = true, message = "El patrocinador fue eliminado correctamente."});
        }
        else
        {
            return EstudiantePatrocinadorNotFound(codigoEstudiantePatrocinador);
        }
    }

    //Definir un método para indicar el mensaje del NotFound
    public NotFoundObjectResult EstudiantePatrocinadorNotFound(int id)
    {
        return NotFound(new { message = $"El Estudiante con el ID {id} no existe" });
    }

}