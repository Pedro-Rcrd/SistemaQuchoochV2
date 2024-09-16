using Microsoft.AspNetCore.Mvc;
using sistemaQuchooch.Sevices;
using sistemaQuchooch.Data.QuchoochModels;
using Microsoft.AspNetCore.Authorization;


namespace sistemaQuchooch.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class CursoController : ControllerBase
{
    private readonly CursoService _cursoService;
    
    //Constructor
    public CursoController(CursoService cursoService )
    {
        _cursoService = cursoService;
    }

    //Metodo para obtener la lista de cursoes
      [HttpGet("getall")]
      public async Task<IActionResult> GetAll(int pagina = 1, int elementosPorPagina = 10)
    {
        var cursos = await _cursoService.GetAll(pagina, elementosPorPagina);

        // Calcula la cantidad total de registros
        var totalRegistros = await _cursoService.CantidadTotalRegistros(); // Debes implementar este método en tu servicio

        // Calcula la cantidad total de páginas
        var totalPaginas = (int)Math.Ceiling((double)totalRegistros / elementosPorPagina);

        // Construye un objeto que incluye la lista de países y la información de paginación
        var resultado = new
        {
            Cursos = cursos,
            PaginaActual = pagina,
            TotalPaginas = totalPaginas,
            TotalRegistros = totalRegistros
        };

        return Ok(resultado);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Curso>> GetById(int id)
    {
        var curso = await _cursoService.GetById(id);

        if(curso is null)
        {
            //Es un método para mostrar error explicito
            return CursoNotFound(id);
        }

        return curso;
    }

    [HttpPost("create")]
    public async Task <IActionResult> Create(Curso curso)
    {
        var newCurso = await _cursoService.Create(curso);

        return Ok(new{status = true, 
         message = "El curso fue creado correctamente"});
    }

    [HttpPut("update/{id}")]
    public async Task<IActionResult> Update (int id, Curso curso)
    {
      
        var cursoToUpdate = await _cursoService.GetById(id);

        if(cursoToUpdate is not null)
        {
            await _cursoService.Update(id, curso);
            return Ok(new{status = true, 
                      message = "El curso fue modificado correctamente"});
        }
        else
        {
            return Ok(new{status = true, 
                      message = "El curso fue modificado correctamente"});
        }
    }

    [HttpDelete("delete/{id}")]
    public async Task <IActionResult> Delete (int id)
    {
        var cursoToDelete = await _cursoService.GetById(id);

        if(cursoToDelete is not null)
        {
            await _cursoService.Delete(id);
            return Ok();
        }
        else
        {
            return CursoNotFound(id);
        }
    }

    //Definir un método para indicar el mensaje del NotFound
    public NotFoundObjectResult CursoNotFound (int id)
    {
        return NotFound(new {message = $"El Curso con el ID {id} no existe"});
    }
    
}