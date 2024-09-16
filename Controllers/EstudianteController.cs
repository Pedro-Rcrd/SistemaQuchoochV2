using Microsoft.AspNetCore.Mvc;
using sistemaQuchooch.Sevices;
using sistemaQuchooch.Data.QuchoochModels;
using sistemaQuchooch.Data.DTOs;
using Microsoft.AspNetCore.Authorization;


namespace sistemaQuchooch.Controllers;


[ApiController]
[Route("api/[controller]")]
public class EstudianteController : ControllerBase
{
    private readonly FileUploadService _fileUploadService;
    private readonly EstudianteService _estudianteService;
    private readonly ComunidadService _comunidadService;
    private readonly NivelAcademicoService _nivelAcademicoService;
    private readonly GradoService _gradoService;
    private readonly CarreraService _carreraService;
    private readonly EstablecimientoService _establecimientoService;


    //Constructor
    public EstudianteController(EstudianteService service,
                                ComunidadService comunidadService,
                                NivelAcademicoService nivelAcademicoService,
                                GradoService gradoService,
                                CarreraService carreraService,
                                EstablecimientoService establecimientoService,
                                FileUploadService fileUploadService)
    {
        _estudianteService = service;
        _comunidadService = comunidadService;
        _nivelAcademicoService = nivelAcademicoService;
        _gradoService = gradoService;
        _carreraService = carreraService;
        _establecimientoService = establecimientoService;
        _fileUploadService = fileUploadService;
    }


    //Metodo para obtener la lista de Estudiantes
    [HttpGet("getall")]
    public async Task<IActionResult> GetAll( int pagina = 1, int elementosPorPagina = 10, int id = 0)
    {
        
        var estudiantes = await _estudianteService.GetAll(pagina, elementosPorPagina, id);

        // Calcula la cantidad total de registros
        var totalRegistros = await _estudianteService.CantidadTotalRegistros(); // Debes implementar este método en tu servicio

        // Calcula la cantidad total de páginas
        var totalPaginas = (int)Math.Ceiling((double)totalRegistros / elementosPorPagina);

        // Construye un objeto que incluye la lista de países y la información de paginación
        var resultado = new
        {
            Estudiantes = estudiantes,
            PaginaActual = pagina,
            TotalPaginas = totalPaginas,
            TotalRegistros = totalRegistros
        };

        return Ok(resultado);
    }

    [HttpGet("selectAll")]
         public async Task<IEnumerable<EstudianteDto>> SelectAll()
    {
        var estudiantes = await _estudianteService.SelectAll();
        return estudiantes;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<EstudianteDto>> GetByIdDto(int id)
    {
        var estudiante = await _estudianteService.GetByIdDto(id);

        if (estudiante is null)
        {
            //Es un método para mostrar error explicito
            return EstudianteNotFound(id);
        }

        return estudiante;
    }
    
     [HttpGet("getbyid/{id}")]
    public async Task<ActionResult<Estudiante>> GetById(int id)
    {
        var estudiante = await _estudianteService.GetById(id);

        if(estudiante is null)
        {
            //Es un método para mostrar error explicito
            return EstudianteNotFound(id);
        }

        return estudiante;
    }

    //Solo el administrador puede crear estudiantes

    [HttpPost("create")]
    public async Task<IActionResult> Create(Estudiante estudiante)
    {
        string validationResult = await ValidateStudent(estudiante);

        if (!validationResult.Equals("valid"))
        {
            return BadRequest(new
            {
                status = false,
                message = validationResult
            });
        }

        await _estudianteService.Create(estudiante);

        return Ok(new
        {
            status = true,
            message = "Estudiante creado correctamente"
        });
        //CreatedAtAction(nameof(GetById), new {id = newEstudiante.CodigoEstudiante}, newEstudiante);
    }

    [HttpPost("createImg")]
    public async Task<IActionResult> CreateImg([FromForm] EstudianteDtoInput estudianteDtoInput, [FromForm] IFormFile imagen)
    {
        // Accede a la imagen con 'imagen' en lugar de 'estudianteDtoInput.imagen'
        Console.WriteLine($"El valor de 'apellidoEstudiante' es: {estudianteDtoInput.ApellidoEstudiante}");
        Console.WriteLine($"El tipo de archivo es: {imagen.ContentType}");

        try
        {
            // Tu lógica de procesamiento aquí
            // Puedes usar 'imagen' para guardar o procesar la imagen como desees

            return Ok(new
            {
                status = true,
                message = "Estudiante creado correctamente",
            });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("upload")]
    public async Task<IActionResult> UploadImage(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("No se proporcionó ningún archivo o el archivo está vacío.");

        // Verificar la extensión o el tipo de archivo si es necesario

        //var uploadPath = Path.Combine("ruta/de/almacenamiento", file.FileName);

        // using (var stream = new FileStream(uploadPath, FileMode.Create))
        //{
        //   await file.CopyToAsync(stream);
        //}

        return Ok("Imagen cargada con éxito.");
    }



    //Solo el administrador puede actualizar Estudiantes
    //[Authorize(Policy = "Administrador")]
    [HttpPut("update/{id}")]
    public async Task<IActionResult> Update(int id, Estudiante estudiante)
    {
        var estudianteToUpdate = await _estudianteService.GetById(id);

        if (estudianteToUpdate is not null)
        {
            string validationResult = await ValidateStudent(estudiante);
            if (!validationResult.Equals("valid"))
            {
                return BadRequest(new { message = validationResult });
            }
            await _estudianteService.Update(id, estudiante);
            return Ok(new
            {
                status = true,
                message = "Estudiante modificado correctamente"
            });
        }
        else
        {
            return EstudianteNotFound(id);
        }
    }


    //Solo el administrador puede eliminar Estudiantes
    //[Authorize(Policy = "Administrador")]
    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var estudianteToDelete = await _estudianteService.GetById(id);

        if (estudianteToDelete is not null)
        {
            await _estudianteService.Delete(id);
            return Ok();
        }
        else
        {
            return EstudianteNotFound(id);
        }
    }

    //Definir un método para indicar el mensaje del NotFound
    public NotFoundObjectResult EstudianteNotFound(int id)
    {
        return NotFound(new { message = $"El Estudiante con el ID {id} no existe" });
    }

    public async Task<string> ValidateStudent(Estudiante estudiante)
    {
        string result = "valid";
        var estudianteComunidad = estudiante.CodigoComunidad.GetValueOrDefault(); //Para validar que el codigoRol no sea Nulo
        var tipoComunidad = await _comunidadService.GetById(estudianteComunidad);
        var estudianteNivelAcademico = estudiante.CodigoNivelAcademico.GetValueOrDefault(); //Para validar que el codigoRol no sea Nulo
        var tipoNivelAcademico = await _nivelAcademicoService.GetById(estudianteNivelAcademico);

        if (tipoComunidad is null)
        {
            result = $"La comunidad {tipoComunidad} no existe";
        }
        if (tipoNivelAcademico is null)
        {
            result = $"El nivel academico {tipoNivelAcademico} no existe";
        }
        return result;
    }
}