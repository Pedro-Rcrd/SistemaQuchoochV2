using Microsoft.AspNetCore.Mvc;
using sistemaQuchooch.Sevices;
using sistemaQuchooch.Data.QuchoochModels;
using Microsoft.AspNetCore.Authorization;
using System.Diagnostics.CodeAnalysis;



namespace sistemaQuchooch.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class FichaCalificacionController : ControllerBase
{
    private readonly FichaCalificacionService _fichaCalificacionService;
    private readonly FichaCalificacionDetalleService _fichaCalificacionDetalleService;

    private readonly EstudianteService _estudianteService;
    private readonly EstablecimientoService _establecimientoService;
    private readonly CarreraService _carreraService;
    private readonly CursoFichaCalificacionService _cursoFichaCalificacionService;
    private readonly FileUploadService _fileUploadService;

    //Constructor
    public FichaCalificacionController(FichaCalificacionService fichaCalificacionService,
                                        FileUploadService fileUploadService,
                                        FichaCalificacionDetalleService fichaCalificacionDetalleService,
                                        CursoFichaCalificacionService cursoFichaCalificacionService,
                                        EstudianteService estudianteService,
                                        EstablecimientoService establecimientoService,
                                        CarreraService carreraService
                                        )
    {
        _fichaCalificacionService = fichaCalificacionService;
        _fileUploadService = fileUploadService;
        _fichaCalificacionDetalleService = fichaCalificacionDetalleService;
        _cursoFichaCalificacionService = cursoFichaCalificacionService;
        _estudianteService = estudianteService;
        _establecimientoService = establecimientoService;
        _carreraService = carreraService;
    }

    //Metodo para obtener la lista de fichaCalificaciones
    [HttpGet("getall")]
    public async Task<IActionResult> GetAll(int pagina = 1, int elementosPorPagina = 10, int id = 0)
    {
        var fichasCalificaciones = await _fichaCalificacionService.GetAll(pagina, elementosPorPagina, id);

        // Calcula la cantidad total de registros
        var totalRegistros = await _fichaCalificacionService.CantidadTotalRegistros(); // Debes implementar este método en tu servicio

        // Calcula la cantidad total de páginas
        var totalPaginas = (int)Math.Ceiling((double)totalRegistros / elementosPorPagina);

        // Construye un objeto que incluye la lista de países y la información de paginación
        var resultado = new
        {
            FichasCalificaciones = fichasCalificaciones,
            PaginaActual = pagina,
            TotalPaginas = totalPaginas,
            TotalRegistros = totalRegistros
        };

        return Ok(resultado);
    }

    //Lista completa de fichas de calificaciones
      [HttpGet("selectAll")]
         public async Task<IEnumerable<FichaCalificacionOutDto>> SelectAll()
    {
        var fichas = await _fichaCalificacionService.SelectAll();
        return fichas;
    }


    [HttpGet("{id}")]
    public async Task<ActionResult<FichaCalificacion>> GetById(int id)
    {
        var fichaCalificacion = await _fichaCalificacionService.GetById(id);

        if (fichaCalificacion is null)
        {
            //Es un método para mostrar error explicito
            return FichaCalificacionNotFound(id);
        }

        return Ok(fichaCalificacion);
    }

    [HttpGet("infoficha/{id}")]
    //Información para visualizar datos
    public async Task<ActionResult<FichaCalificacionOutDto>> GetByIdFichaDto(int id)
    {
        var fichaCalificaciones = await _fichaCalificacionService.GetByIdFichaDto(id);

        if (fichaCalificaciones is null)
        {
            //Es un método para mostrar error explicito
            return FichaCalificacionNotFound(id);
        }

        return fichaCalificaciones;
    }

    //Devolver información del bloque por id
    [HttpGet("bloque/{idFicha}/{bloque}")]
    //Información para visualizar datos
    public async Task<ActionResult<FichaCalificacionDetalle>> GetByIdBloqueDto(int idFicha, int bloque)
    {
        var bloqueObtenido = await _fichaCalificacionDetalleService.GetByIdBloque(idFicha, bloque);

        if (bloqueObtenido is null)
        {
            //Es un método para mostrar error explicito
            return NotFound(new { message = $"El bloque con el ID {idFicha} no existe" });
        }
        return bloqueObtenido;
    }

