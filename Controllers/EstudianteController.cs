using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;
using sistemaQuchooch.Sevices;
using Microsoft.AspNetCore.Authorization;
using sistemaQuchooch.Data.QuchoochModels;
using System.Linq.Expressions;
using CloudinaryDotNet.Actions;
using sistemaQuchooch.Data.DTOs;
[Authorize]
[Route("api/[controller]")]
[ApiController]
public class EstudianteController : ControllerBase
{
    private readonly FileUploadService _fileUploadService;
    private readonly EstudianteService _estudianteService;
    private readonly ComunidadService _comunidadService;
    private readonly NivelAcademicoService _nivelAcademicoService;
     private readonly ConvertirImagenBase64Service _convertirImagenBase64Service;

    public EstudianteController(FileUploadService fileUploadService,
                                EstudianteService estudianteService,
                                ComunidadService comunidadService,
                                NivelAcademicoService nivelAcademicoService,
                                ConvertirImagenBase64Service convertirImagenBase64Service)
    {
        _fileUploadService = fileUploadService;
        _estudianteService = estudianteService;
        _comunidadService = comunidadService;
        _nivelAcademicoService = nivelAcademicoService;
        _convertirImagenBase64Service = convertirImagenBase64Service;
        
    }

    [HttpGet("selectAll")]
    public async Task<IEnumerable<EstudianteDto>> SelectAll()
    {
        var estudiantes = await _estudianteService.SelectAll();
        return estudiantes;
    }

    [HttpGet("buscarPorRangoFecha")]
    public async Task<IEnumerable<EstudianteDto>> BuscarPorRangoFecha([FromQuery] DateTime fechaInicio, [FromQuery] DateTime fechaFin)
    {
        var model = new RangoFecha { FechaInicio = fechaInicio, FechaFin = fechaFin };
        var estudiantes = await _estudianteService.EstudiantesPorRangoFecha(model);
        return estudiantes;
    }

    [HttpGet("estudiantesPorAnio")]
    public async Task<IEnumerable<CantidadEstudiantesPorAnioDto>> CantidadaEstudiantesPorAnio()
    {

        var estudiantes = await _estudianteService.ObtenerEstudiantesPorAnio();
        return estudiantes;
    }

    [HttpGet("getbyid/{id}")]
    public async Task<ActionResult<Estudiante>> GetById(int id)
    {
        var estudiante = await _estudianteService.GetById(id);

        if (estudiante is null)
        {
            //Es un método para mostrar error explicito
            return EstudianteNotFound(id);
        }

        return estudiante;
    }

        [HttpGet("Ficha/{codigoEstudiante}")]
    public async Task<ActionResult<EstudianteDto>> Ficha(int codigoEstudiante)
    {
        var fichaEstudiante = await _estudianteService.Ficha(codigoEstudiante);
        var urlFotoPerfil = fichaEstudiante.FotoPerfil;
        string fotoPerfilBase64 = string.Empty;
         if (urlFotoPerfil != null){
            fotoPerfilBase64 = await _convertirImagenBase64Service.ConvertirImagenBase64(urlFotoPerfil);
            if(fotoPerfilBase64 != null){
                fichaEstudiante.FotoPerfil = fotoPerfilBase64;
            }
         }
        if (fichaEstudiante is null)
        {
            //Es un método para mostrar error explicito
            return EstudianteNotFound(codigoEstudiante);
        }

        return fichaEstudiante;
    }

