using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;
using sistemaQuchooch.Sevices;
using Microsoft.AspNetCore.Authorization;
using sistemaQuchooch.Data.QuchoochModels;
using sistemaQuchooch.Data.DTOs;
[Authorize]
[Route("api/[controller]")]
[ApiController]
public class PatrocinadorController : ControllerBase
{
    private readonly FileUploadService _fileUploadService;
    private readonly PatrocinadorService _patrocinadorService;
    private readonly ComunidadService _comunidadService;
    private readonly NivelAcademicoService _nivelAcademicoService;

    private readonly ConvertirImagenBase64Service _convertirImagenBase64Service;

    public PatrocinadorController(FileUploadService fileUploadService,
                                PatrocinadorService patrocinadorService,
                                ComunidadService comunidadService,
                                NivelAcademicoService nivelAcademicoService,
                                ConvertirImagenBase64Service convertirImagenBase64Service)
    {
        _fileUploadService = fileUploadService;
        _patrocinadorService = patrocinadorService;
        _comunidadService = comunidadService;
        _nivelAcademicoService = nivelAcademicoService;
        _convertirImagenBase64Service = convertirImagenBase64Service;
    }

    //Metodo para obtener la lista de Patrocinadores
    [HttpGet("getall")]
    public async Task<IActionResult> GetAll(int pagina = 1, int elementosPorPagina = 10, int id = 0)
    {
        var patrocinadores = await _patrocinadorService.GetAll(pagina, elementosPorPagina, id);

        // Calcula la cantidad total de registros
        var totalRegistros = await _patrocinadorService.CantidadTotalRegistros(); // Debes implementar este método en tu servicio

        // Calcula la cantidad total de páginas
        var totalPaginas = (int)Math.Ceiling((double)totalRegistros / elementosPorPagina);

        // Construye un objeto que incluye la lista de países y la información de paginación
        var resultado = new
        {
            Patrocinadores = patrocinadores,
            PaginaActual = pagina,
            TotalPaginas = totalPaginas,
            TotalRegistros = totalRegistros
        };

        return Ok(resultado);
    }
    [HttpGet("selectAll")]
    public async Task<IEnumerable<PatrocinadorOutAllDto>> SelectAll()
    {
        var patrocinadores = await _patrocinadorService.SelectAll();
        return patrocinadores;
    }

     [HttpGet("buscarPorRangoFecha")]
    public async Task<IEnumerable<PatrocinadorOutAllDto>> BuscarPorRangoFecha([FromQuery] DateTime fechaInicio, [FromQuery] DateTime fechaFin)
    {
        var model = new RangoFecha { FechaInicio = fechaInicio, FechaFin = fechaFin };
        var patrocinadores = await _patrocinadorService.PatrocinadoresPorRangoFecha(model);
        return patrocinadores;
    }


    [HttpGet("{codigoPatrocinador}")]
    public async Task<ActionResult<Patrocinador>> GetById(int codigoPatrocinador)
    {
        var patrocinador = await _patrocinadorService.GetById(codigoPatrocinador);

        if (patrocinador is null)
        {
            //Es un método para mostrar error explicito
            return PatrocinadorNotFound(codigoPatrocinador);
        }

        return patrocinador;
    }

    //Información visible del patrocinador
    [HttpGet("ficha/{codigoPatrocinador}")]
    //Información para visualizar datos
    public async Task<ActionResult<PatrocinadorOutAllDto>> GetByIdDto(int codigoPatrocinador)
    {
        try
        {
            string fotoPerfilBase64 = string.Empty;
            var patrocinador = await _patrocinadorService.GetByIdDto(codigoPatrocinador);
            if (patrocinador is null)
            {
                //Es un método para mostrar error explicito
                return PatrocinadorNotFound(codigoPatrocinador);
            }

            string urlFotoPerfil = patrocinador.FotoPerfil;
            if (urlFotoPerfil != null)
            {
                fotoPerfilBase64 = await _convertirImagenBase64Service.ConvertirImagenBase64(urlFotoPerfil);
                if (fotoPerfilBase64 != null)
                {
                    patrocinador.FotoPerfil = fotoPerfilBase64;
                }
            }

            return patrocinador;
        }
        catch
        {
            return BadRequest(new { status = false, message = "Hubo un error al intentar obtener la información de la ficha del pratrocinador" });
        }
    }