    [HttpGet("getfichaupdate/{id}")]
    public async Task<ActionResult<FichaUpdateOutDto>> GetByIdUpdateFicha(int id)
    {
        var fichaCalificacion = await _fichaCalificacionService.GetByIdUpdateDto(id);

        if (fichaCalificacion is null)
        {
            //Es un método para mostrar error explicito
            return FichaCalificacionNotFound(id);
        }

        return fichaCalificacion;
    }

    [HttpGet("obtenerbloques/{codigoFichaCalificacion}/{bloqueSeleccionado}")]
    public async Task<IActionResult> ObtenerBloquesFichaPorCodigo(int codigoFichaCalificacion, int bloqueSeleccionado)
    {
        try
        {
            var codigoFichaDetalle = await _fichaCalificacionDetalleService.GetBloquePorFicha(codigoFichaCalificacion, bloqueSeleccionado);
            List<int?> codigoBloques = codigoFichaDetalle.Select(x => (int?)x).ToList();
            if (codigoFichaDetalle != null)

            {
                var resultados = await _cursoFichaCalificacionService.GetByCodigoCurso(codigoBloques);
                return Ok(resultados);
            }
            else
            {
                return NotFound(); // Otra respuesta adecuada si no se encuentra nada
            }
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Error interno del servidor: " + ex.Message);
        }
    }