    [HttpPost("create")]
    public async Task<IActionResult> CrearEstudiante([FromForm] EstudianteInputModel model)
    {
        try
        {
            if (model == null || model.ImagenEstudiante == null)
            {
                return BadRequest(new { message = "Los datos del estudiante son inválidos." });
            }

            var file = Request.Form.Files[0];
            var img = model.ImagenEstudiante.FileName;
            string folder = "Becarios";
            if (img is null)
            {
                return BadRequest("La imagen no se cargó correctamente");
            }
            var imageUrl = await _fileUploadService.UploadFileAsync(file, folder);

            //Guardar datos del estudiante
            Estudiante estudiante = new Estudiante
            {
                CodigoComunidad = model.CodigoComunidad,
                CodigoNivelAcademico = model.CodigoNivelAcademico,
                CodigoGrado = model.CodigoGrado,
                CodigoCarrera = model.CodigoCarrera,
                CodigoEstablecimiento = model.CodigoEstablecimiento,
                NombreEstudiante = model.NombreEstudiante,
                ApellidoEstudiante = model.ApellidoEstudiante,
                FechaNacimieto = model.FechaNacimiento,
                Genero = model.Genero,
                Estado = model.Estado,
                TelefonoEstudiante = model.TelefonoEstudiante,
                Email = model.Email,
                Sector = model.Sector,
                NumeroCasa = model.NumeroCasa,
                Descripcion = model.Descripcion,
                NombrePadre = model.NombrePadre,
                TelefonoPadre = model.TelefonoPadre,
                OficioPadre = model.OficioPadre,
                NombreMadre = model.NombreMadre,
                TelefonoMadre = model.TelefonoMadre,
                OficioMadre = model.OficioMadre,
                FotoPerfil = imageUrl,
                FechaRegistro = model.FechaRegistro,
                CodigoBecario = model.CodigoBecario,
                 CodigoModalidadEstudio = model.CodigoModalidadEstudio
            };

            await _estudianteService.Create(estudiante);

            return Ok(new { status = true, message = "El estudiante fue creado correctamente." });

        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = $"Se produjo un error al crear el estudiante: {ex.Message}" });
        }
    }

    //METODO PARA EDITAR
    [HttpPut("update/{id}")]
    public async Task<IActionResult> EditarEstudiante(int id, [FromForm] EstudianteInputModel model)
    {
        string imageUrl = string.Empty;
        string folder = "Becarios";

        var estudianteToUpdate = await _estudianteService.GetById(id);
        if (estudianteToUpdate == null)
        {
            return BadRequest(new { message = $"El estudiante con el codigo {id} no existe." });
        }

        if (model.ImagenEstudiante != null)
        {
            Console.WriteLine("Se cargó la imagen");
            //Capturando archivo img
            var file = Request.Form.Files[0];
            //Capturando el nombre del archivo
            var img = model.ImagenEstudiante.FileName;
            imageUrl = await _fileUploadService.UploadFileAsync(file, folder);
        }

        //Guardar datos del estudiante
        Estudiante estudiante = new Estudiante
        {
            CodigoComunidad = model.CodigoComunidad,
            CodigoNivelAcademico = model.CodigoNivelAcademico,
            CodigoGrado = model.CodigoGrado,
            CodigoCarrera = model.CodigoCarrera,
            CodigoEstablecimiento = model.CodigoEstablecimiento,
            NombreEstudiante = model.NombreEstudiante,
            ApellidoEstudiante = model.ApellidoEstudiante,
            FechaNacimieto = model.FechaNacimiento,
            Genero = model.Genero,
            Estado = model.Estado,
            TelefonoEstudiante = model.TelefonoEstudiante,
            Email = model.Email,
            Sector = model.Sector,
            NumeroCasa = model.NumeroCasa,
            Descripcion = model.Descripcion,
            NombrePadre = model.NombrePadre,
            TelefonoPadre = model.TelefonoPadre,
            OficioPadre = model.OficioPadre,
            NombreMadre = model.NombreMadre,
            TelefonoMadre = model.TelefonoMadre,
            OficioMadre = model.OficioMadre,
            FotoPerfil = imageUrl,
            FechaRegistro = model.FechaRegistro,
            CodigoBecario = model.CodigoBecario,
            CodigoModalidadEstudio = model.CodigoModalidadEstudio
        };

        if (estudianteToUpdate is not null)
        {
            Console.WriteLine("Se encontró el estudiante a actualizar");
            string validationResult = await ValidateStudent(estudiante);
            if (!validationResult.Equals("valid"))
            {
                Console.WriteLine("La validación del estudiante no es valid");
                return BadRequest(new { message = validationResult });
            }
            Console.WriteLine("Proceso de actualizar");
            await _estudianteService.Update(id, estudiante);
            
            return Ok(new
            {
                status = true,
                message = "Estudiante modificado correctamente"
            });
        }
        else
        {
            Console.WriteLine("No se encontró el ID del estudiante ");
            return EstudianteNotFound(id);
        }


    }

    [HttpPut("updateEst/{id}")]
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
     public int? CodigoModalidadEstudio { get; set; }
    public int? CodigoEstablecimiento { get; set; }
    public string NombreEstudiante { get; set; } = null!;
    public string ApellidoEstudiante { get; set; } = null!;
    public DateTime FechaNacimiento { get; set; }
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
    public IFormFile? ImagenEstudiante { get; set; }
    public DateTime? FechaRegistro { get; set; }
}
