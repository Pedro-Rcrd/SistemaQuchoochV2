using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;
using sistemaQuchooch.Sevices;
using Microsoft.AspNetCore.Authorization;
using sistemaQuchooch.Data.QuchoochModels;
[Authorize]
[Route("api/estudiantes")]
[ApiController]
public class EstudiantesController : ControllerBase
{
    private readonly FileUploadService _fileUploadService;
    private readonly EstudianteService _estudianteService;
     private readonly ComunidadService _comunidadService;
     private readonly NivelAcademicoService _nivelAcademicoService;

    public EstudiantesController(FileUploadService fileUploadService,
                                EstudianteService estudianteService,
                                ComunidadService comunidadService,
                                NivelAcademicoService nivelAcademicoService)
    {
        _fileUploadService = fileUploadService;
        _estudianteService = estudianteService;
        _comunidadService = comunidadService;
        _nivelAcademicoService = nivelAcademicoService;
    }
    [HttpPost("create")]
    public async Task<IActionResult> CrearEstudiante([FromForm] EstudianteInputModel model)
    {
        if (model == null || model.ImagenEstudiante == null)
        {
            return BadRequest("Los datos del estudiante son inválidos.");
        }

        var nombreEstudiante = model.NombreEstudiante;
        var telefono = model.TelefonoEstudiante;
        var madre = model.NombreMadre;
        var estado = model.Estado;
        var genero = model.Genero;
        var codigoBecario = model.CodigoBecario;
        Console.WriteLine($"Que viene en codigoBecario: {codigoBecario}");
        Console.WriteLine($"Que viene aquí {nombreEstudiante}");
        Console.WriteLine($"Aquí viene telefono {telefono}");
        Console.WriteLine($"Nombre madre {madre}");
        Console.WriteLine($"Que estado viene {estado}");
        Console.WriteLine($"Que genero viene {genero}");


        var file = Request.Form.Files[0];
        var img = model.ImagenEstudiante.FileName;
        string folder = "Becarios";
        if (img is null)
        {
            return BadRequest("La imagen no se cargó correctamente");
        }
        Console.WriteLine("Esta llegando la imagen");
        var imageUrl = await _fileUploadService.UploadFileAsync(file, folder);
        Console.WriteLine($"Que se esta obteniendo {imageUrl}");

        //Guardar datos del estudiante
        Estudiante estudiante = new Estudiante();

        estudiante.CodigoComunidad = model.CodigoComunidad;
        estudiante.CodigoNivelAcademico = model.CodigoNivelAcademico;
        estudiante.CodigoGrado = model.CodigoGrado;
        estudiante.CodigoCarrera = model.CodigoCarrera;
        estudiante.CodigoEstablecimiento = model.CodigoEstablecimiento;
        estudiante.NombreEstudiante = model.NombreEstudiante;
        estudiante.ApellidoEstudiante = model.ApellidoEstudiante;
        estudiante.FechaNacimieto = model.FechaNacimieto;
        estudiante.Genero = model.Genero;
        estudiante.Estado = model.Estado;
        estudiante.TelefonoEstudiante = model.TelefonoEstudiante;
        estudiante.Email = model.Email;
        estudiante.Sector = model.Sector;
        estudiante.NumeroCasa = model.NumeroCasa;
        estudiante.Descripcion = model.Descripcion;
        estudiante.NombrePadre = model.NombrePadre;
        estudiante.TelefonoPadre = model.TelefonoPadre;
        estudiante.OficioPadre = model.OficioPadre;
        estudiante.NombreMadre = model.NombreMadre;
        estudiante.TelefonoMadre = model.TelefonoMadre;
        estudiante.OficioMadre = model.OficioMadre;
        estudiante.FotoPerfil = imageUrl;
        estudiante.FechaRegistro = model.FechaRegistro;
        estudiante.CodigoBecario = model.CodigoBecario;
        

        await _estudianteService.Create(estudiante);

        return Ok("Estudiante creado exitosamente.");
    }