    [HttpGet("obtenerNotasCursos/{codigoFicha}")]
    public async Task<IActionResult> ObtenerNotasCursos(int codigoFicha)
    {
        try
        {
            int cantidadCursos = 0;
            IEnumerable<CursoNotaOutDto> cursosBloqueI = null;
            IEnumerable<CursoNotaOutDto> cursosBloqueII = null;
            IEnumerable<CursoNotaOutDto> cursosBloqueIII = null;
            IEnumerable<CursoNotaOutDto> cursosBloqueIV = null;
            //Obtener codigo de ficha detalle del bloque I
            var codigoFichaDetalleBloqueI = await _fichaCalificacionDetalleService.CodigoFichaDetalle(codigoFicha, 1);
            if (codigoFichaDetalleBloqueI > 0)
            {
                //si existes el bloque uno
                int codigoFichaDetalleI = codigoFichaDetalleBloqueI.Value;
                cursosBloqueI = await _cursoFichaCalificacionService.ListaCursoPorFichaDetalle(codigoFichaDetalleI);
                cantidadCursos = cursosBloqueI.Count();
                Console.WriteLine($"La cantidad de cursos del bloque I es: {cantidadCursos}");
            }
            else
            {
                // Crear una lista de objetos que quieres introducir en la variable cursosBloqueIII
                List<CursoNotaOutDto> objetosAInsertar = new List<CursoNotaOutDto>();

                for (int i = 0; i < cantidadCursos; i++)
                {
                    CursoNotaOutDto objeto = new CursoNotaOutDto
                    {
                        CodigoFichaCalificacionDetalle = 0,
                        CodigoCursoFichaCalificacion = 0,
                        Bloque = 0,
                        Curso = "",
                        CodigoCurso = 0,
                        Nota = 0
                    };

                    objetosAInsertar.Add(objeto);
                }

                // Ahora, asigna la lista de objetos a la variable cursosBloqueIII
                cursosBloqueI = objetosAInsertar;
            }

            //Obtener codigo de ficha detalle del bloque II
            var codigoFichaDetalleBloqueII = await _fichaCalificacionDetalleService.CodigoFichaDetalle(codigoFicha, 2);
            if (codigoFichaDetalleBloqueII > 0)
            {
                int codigoFichaDetalleII = codigoFichaDetalleBloqueII.Value;
                cursosBloqueII = await _cursoFichaCalificacionService.ListaCursoPorFichaDetalle(codigoFichaDetalleII);
            }
            else
            {
                // Crear una lista de objetos que quieres introducir en la variable cursosBloqueIII
                List<CursoNotaOutDto> objetosAInsertar = new List<CursoNotaOutDto>();

                for (int i = 0; i < cantidadCursos; i++)
                {
                    CursoNotaOutDto objeto = new CursoNotaOutDto
                    {
                        CodigoFichaCalificacionDetalle = 0,
                        CodigoCursoFichaCalificacion = 0,
                        Bloque = 0,
                        Curso = "",
                        CodigoCurso = 0,
                        Nota = 0
                    };

                    objetosAInsertar.Add(objeto);
                }

                // Ahora, asigna la lista de objetos a la variable cursosBloqueIII
                cursosBloqueII = objetosAInsertar;
            }

            //Obtener codigo de ficha detalle del bloque III
            var codigoFichaDetalleBloqueIII = await _fichaCalificacionDetalleService.CodigoFichaDetalle(codigoFicha, 3);
            if (codigoFichaDetalleBloqueIII > 0)
            {
                //Convertiendo en INT el codigo de ficha DETALLE
                int codigoFichaDetalleIII = codigoFichaDetalleBloqueIII.Value;
                cursosBloqueIII = await _cursoFichaCalificacionService.ListaCursoPorFichaDetalle(codigoFichaDetalleIII);
            }
            else
            {
                // Crear una lista de objetos que quieres introducir en la variable cursosBloqueIII
                List<CursoNotaOutDto> objetosAInsertar = new List<CursoNotaOutDto>();

                for (int i = 0; i < cantidadCursos; i++)
                {
                    CursoNotaOutDto objeto = new CursoNotaOutDto
                    {
                        CodigoFichaCalificacionDetalle = 0,
                        CodigoCursoFichaCalificacion = 0,
                        Bloque = 0,
                        Curso = "",
                        CodigoCurso = 0,
                        Nota = 0
                    };

                    objetosAInsertar.Add(objeto);
                }

                // Ahora, asigna la lista de objetos a la variable cursosBloqueIII
                cursosBloqueIII = objetosAInsertar;


            }

            //Obtener codigo de ficha detalle del bloque IV
            var codigoFichaDetalleBloqueIV = await _fichaCalificacionDetalleService.CodigoFichaDetalle(codigoFicha, 4);
            if (codigoFichaDetalleBloqueIV > 0)
            {
                int codigoFichaDetalleIV = codigoFichaDetalleBloqueIV.Value;
                cursosBloqueIV = await _cursoFichaCalificacionService.ListaCursoPorFichaDetalle(codigoFichaDetalleIV);
            }
            else
            {
                Console.WriteLine("No existe bloque IV");
                // Crear una lista de objetos que quieres introducir en la variable cursosBloqueIII
                List<CursoNotaOutDto> objetosAInsertar = new List<CursoNotaOutDto>();

                for (int i = 0; i < cantidadCursos; i++)
                {
                    CursoNotaOutDto objeto = new CursoNotaOutDto
                    {
                        CodigoFichaCalificacionDetalle = 0,
                        CodigoCursoFichaCalificacion = 0,
                        Bloque = 0,
                        Curso = "",
                        CodigoCurso = 0,
                        Nota = 0
                    };

                    objetosAInsertar.Add(objeto);
                }

                // Ahora, asigna la lista de objetos a la variable cursosBloqueIII
                cursosBloqueIV = objetosAInsertar;
            }
            var result = new
            {
                CursosBloqueI = cursosBloqueI,
                CursosBloqueII = cursosBloqueII,
                CursosBloqueIII = cursosBloqueIII,
                CursosBloqueIV = cursosBloqueIV
            };

            return Ok(result);
        }
        catch (Exception ex)
        {
            // Manejar errores
            return BadRequest("Error al obtener los resultados por código de curso");
        }
    }


    //PARA LA GRAFICA - OBTENIENDO BLOQUES Y PROMEDIOS
    [HttpGet("bloquesficha/{codigoFicha}")]
    public async Task<IActionResult> GetByCodigoFichaCalificacion(int codigoFicha)
    {
        var result = await _fichaCalificacionDetalleService.GetByCodigoFichaCalificacion(codigoFicha);
        return Ok(result);
    }

