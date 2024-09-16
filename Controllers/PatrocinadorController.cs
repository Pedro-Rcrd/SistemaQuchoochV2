using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;
using sistemaQuchooch.Sevices;
using Microsoft.AspNetCore.Authorization;
using sistemaQuchooch.Data.QuchoochModels;
[Authorize]
[Route("api/[controller]")]
[ApiController]
public class PatrocinadorController : ControllerBase
{
    private readonly FileUploadService _fileUploadService;
    private readonly PatrocinadorService _patrocinadorService;
    private readonly ComunidadService _comunidadService;
    private readonly NivelAcademicoService _nivelAcademicoService;

    public PatrocinadorController(FileUploadService fileUploadService,
                                PatrocinadorService patrocinadorService,
                                ComunidadService comunidadService,
                                NivelAcademicoService nivelAcademicoService)
    {
        _fileUploadService = fileUploadService;
        _patrocinadorService = patrocinadorService;
        _comunidadService = comunidadService;
        _nivelAcademicoService = nivelAcademicoService;
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
    [HttpGet("{id}")]
    public async Task<ActionResult<Patrocinador>> GetById(int id)
    {
        var patrocinador = await _patrocinadorService.GetById(id);

        if (patrocinador is null)
        {
            //Es un método para mostrar error explicito
            return PatrocinadorNotFound(id);
        }

        return patrocinador;
    }

    //Información visible del patrocinador
    [HttpGet("infpatrocinador/{id}")]
    //Información para visualizar datos
    public async Task<ActionResult<PatrocinadorOutAllDto>> GetByIdDto(int id)
    {
        var patrocinador = await _patrocinadorService.GetByIdDto(id);

        if (patrocinador is null)
        {
            //Es un método para mostrar error explicito
            return PatrocinadorNotFound(id);
        }

        return patrocinador;
    }


    [HttpPost("create")]
    public async Task<IActionResult> CrearPatrocinador([FromForm] PatrocinadorInputDto model)
    {
        try{
            if (model == null)
        {
            return BadRequest("Los datos del Patrocinador son inválidos.");
        }
        
        //var img = model.ImgPatrocinador.FileName;
        var img = model.ImgPatrocinador;
         string folder = "Becarios";
         string imageUrl = "sin imagenes";

        if (img != null)
        {
            var file = Request.Form.Files[0];
             imageUrl = await _fileUploadService.UploadFileAsync(file, folder);
             Console.WriteLine("Se pasó por aquí");
        }

        Patrocinador patrocinador = new Patrocinador();
        patrocinador.CodigoPais = model.CodigoPais;
        patrocinador.NombrePatrocinador = model.NombrePatrocinador;
        patrocinador.ApellidoPatrocinador = model.ApellidoPatrocinador;
        patrocinador.Profesion = model.Profesion;
        patrocinador.Estado = model.Estado;
        patrocinador.FechaNacimiento = model.FechaNacimiento;
        patrocinador.FechaCreacion = model.FechaCreacion;
        patrocinador.FotoPerfil = imageUrl;
        await _patrocinadorService.Create(patrocinador);

        return Ok("Patrocinador Creado exitosamente");

        }catch{
           return BadRequest();
        }
        
    }

    //Método para actualizar estado
    //METODO PARA EDITAR
    [HttpPut("updateestado/{id}")]
    public async Task<IActionResult> UpdateEstado(int id)
    {
         try{
            await _patrocinadorService.UpdateStatus(id);
             return Ok(new
            {
                status = true,
                message = "Patrocinador modificado correctamente"
            });

        }catch{
           return BadRequest();
        }
    }


    //METODO PARA EDITAR
    [HttpPut("updateimage/{id}")]
    public async Task<IActionResult> EditarPatrocinador(int id, [FromForm] PatrocinadorInputDto model)
    {
         try{
            if (model == null)
        {
            return BadRequest("Los datos del Patrocinador son inválidos.");
        }
        
        //var img = model.ImgPatrocinador.FileName;
        var img = model.ImgPatrocinador;
         string folder = "Becarios";
         string imageUrl = null;

        if (img != null)
        {
            var file = Request.Form.Files[0];
             imageUrl = await _fileUploadService.UploadFileAsync(file, folder);
             Console.WriteLine("Se pasó por aquí");
        }

        Patrocinador patrocinador = new Patrocinador();
        patrocinador.CodigoPais = model.CodigoPais;
        patrocinador.NombrePatrocinador = model.NombrePatrocinador;
        patrocinador.ApellidoPatrocinador = model.ApellidoPatrocinador;
        patrocinador.Profesion = model.Profesion;
        patrocinador.Estado = model.Estado;
        patrocinador.FechaNacimiento = model.FechaNacimiento;
        patrocinador.FechaCreacion = model.FechaCreacion;
        patrocinador.FotoPerfil = imageUrl;
        Console.WriteLine("Se llegó hasta aquí");
        var patrocinadorToUpdate = await _patrocinadorService.GetById(id);
        if(patrocinadorToUpdate is not null){
            await _patrocinadorService.Update(id, patrocinador);
             return Ok(new
            {
                status = true,
                message = "Patrocinador modificado correctamente"
            });

        }else{
            return PatrocinadorNotFound(id);
            }

        }catch{
           return BadRequest();
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