    //METODO PARA EDITAR
    [HttpPut("updateimage/{id}")]
    public async Task<IActionResult> EditarEstudiante(int id, [FromForm] EstudianteInputModel model)
    {
        if (model.ImagenEstudiante == null)
        {
            Console.WriteLine("Los datos del estudiante son inválidos.");
        }
        Console.WriteLine(id);
        var estudianteToUpdate = await _estudianteService.GetById(id);

        var nombreEstudiante = model.NombreEstudiante;
        var telefono = model.TelefonoEstudiante;
        var madre = model.NombreMadre;
        var estado = model.Estado;
        var genero = model.Genero;
        var codigoBecario = model.CodigoBecario;
        Console.WriteLine($"Que viene en codigoBecario: {codigoBecario}");
        Console.WriteLine($"Que viene aquí {nombreEstudiante}");
        Console.WriteLine($"Aquí viene telefono {telefono}");
        Console.WriteLine($"Nombre madre {madre}");
        Console.WriteLine($"Que estado viene {estado}");
        Console.WriteLine($"Que genero viene {genero}");

        
                var file = Request.Form.Files[0];
                var img = model.ImagenEstudiante.FileName;
                string folder = "Becarios";
                if (img is null)
                {
                    return BadRequest("La imagen no se cargó correctamente");
                }
                Console.WriteLine("Esta llegando la imagen");
                var imageUrl = await _fileUploadService.UploadFileAsync(file, folder);
                Console.WriteLine($"Que se esta obteniendo {imageUrl}");

                //Guardar datos del estudiante
                Estudiante estudiante = new Estudiante();

                 estudiante.CodigoComunidad = model.CodigoComunidad;
                estudiante.CodigoNivelAcademico = model.CodigoNivelAcademico;
                estudiante.CodigoGrado = model.CodigoGrado;
                estudiante.CodigoCarrera = model.CodigoCarrera;
                estudiante.CodigoEstablecimiento = model.CodigoEstablecimiento;
                estudiante.NombreEstudiante = model.NombreEstudiante;
                estudiante.ApellidoEstudiante = model.ApellidoEstudiante;
                estudiante.FechaNacimieto = model.FechaNacimieto;
                estudiante.Genero = model.Genero;
                estudiante.Estado = model.Estado;
                estudiante.TelefonoEstudiante = model.TelefonoEstudiante;
                estudiante.Email = model.Email;
                estudiante.Sector = model.Sector;
                estudiante.NumeroCasa = model.NumeroCasa;
                estudiante.Descripcion = model.Descripcion;
                estudiante.NombrePadre = model.NombrePadre;
                estudiante.TelefonoPadre = model.TelefonoPadre;
                estudiante.OficioPadre = model.OficioPadre;
                estudiante.NombreMadre = model.NombreMadre;
                estudiante.TelefonoMadre = model.TelefonoMadre;
                estudiante.OficioMadre = model.OficioMadre;
                estudiante.FotoPerfil = imageUrl;
                estudiante.FechaRegistro = model.FechaRegistro;
                estudiante.CodigoBecario = model.CodigoBecario; 

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

    [HttpPut("update/{id}")]
    public async Task<IActionResult> EditarEstudianteSinImagen(int id, [FromBody] EstudianteInputModel model)
    {
        // Tu lógica para manejar la edición del estudiante aquí sin la imagen.
        var nombreEstudiante = model.NombreEstudiante;
        Console.WriteLine($"Que viene aquí {nombreEstudiante}");
        // Puedes utilizar este método cuando no necesitas incluir una imagen.

        return Ok("Estudiante editado con éxito (sin imagen)");
    }

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

public class EstudianteInputModel
{
    public string? CodigoBecario { get; set; }
    public int CodigoComunidad { get; set; }
    public int? CodigoNivelAcademico { get; set; }
    public int? CodigoGrado { get; set; }
    public int? CodigoCarrera { get; set; }

    public int? CodigoEstablecimiento { get; set; }
    public string NombreEstudiante { get; set; } = null!;
    public string ApellidoEstudiante { get; set; } = null!;
    public DateTime FechaNacimieto { get; set; }
    public string Genero { get; set; } = null!;
    public string Estado { get; set; } = null!;
    public string? TelefonoEstudiante { get; set; }
    public string? Email { get; set; }

    public byte? Sector { get; set; }

    public string? NumeroCasa { get; set; }

    public string? Descripcion { get; set; }
    public string NombrePadre { get; set; } = null!;

    public string? TelefonoPadre { get; set; }

    public string? OficioPadre { get; set; }

    public string NombreMadre { get; set; } = null!;

    public string? TelefonoMadre { get; set; }

    public string? OficioMadre { get; set; }
    public IFormFile ImagenEstudiante { get; set; }
    public DateTime? FechaRegistro { get; set; }
}