    //Cantida de niveles academicos por ciclo
    [HttpGet("totalregistronivelacademico/{año}")]
    public async Task<IActionResult> GetTotalRegistrosPorNivelAcademico(int año)
    {
        try
        {
            var cantidadTotal = await _fichaCalificacionService.CantidadRegistrosPorNivelAcademico(año);
            return Ok(cantidadTotal);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }


    //Crear ficha 
    [HttpPost("create")]
    public async Task<IActionResult> Create(FichaInput ficha)
    {
        string validationResult = await ValidateFicha(ficha);
        if (!validationResult.Equals("valid"))
        {
            return BadRequest(new { message = validationResult });
        }
        FichaCalificacion fichaCalificacion = new FichaCalificacion();
        fichaCalificacion.CodigoEstudiante = ficha.CodigoEstudiante;
        fichaCalificacion.CodigoEstablecimiento = ficha.CodigoEstablecimiento;
        fichaCalificacion.CodigoNivelAcademico = ficha.CodigoNivelAcademico;
        fichaCalificacion.CodigoGrado = ficha.CodigoGrado;
        fichaCalificacion.CodigoCarrera = ficha.CodigoCarrera;
        fichaCalificacion.CicloEscolar = ficha.CicloEscolar;
        fichaCalificacion.FechaRegistro = ficha.FechaRegistro;
        var newFichaCalificacion = await _fichaCalificacionService.Create(fichaCalificacion);

        return Ok("Ficha de calificacion creada");
    }


    //CREAR FICHA CON IMÁGENES
    [HttpPost("createficha")]
    public async Task<IActionResult> CrearFicha([FromForm] FichaInputModel model)
    {
        try
        {
            if (model == null)
            {
                return BadRequest("Los datos de la ficha son inválidos.");
            }

            var imgEstudiante = model.ImgEstudiante;
            var imgFicha = model.ImgFicha;
            var imgCarta = model.ImgCarta;
            string folder = "Becarios";
            string imageUrlEstudiante = "sin imagen";
            string imageUrlFicha = "sin imagen";
            string imageUrlCarta = "sin imagen";

            //Cargando imagenes uno por uno con validaciones
            if (imgEstudiante != null)
            {
                var fileEstudiante = Request.Form.Files[0];
                imageUrlEstudiante = await _fileUploadService.UploadFileAsync(fileEstudiante, folder);
            }

            if (imgFicha != null)
            {
                var fileFicha = Request.Form.Files[1];
                imageUrlFicha = await _fileUploadService.UploadFileAsync(fileFicha, folder);
            }

            if (imgCarta != null)
            {
                var fileCarta = Request.Form.Files[2];
                imageUrlCarta = await _fileUploadService.UploadFileAsync(fileCarta, folder);
            }

            //Guardando datos de la ficha
            FichaCalificacion fichaCalificacion = new FichaCalificacion();
            fichaCalificacion.CodigoEstudiante = model.CodigoEstudiante;
            fichaCalificacion.CodigoEstablecimiento = model.CodigoEstablecimiento;
            fichaCalificacion.CodigoNivelAcademico = model.CodigoNivelAcademico;
            fichaCalificacion.CodigoGrado = model.CodigoGrado;
            fichaCalificacion.CodigoCarrera = model.CodigoCarrera;
            fichaCalificacion.CicloEscolar = model.CicloEscolar;
            fichaCalificacion.FechaRegistro = model.FechaRegistro;
            var newFichaCalificacion = await _fichaCalificacionService.Create(fichaCalificacion);
            //Obteniendo el codigo de la ficha insertada
            int codigoFichaCalificacion = newFichaCalificacion.CodigoFichaCalificacion;

            //Guardando datos de detalle de evaluación
            FichaCalificacionDetalle fichaCalificacionDetalle = new FichaCalificacionDetalle();
            fichaCalificacionDetalle.CodigoFichaCalificacion = codigoFichaCalificacion;
            fichaCalificacionDetalle.Bloque = 1;
            fichaCalificacionDetalle.ImgEstudiante = imageUrlEstudiante;
            fichaCalificacionDetalle.ImgFichaCalificacion = imageUrlFicha;
            fichaCalificacionDetalle.ImgCarta = imageUrlCarta;
            fichaCalificacionDetalle.Promedio = 0;
            fichaCalificacionDetalle.Desempenio = "";
            var newFichaCalificacionDetalle = await _fichaCalificacionDetalleService.Create(fichaCalificacionDetalle);

            return Ok(newFichaCalificacionDetalle.CodigoFichaCalificacionDetalle);
        }
        catch
        {
            return BadRequest("No se pudo crear la ficha de calificación");

        }

    }

    //Aquí se manda la lista de cursos y se va asignando a fichaDetalle recién creado
    [HttpPost("createcurso")]
    public async Task<IActionResult> GuardarCursosNotas([FromBody] List<CursoNota> cursosNotas)
    {
        // Aquí puedes procesar la lista de cursos y notas como desees
        // Por ejemplo, puedes guardarlos en la base de datos
        Console.WriteLine("Aqui se llegó");
        Console.WriteLine("Ya hay respuesta");
        int maxCodigo = await _fichaCalificacionDetalleService.GetMaxCodigoFichaCalificacionDetalle();
        Console.WriteLine($"El ultimo registro insertado del bloque es: {maxCodigo}");

        foreach (var cursoNota in cursosNotas)
        {
            Console.WriteLine($"CodigoCurso: {cursoNota.CodigoCurso}");
            Console.WriteLine($"Curso: {cursoNota.Curso}");
            Console.WriteLine($"Nota: {cursoNota.Nota}");

            CursoFichaCalificacion cursoFichaCalificacion = new CursoFichaCalificacion();
            cursoFichaCalificacion.CodigoFichaCalificacionDetalle = maxCodigo;
            cursoFichaCalificacion.CodigoCurso = cursoNota.CodigoCurso;
            cursoFichaCalificacion.Nota = cursoNota.Nota;
            await _cursoFichaCalificacionService.Create(cursoFichaCalificacion);
        }

        return Ok("Cursos y notas guardados con éxito");
    }
    //Creación del NUEVO BLOQUE
    //MODIFICAR IMAGENES EN EL BLOQUE
    [HttpPost("createnuevobloque")]
    public async Task<IActionResult> CrearNuevoBloque([FromForm] NuevoBloque model)
    {
        try
        {
            //Obteniendo los bloques que tiene la ficha para no replicarlos
            var bloques = await _fichaCalificacionDetalleService.GetCuantosBloquesTieneFicha(model.CodigoFichaCalificacion);

            foreach (var bloque in bloques)
            {
                Console.WriteLine("Bloque: " + bloque);
                Console.WriteLine("Model Bloque: " + model.Bloque);
                if (model.Bloque == bloque)
                {
                    return BadRequest("El bloque ya existe");
                }
            }

            if (model == null)
            {
                return BadRequest("Los datos de la ficha son inválidos.");
            }

            var imgEstudiante = model.ImgEstudiante;
            var imgFicha = model.ImgFicha;
            var imgCarta = model.ImgCarta;
            string folder = "Becarios";
            string imageUrlEstudiante = "sin imagen";
            string imageUrlFicha = "sin imagen";
            string imageUrlCarta = "sin imagen";

            //Cargando imagenes uno por uno con validaciones
            if (imgEstudiante != null)
            {
                var fileEstudiante = Request.Form.Files[0];
                imageUrlEstudiante = await _fileUploadService.UploadFileAsync(fileEstudiante, folder);
            }

            if (imgFicha != null)
            {
                var fileFicha = Request.Form.Files[1];
                imageUrlFicha = await _fileUploadService.UploadFileAsync(fileFicha, folder);
            }

            if (imgCarta != null)
            {
                var fileCarta = Request.Form.Files[2];
                imageUrlCarta = await _fileUploadService.UploadFileAsync(fileCarta, folder);
            }

            //Guardando datos de detalle de evaluación
            FichaCalificacionDetalle fichaCalificacionDetalle = new FichaCalificacionDetalle();
            fichaCalificacionDetalle.CodigoFichaCalificacion = model.CodigoFichaCalificacion;
            fichaCalificacionDetalle.Bloque = model.Bloque;
            fichaCalificacionDetalle.ImgEstudiante = imageUrlEstudiante;
            fichaCalificacionDetalle.ImgFichaCalificacion = imageUrlFicha;
            fichaCalificacionDetalle.ImgCarta = imageUrlCarta;
            fichaCalificacionDetalle.Promedio = 0;
            fichaCalificacionDetalle.Desempenio = "";
            await _fichaCalificacionDetalleService.Create(fichaCalificacionDetalle);

            return Ok();
        }
        catch
        {
            return BadRequest("No se pudo crear el bloque de la ficha de calificación");

        }
    }

    //CREAR CURSOS PARA EL SEGUNDO, TERCERO, Y CUARTO BLOQUE CREADO
    [HttpPost("createcursosnuevobloque")]
    public async Task<IActionResult> CursosNuevoBloque([FromBody] List<CursosNuevoBloque> cursosNuevoBloque)
    {
        Console.WriteLine("Aqui se llegó");
        Console.WriteLine("Ya hay respuesta");
        int maxCodigo = await _fichaCalificacionDetalleService.GetMaxCodigoFichaCalificacionDetalle();
        Console.WriteLine($"El ultimo registro insertado del bloque es: {maxCodigo}");

        foreach (var cursoNota in cursosNuevoBloque)
        {
            Console.WriteLine($"CodigoCurso: {cursoNota.CodigoCurso}");
            Console.WriteLine($"Nota: {cursoNota.Nota}");

            CursoFichaCalificacion cursoFichaCalificacion = new CursoFichaCalificacion();
            cursoFichaCalificacion.CodigoFichaCalificacionDetalle = maxCodigo;
            cursoFichaCalificacion.CodigoCurso = cursoNota.CodigoCurso;
            cursoFichaCalificacion.Nota = cursoNota.Nota;
            await _cursoFichaCalificacionService.Create(cursoFichaCalificacion);
        }

        return Ok("Cursos y notas guardados con éxito");
    }

    //Aquí se crea otro curso y será asignado al bloque seleccionado
    [HttpPost("asignarcurso")]
    public async Task<IActionResult> AsignarNuevosCursosBloques(CursoFichaCalificacion cursoFichaCalificacion)
    {
        Console.WriteLine("Este es el método para asignar nuevos cursos");

        await _cursoFichaCalificacionService.Create(cursoFichaCalificacion);
        return Ok("Cursos asignados con éxito");
    }

    [HttpPut("updatecursonota/{id}")]
    public async Task<IActionResult> UpdateCursoNota(int id, CursoFichaCalificacion cursoFichaCalificacion)
    {
        var fichaCalificacionToUpdate = await _cursoFichaCalificacionService.GetById(id);

        if (fichaCalificacionToUpdate is not null)
        {
            await _cursoFichaCalificacionService.Update(id, cursoFichaCalificacion);
            return Ok(new
            {
                status = true,
                message = "El curso fue modificado correctamente"
            });
        }
        else
        {
            return FichaCalificacionNotFound(id);
        }
    }


    //MODIFICAR IMAGENES EN EL BLOQUE
    [HttpPut("updatefichadetalleimg/{codigoFichaDetalle}")]
    public async Task<IActionResult> ActualizrImgFichaDetalle([FromForm] FichaDetalleImgInputDto model, int codigoFichaDetalle)
    {
        if (model == null || model.ImgEstudiante == null)
        {
            return BadRequest("Los datos del estudiante son inválidos.");
        }

        var file = Request.Form.Files[0];
        var fileFicha = Request.Form.Files[1];
        var fileCarta = Request.Form.Files[2];

        var img = model.ImgEstudiante.FileName;
        var imgFicha = model.ImgFicha.FileName;
        //var ImgCarta = model.ImgCarta.FileName;
        string folder = "Becarios";
        if (imgFicha is null)
        {
            return BadRequest("La imagen de ficha no se cargó correctamente");
        }
        if (img is null)
        {
            return BadRequest("La imagen no se cargó correctamente");
        }
        Console.WriteLine("Esta llegando la imagen");
        var imageUrlEstudiante = await _fileUploadService.UploadFileAsync(file, folder);
        Console.WriteLine($"Que se esta obteniendo URL ESTUDIANTE {imageUrlEstudiante}");

        var imageUrlFicha = await _fileUploadService.UploadFileAsync(fileFicha, folder);
        Console.WriteLine($"Url FICHA CALIFICACION {imageUrlFicha}");


        var imageUrlCarta = await _fileUploadService.UploadFileAsync(fileCarta, folder);
        Console.WriteLine($"Url CARTA {imageUrlCarta}");

        //GUARDAR DATOS DE DETALLE DE EVALUACIÓN
        FichaCalificacionDetalle fichaCalificacionDetalle = new FichaCalificacionDetalle();
        fichaCalificacionDetalle.CodigoFichaCalificacion = model.CodigoFichaCalificacion;
        fichaCalificacionDetalle.Bloque = model.Bloque;
        fichaCalificacionDetalle.ImgEstudiante = imageUrlEstudiante;
        fichaCalificacionDetalle.ImgFichaCalificacion = imageUrlFicha;
        fichaCalificacionDetalle.ImgCarta = imageUrlCarta;
        fichaCalificacionDetalle.Promedio = 0;
        fichaCalificacionDetalle.Desempenio = "";
        Console.WriteLine("Ya se asignaron DATOS");
        await _fichaCalificacionDetalleService.Update(codigoFichaDetalle, fichaCalificacionDetalle);

        return Ok();
    }

    //DELETE CURSO NOTA
    [HttpDelete("deletecursonota/{id}")]
    public async Task<IActionResult> DeleteCursoNota(int id)
    {
        var cursoNotaToDelete = await _cursoFichaCalificacionService.GetById(id);

        if (cursoNotaToDelete is not null)
        {
            await _cursoFichaCalificacionService.Delete(id);
            return Ok();
        }
        else
        {
            return FichaCalificacionNotFound(id);
        }
    }

    [HttpPut("update/{id}")]
    public async Task<IActionResult> Update(int id, FichaInputDto fichaInput)
    {
        var fichaCalificacionToUpdate = await _fichaCalificacionService.GetById(id);
        FichaCalificacion fichaCalificacion = new FichaCalificacion();
        fichaCalificacion.CodigoEstudiante = fichaInput.CodigoEstudiante;
        fichaCalificacion.CodigoEstablecimiento = fichaInput.CodigoEstablecimiento;
        fichaCalificacion.CodigoNivelAcademico = fichaInput.CodigoNivelAcademico;
        fichaCalificacion.CodigoGrado = fichaInput.CodigoGrado;
        fichaCalificacion.CodigoCarrera = fichaInput.CodigoCarrera;
        fichaCalificacion.CicloEscolar = fichaInput.CicloEscolar;
        fichaCalificacion.FechaRegistro = fichaInput.FechaRegistro;

        if (fichaCalificacionToUpdate is not null)
        {
            await _fichaCalificacionService.Update(id, fichaCalificacion);
            return NoContent();
        }
        else
        {
            return FichaCalificacionNotFound(id);
        }
    }

    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var fichaCalificacionToDelete = await _fichaCalificacionService.GetById(id);

        if (fichaCalificacionToDelete is not null)
        {
            await _fichaCalificacionService.Delete(id);
            return Ok();
        }
        else
        {
            return FichaCalificacionNotFound(id);
        }
    }