    [HttpPost("create")]
    public async Task<IActionResult> CrearPatrocinador([FromForm] PatrocinadorInputDto model)
    {
        try
        {
            if (model == null)
            {
                return BadRequest("Los datos del Patrocinador son inválidos.");
            }

            //var img = model.ImgPatrocinador.FileName;
            var img = model.ImgPatrocinador;
            string folder = "Patrocinadores";
            string imageUrl = "sin imagenes";

            if (img != null)
            {
                var file = Request.Form.Files[0];
                imageUrl = await _fileUploadService.UploadFileAsync(file, folder);
            }

            Patrocinador patrocinador = new Patrocinador()
            {
                CodigoPais = model.CodigoPais,
                NombrePatrocinador = model.NombrePatrocinador,
                ApellidoPatrocinador = model.ApellidoPatrocinador,
                Profesion = model.Profesion,
                Estado = model.Estado,
                FechaNacimiento = model.FechaNacimiento,
                FechaCreacion = model.FechaCreacion,
                FotoPerfil = imageUrl
            };
            await _patrocinadorService.Create(patrocinador);

            return Ok(new { status = true, message = "El patrocinador se ha creado correctamente." });

        }
        catch
        {
            return BadRequest();
        }

    }

    //METODO PARA EDITAR
    [HttpPut("update/{codigoPatrocinador}")]
    public async Task<IActionResult> EditarPatrocinador(int codigoPatrocinador, [FromForm] PatrocinadorInputDto model)
    {
        try
        {
            if (model == null)
            {
                return BadRequest("Los datos del Patrocinador son inválidos.");
            }

            //var img = model.ImgPatrocinador.FileName;
            var img = model.ImgPatrocinador;
            string folder = "Patrocinadores";
            string imageUrl = null;

            if (img != null)
            {
                var file = Request.Form.Files[0];
                imageUrl = await _fileUploadService.UploadFileAsync(file, folder);
            }

            Patrocinador patrocinador = new Patrocinador()
            {
                CodigoPais = model.CodigoPais,
                NombrePatrocinador = model.NombrePatrocinador,
                ApellidoPatrocinador = model.ApellidoPatrocinador,
                Profesion = model.Profesion,
                Estado = model.Estado,
                FechaNacimiento = model.FechaNacimiento,
                FechaCreacion = model.FechaCreacion,
                FotoPerfil = imageUrl
            };

            var patrocinadorToUpdate = await _patrocinadorService.GetById(codigoPatrocinador);
            if (patrocinadorToUpdate is not null)
            {
                await _patrocinadorService.Update(codigoPatrocinador, patrocinador);
                return Ok(new
                {
                    status = true,
                    message = "Patrocinador modificado correctamente"
                });

            }
            else
            {
                return PatrocinadorNotFound(codigoPatrocinador);
            }

        }
        catch
        {
            return BadRequest();
        }

    }

    //Método para actualizar estado
    //METODO PARA EDITAR
    [HttpPut("delete/{id}")]
    public async Task<IActionResult> Delete (int codigoPatrocinador)
    {
        try
        {
            await _patrocinadorService.Delete(codigoPatrocinador);
            return Ok(new
            {
                status = true,
                message = "El Patrocinador ha sido eliminado correctamente"
            });

        }
        catch
        {
            return BadRequest(new{status = false, message ="Hubo un error al intentar eliminar el patrocinador."});
        }
    }


    public NotFoundObjectResult PatrocinadorNotFound(int id)
    {
        return NotFound(new { message = $"El Patrocinador con el ID {id} no existe" });
    }


    /*   public async Task<string> ValidateStudent(Patrocinador patrocinador)
      {
          string result = "valid";
          var patrocinadorComunidad = patrocinador.CodigoComunidad.GetValueOrDefault(); //Para validar que el codigoRol no sea Nulo
          var tipoComunidad = await _comunidadService.GetById(patrocinadorComunidad);
          var patrocinadorNivelAcademico = patrocinador.CodigoNivelAcademico.GetValueOrDefault(); //Para validar que el codigoRol no sea Nulo
          var tipoNivelAcademico = await _nivelAcademicoService.GetById(patrocinadorNivelAcademico);

          if (tipoComunidad is null)
          {
              result = $"La comunidad {tipoComunidad} no existe";
          }
          if (tipoNivelAcademico is null)
          {
              result = $"El nivel academico {tipoNivelAcademico} no existe";
          }
          return result;
      } */
}