    //Definir un método para indicar el mensaje del NotFound
    public NotFoundObjectResult FichaCalificacionNotFound(int id)
    {
        return NotFound(new { message = $"La FichaCalificacion con el ID {id} no existe" });
    }

    public async Task<string> ValidateFicha(FichaInput ficha)
    {
        string result = "valid";
        var estudiante = ficha.CodigoEstudiante;
        var existeEstudiante = await _estudianteService.GetById(estudiante);
        var carrera = ficha.CodigoCarrera.GetValueOrDefault();
        var existeCarrera = await _carreraService.GetById(carrera);
        var establecimiento = ficha.CodigoEstablecimiento;
        var existeEstablecimiento = await _establecimientoService.GetById(establecimiento);
        if (existeEstudiante is null)
        {
            result = $"La El estudiante con el codigo {estudiante} no existe";
        }
        if (existeCarrera is null)
        {
            result = $"La carrera con el codigo {carrera} no existe";
        }
        if (existeEstablecimiento is null)
        {
            result = $"El establecimiento con el codigo {establecimiento} no existe";
        }
        return result;
    }


}

public class FichaInputModel
{
    public int CodigoEstudiante { get; set; }
    public int CodigoEstablecimiento { get; set; }
    public int CodigoNivelAcademico { get; set; }
    public int CodigoGrado { get; set; }

    public int? CodigoCarrera { get; set; }

    public DateTime? CicloEscolar { get; set; }

    public IFormFile? ImgEstudiante { get; set; }
    public IFormFile? ImgFicha { get; set; }
    public IFormFile? ImgCarta { get; set; }
    public DateTime? FechaRegistro { get; set; }

}

public class CursoNota
{
    public int CodigoCurso { get; set; }
    public string Curso { get; set; }
    public int Nota { get; set; }
}

public class CursosNuevoBloque
{
    public int CodigoCurso { get; set; }
    public int Nota { get; set; }
}

//MODELO PARA EL NUEVO BLOQUE
public class NuevoBloque
{
    public int CodigoFichaCalificacion { get; set; }
    public int Bloque { get; set; }
    public IFormFile ImgEstudiante { get; set; }
    public IFormFile ImgFicha { get; set; }
    public IFormFile ImgCarta { get; set; }
}