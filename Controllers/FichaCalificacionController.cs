using Microsoft.AspNetCore.Mvc;
using sistemaQuchooch.Sevices;
using sistemaQuchooch.Data.QuchoochModels;
using Microsoft.AspNetCore.Authorization;
using System.Diagnostics.CodeAnalysis;
using sistemaQuchooch.Data.DTOs;



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
    private readonly ConvertirImagenBase64Service _convertirImagenBase64Service;

    //Constructor
    public FichaCalificacionController(FichaCalificacionService fichaCalificacionService,
                                        FileUploadService fileUploadService,
                                        FichaCalificacionDetalleService fichaCalificacionDetalleService,
                                        CursoFichaCalificacionService cursoFichaCalificacionService,
                                        EstudianteService estudianteService,
                                        EstablecimientoService establecimientoService,
                                        CarreraService carreraService,
                                        ConvertirImagenBase64Service convertirImagenBase64Service
                                        )
    {
        _fichaCalificacionService = fichaCalificacionService;
        _fileUploadService = fileUploadService;
        _fichaCalificacionDetalleService = fichaCalificacionDetalleService;
        _cursoFichaCalificacionService = cursoFichaCalificacionService;
        _estudianteService = estudianteService;
        _establecimientoService = establecimientoService;
        _carreraService = carreraService;
        _convertirImagenBase64Service = convertirImagenBase64Service;
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

    //Lista completa de fichas de calificaciones
    [HttpGet("historial/{codigoBecario}")]
    public async Task<IEnumerable<FichaCalificacionOutDto>> FichaPorEstudiante(string codigoBecario)
    {
        var codigoEstudiante = await _estudianteService.BuscarCodigoEstudiantePorBecario(codigoBecario);
     
        var fichas = await _fichaCalificacionService.FichasPorEstudiante(Convert.ToInt32(codigoEstudiante)); //La converción devolverá un 0 si el valor es nulo
        return fichas;
    }
    
    [HttpGet("buscarPorRangoFecha")]
    public async Task<IEnumerable<FichaCalificacionOutDto>> BuscarPorRangoFecha([FromQuery] DateTime fechaInicio, [FromQuery] DateTime fechaFin)
    {
        var model = new RangoFecha { FechaInicio = fechaInicio, FechaFin = fechaFin };
        var fichas = await _fichaCalificacionService.FichasPorRangoFecha(model);
        return fichas;
    }

    [HttpGet("promediosGenerales/{cantidad}")]
    public async Task<IEnumerable<PromedioEstudianteDto>> PromediosGenerales (int cantidad)
    {
        var promediosGenerales = await _fichaCalificacionService.ObtenerPromediosGenerales(cantidad);
        return promediosGenerales;
    }
    [HttpGet("promedioPorCurso/{codigoFichaCalificacion}")]
    public async Task<IEnumerable<PromedioCursosPorFicha>> PromedioPorCursoFicha (int codigoFichaCalificacion)
    {
        var promediosCursos = await _fichaCalificacionService.ObtenerPromedioPorCursos(codigoFichaCalificacion);
        return promediosCursos;
    }

    [HttpGet("cursos/{codigoFicha}")]
    public async Task<IEnumerable<CursosFichaEscolarDto>> ObtenerListaCursos(int codigoFicha)
    {
        var fichas = await _cursoFichaCalificacionService.ObtenerTodosLosCursosPorFicha(codigoFicha);
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

    [HttpGet("InformacionNuevoBloqueFicha/{codigoFichaCalificacion}")]
    //Información para visualizar datos
    public async Task<ActionResult<InformacionNuevoBloqueFichaDto>> InformacionNuevoBloqueFicha(int codigoFichaCalificacion)
    {
        var informacionfichaCalificacion = await _fichaCalificacionService.InformacionFichaCalificacion(codigoFichaCalificacion);

        var listaBloquesYcursos = await _fichaCalificacionService.ObtenerBloquesYCursos(codigoFichaCalificacion);
        informacionfichaCalificacion.Bloques = listaBloquesYcursos.Bloques;

        if (informacionfichaCalificacion is null)
        {
            //Es un método para mostrar error explicito
            return FichaCalificacionNotFound(codigoFichaCalificacion);
        }


        return informacionfichaCalificacion;
    }

    [HttpGet("InformacionActualizarFicha/{codigoFichaCalificacion}")]
    //Información para visualizar datos
    public async Task<ActionResult<InformacionActualizarFichaDto>> InformacionActualizarFicha(int codigoFichaCalificacion)
    {
        var informacionfichaCalificacion = await _fichaCalificacionService.InformacionActualizarFichaCalificacion(codigoFichaCalificacion);

        var listaBloquesYcursos = await _fichaCalificacionService.ObtenerBloquesYCursosActualizarFicha(codigoFichaCalificacion);
        informacionfichaCalificacion.Bloques = listaBloquesYcursos.Bloques;

        if (informacionfichaCalificacion is null)
        {
            //Es un método para mostrar error explicito
            return FichaCalificacionNotFound(codigoFichaCalificacion);
        }


        return informacionfichaCalificacion;
    }

    //Actualizar nota

    [HttpGet("ObtenerCursos/{codigoFichaCalificacion}")]
    //Información para visualizar datos
    public async Task<ActionResult<IEnumerable<Curso>>> ObtenerCursosPorNivelAcademico(int codigoFichaCalificacion)
    {
        var listadoCursos = await _fichaCalificacionService.ObtenerCursosPorNivelAcademico(codigoFichaCalificacion);

        if (listadoCursos is null)
        {
            //Es un método para mostrar error explicito
            return NotFound(new { status = false, message = "No se encontraron los cursos con el nivel académico indicado." });
        }
        return Ok(listadoCursos);
    }

    [HttpGet("ObtenerCursosPorFicha/{codigoFichaCalificacion}")]
    //Información para visualizar datos
    public async Task<ActionResult<IEnumerable<Curso>>> ObtenerCursosPorFicha(int codigoFichaCalificacion)
    {
        var listadoCursos = await _fichaCalificacionService.ObtenerCursosPorFicha(codigoFichaCalificacion);

        if (listadoCursos is null)
        {
            //Es un método para mostrar error explicito
            return NotFound(new { status = false, message = "No se encontraron los cursos seleccionados." });
        }
        return Ok(listadoCursos);
    }

    [HttpGet("ObtenerImagenesFicha/{codigoFichaCalificacion}")]
    //Información para visualizar datos
    public async Task<ActionResult<IEnumerable<ImagenesFichaDto>>> ObtenerImagenesFicha(int codigoFichaCalificacion)
    {
        var imagenesPorBloqueFicha = await _fichaCalificacionService.ObtenerImagenesFicha(codigoFichaCalificacion);

        if (imagenesPorBloqueFicha is null)
        {
            //Es un método para mostrar error explicito
            return NotFound(new { status = false, message = "No se encontraron las fotografías." });
        }

        foreach (var imagenesBloque in imagenesPorBloqueFicha)
        {
            if (!string.IsNullOrEmpty(imagenesBloque.ImgEstudiante) && imagenesBloque.ImgEstudiante.Length > 30)
            {
                var imgEstudianteBase64 = await _convertirImagenBase64Service.ConvertirImagenBase64(imagenesBloque.ImgEstudiante);
                if (imgEstudianteBase64 != null)
                {
                    imagenesBloque.ImgEstudiante = imgEstudianteBase64;
                }
            }

            if (!string.IsNullOrEmpty(imagenesBloque.ImgFichaCalificacion) && imagenesBloque.ImgFichaCalificacion.Length > 30)
            {
                var imgFichaBase64 = await _convertirImagenBase64Service.ConvertirImagenBase64(imagenesBloque.ImgFichaCalificacion);
                if (imgFichaBase64 != null)
                {
                    imagenesBloque.ImgFichaCalificacion = imgFichaBase64;
                }
            }

            if (!string.IsNullOrEmpty(imagenesBloque.ImgCarta) && imagenesBloque.ImgCarta.Length > 30)
            {
                var imgCartaBase64 = await _convertirImagenBase64Service.ConvertirImagenBase64(imagenesBloque.ImgCarta);
                if (imgCartaBase64 != null)
                {
                    imagenesBloque.ImgCarta = imgCartaBase64;
                }
            }
        }

        return Ok(imagenesPorBloqueFicha);
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


    //CREAR FICHA CON IMÁGENES
    [HttpPost("createFicha")]
    public async Task<IActionResult> CrearFicha([FromForm] FichaInputModel model)
    {
        try
        {
            int sumaDeNotas = 0;
            int contador = 0;
            float promedioNotas = 0;

            if (model == null || model.ImgEstudiante == null)
            {
                return BadRequest("Los datos de la ficha son inválidos.");
            }
            if (model.Cursos == null)
            {
                return BadRequest("No hay cursos para guardar...");
            }

            var imgEstudiante = model.ImgEstudiante;
            var imgFicha = model.ImgFicha;
            var imgCarta = model.ImgCarta;
            string folder = "FichasCalificaciones";
            string imageUrlEstudiante = "sin imagen";
            string imageUrlFicha = "sin imagen";
            string imageUrlCarta = "sin imagen";

            //Cargando imagenes uno por uno con validaciones
            if (imgEstudiante != null)
            {
                var fileEstudiante = Request.Form.Files.FirstOrDefault(f => f.Name == nameof(model.ImgEstudiante));
                if (fileEstudiante != null)
                {
                    imageUrlEstudiante = await _fileUploadService.UploadFileAsync(fileEstudiante, folder);
                }
            }

            if (imgFicha != null)
            {
                var fileFicha = Request.Form.Files.FirstOrDefault(f => f.Name == nameof(model.ImgFicha));
                if (fileFicha != null)
                {
                    imageUrlFicha = await _fileUploadService.UploadFileAsync(fileFicha, folder);
                }
            }

            if (imgCarta != null)
            {
                var fileCarta = Request.Form.Files.FirstOrDefault(f => f.Name == nameof(model.ImgCarta));
                if (fileCarta != null)
                {
                    imageUrlCarta = await _fileUploadService.UploadFileAsync(fileCarta, folder);
                }
            }

            //Guardando datos de la ficha
            FichaCalificacion fichaCalificacion = new FichaCalificacion()
            {
                CodigoEstudiante = model.CodigoEstudiante,
                CodigoEstablecimiento = model.CodigoEstablecimiento,
                CodigoNivelAcademico = model.CodigoNivelAcademico,
                CodigoGrado = model.CodigoGrado,
                CodigoCarrera = model.CodigoCarrera,
                CicloEscolar = model.CicloEscolar,
                FechaRegistro = model.FechaRegistro,
                CodigoModalidadEstudio = model.CodigoModalidadEstudio,
                Estatus = model.Estatus
            };

            var newFichaCalificacion = await _fichaCalificacionService.Create(fichaCalificacion);

            //Obteniendo el codigo de la ficha insertada
            int codigoFichaCalificacionInsertado = newFichaCalificacion.CodigoFichaCalificacion;

            //Guardando datos de detalle de evaluación
            FichaCalificacionDetalle fichaCalificacionDetalle = new FichaCalificacionDetalle()
            {
                CodigoFichaCalificacion = codigoFichaCalificacionInsertado,
                Bloque = model.Bloque,
                ImgEstudiante = imageUrlEstudiante,
                ImgFichaCalificacion = imageUrlFicha,
                ImgCarta = imageUrlCarta,
                Promedio = 0,
                Desempenio = "",
                Estatus = "A"
            };

            var newFichaCalificacionDetalle = await _fichaCalificacionDetalleService.Create(fichaCalificacionDetalle);
            int codigoFichaDetalleInsertado = newFichaCalificacionDetalle.CodigoFichaCalificacionDetalle;

            //Inserción de los cursos

            foreach (var curso in model.Cursos)
            {
                sumaDeNotas += curso.Nota;
                contador++;
                CursoFichaCalificacion cursoFichaCalificacion = new CursoFichaCalificacion()
                {
                    CodigoFichaCalificacionDetalle = codigoFichaDetalleInsertado,
                    CodigoCurso = curso.CodigoCurso,
                    Nota = curso.Nota,
                    Estatus = "A"
                };

                await _cursoFichaCalificacionService.Create(cursoFichaCalificacion);
                promedioNotas = sumaDeNotas / contador;
            }
            //Guardar promedio y codigoPromedio
            var rangosDePromedios = await _fichaCalificacionService.RangosDePromedios();
            int codigoPromedio = 0;
            foreach (var rangos in rangosDePromedios)
            {
                if (promedioNotas >= rangos.ValorMinimo && promedioNotas <= rangos.ValorMaximo)
                {
                    codigoPromedio = rangos.CodigoPromedio;
                }
            }

            await _fichaCalificacionService.UpdatePromedio(promedioNotas, codigoPromedio, codigoFichaDetalleInsertado);

            //Actualizar datos del perfil de estudiante
             //actualizar datos en estudiante
            Estudiante estudiante = new Estudiante(){
                CodigoNivelAcademico = model.CodigoNivelAcademico,
                CodigoGrado = model.CodigoGrado,
                CodigoCarrera = model.CodigoCarrera,
                CodigoEstablecimiento = model.CodigoEstablecimiento,
                CodigoModalidadEstudio = model.CodigoModalidadEstudio
            };
            
            await _fichaCalificacionService.ActualizarPerfilEstudiante(model.CodigoEstudiante ,estudiante);

            return Ok(new { status = true, message = "La ficha de calificación fue creado correctamente." });
        }
        catch
        {
            return BadRequest("Se producjo un error al intentar guardar la ficha de calificación");

        }

    }
    //CREAR FICHA CON IMÁGENES
    [HttpPut("actualizarFicha/{codigoFichaCalificacion}")]
    public async Task<IActionResult> ActualizarFicha(int codigoFichaCalificacion, FichaInputDto fichaInput)
    {
        try
        {
            var fichaCalificacionToUpdate = await _fichaCalificacionService.GetById(codigoFichaCalificacion);
            FichaCalificacion fichaCalificacion = new FichaCalificacion()
            {
                CodigoEstudiante = fichaInput.CodigoEstudiante,
                CodigoEstablecimiento = fichaInput.CodigoEstablecimiento,
                CodigoNivelAcademico = fichaInput.CodigoNivelAcademico,
                CodigoGrado = fichaInput.CodigoGrado,
                CicloEscolar = fichaInput.CicloEscolar,
                FechaRegistro = fichaInput.FechaRegistro,
                CodigoModalidadEstudio = fichaInput.CodigoModalidadEstudio,
                Estatus = fichaInput.Estatus
            };

            if (fichaInput.CodigoCarrera != 0 && fichaInput.CodigoCarrera.HasValue)
            {
                fichaCalificacion.CodigoCarrera = fichaInput.CodigoCarrera;
            }

            if (fichaCalificacionToUpdate is not null)
            {
                //Guardando datos de la ficha
                var updateFichaCalificacion = await _fichaCalificacionService.UpdateFicha(codigoFichaCalificacion, fichaCalificacion);

                return Ok(new { status = true, message = "La ficha de calificación fue actualizado correctamente." });
            }
            else
            {
                return FichaCalificacionNotFound(codigoFichaCalificacion);
            }

        }
        catch
        {
            return BadRequest("Se produjo un error al intentar actualizar la ficha de calificación");

        }

    }

    //Crear nuevo bloque de la ficha
    [HttpPost("crearNuevoBloque/{codigoFichaCalificacion}")]
    public async Task<IActionResult> CrearNuevoBloqueDeFichaEscolar([FromForm] NuevoBloqueFichaEscolar model, int codigoFichaCalificacion)
    {
        try
        {
            int sumaDeNotas = 0;
            int contador = 0;
            float promedioNotas = 0;

            if (model == null || model.ImgEstudiante == null)
            {
                return BadRequest("Los datos de la ficha son inválidos.");
            }
            if (model.Cursos == null)
            {
                return BadRequest("No hay cursos para guardar...");
            }


            var imgEstudiante = model.ImgEstudiante;
            var imgFicha = model.ImgFicha;
            var imgCarta = model.ImgCarta;
            string folder = "FichasCalificaciones";
            string imageUrlEstudiante = "sin imagen";
            string imageUrlFicha = "sin imagen";
            string imageUrlCarta = "sin imagen";

            //Cargando imagenes uno por uno con validaciones
            if (imgEstudiante != null)
            {
                var fileEstudiante = Request.Form.Files.FirstOrDefault(f => f.Name == nameof(model.ImgEstudiante));
                if (fileEstudiante != null)
                {
                    imageUrlEstudiante = await _fileUploadService.UploadFileAsync(fileEstudiante, folder);
                }
            }

            if (imgFicha != null)
            {
                var fileFicha = Request.Form.Files.FirstOrDefault(f => f.Name == nameof(model.ImgFicha));
                if (fileFicha != null)
                {
                    imageUrlFicha = await _fileUploadService.UploadFileAsync(fileFicha, folder);
                }
            }

            if (imgCarta != null)
            {
                var fileCarta = Request.Form.Files.FirstOrDefault(f => f.Name == nameof(model.ImgCarta));
                if (fileCarta != null)
                {
                    imageUrlCarta = await _fileUploadService.UploadFileAsync(fileCarta, folder);
                }
            }

            //Guardando datos de detalle de evaluación
            FichaCalificacionDetalle fichaCalificacionDetalle = new FichaCalificacionDetalle()
            {
                CodigoFichaCalificacion = codigoFichaCalificacion,
                Bloque = model.Bloque,
                ImgEstudiante = imageUrlEstudiante,
                ImgFichaCalificacion = imageUrlFicha,
                ImgCarta = imageUrlCarta,
                Promedio = 0,
                Desempenio = "",
                Estatus = "A"
            };

            var newFichaCalificacionDetalle = await _fichaCalificacionDetalleService.Create(fichaCalificacionDetalle);
            int codigoFichaDetalleInsertado = newFichaCalificacionDetalle.CodigoFichaCalificacionDetalle;

            //Inserción de los cursos

            foreach (var curso in model.Cursos)
            {
                sumaDeNotas += curso.Nota;
                contador++;
                CursoFichaCalificacion cursoFichaCalificacion = new CursoFichaCalificacion()
                {
                    CodigoFichaCalificacionDetalle = codigoFichaDetalleInsertado,
                    CodigoCurso = curso.CodigoCurso,
                    Nota = curso.Nota,
                    Estatus = "A"
                };

                await _cursoFichaCalificacionService.Create(cursoFichaCalificacion);
                promedioNotas = sumaDeNotas / contador;
            }
            //Guardar promedio y codigoPromedio
            var rangosDePromedios = await _fichaCalificacionService.RangosDePromedios();
            int codigoPromedio = 0;
            foreach (var rangos in rangosDePromedios)
            {
                if (promedioNotas >= rangos.ValorMinimo && promedioNotas <= rangos.ValorMaximo)
                {
                    codigoPromedio = rangos.CodigoPromedio;
                }
            }

            await _fichaCalificacionService.UpdatePromedio(promedioNotas, codigoPromedio, codigoFichaDetalleInsertado);

            return Ok(new { status = true, message = "El nuevo bloque fue creado correctamente." });
        }
        catch
        {
            return BadRequest("Se producjo un error al intentar guardar el nuevo bloque de la ficha de calificaciones");

        }

    }

    [HttpPut("actualizarImagenesBloque/{codigoFichaCalificacionDetalle}")]
    public async Task<IActionResult> ActualizarImagenesBloque(int codigoFichaCalificacionDetalle, [FromForm] ImagenesBloque model)
    {
        try
        {
            if (model == null)
            {
                return BadRequest("Los datos no son inválidos.");
            }
            var imgEstudiante = model.ImgEstudiante;
            var imgFicha = model.ImgFicha;
            var imgCarta = model.ImgCarta;
            string folder = "FichasCalificaciones";
            string imageUrlEstudiante = string.Empty;
            string imageUrlFicha = string.Empty;
            string imageUrlCarta = string.Empty;

            //Cargando imagenes uno por uno con validaciones
            if (imgEstudiante != null)
            {
                var fileEstudiante = Request.Form.Files.FirstOrDefault(f => f.Name == nameof(model.ImgEstudiante));
                if (fileEstudiante != null)
                {
                    imageUrlEstudiante = await _fileUploadService.UploadFileAsync(fileEstudiante, folder);
                }
            }

            if (imgFicha != null)
            {
                var fileFicha = Request.Form.Files.FirstOrDefault(f => f.Name == nameof(model.ImgFicha));
                if (fileFicha != null)
                {
                    imageUrlFicha = await _fileUploadService.UploadFileAsync(fileFicha, folder);
                }
            }

            if (imgCarta != null)
            {
                var fileCarta = Request.Form.Files.FirstOrDefault(f => f.Name == nameof(model.ImgCarta));
                if (fileCarta != null)
                {
                    imageUrlCarta = await _fileUploadService.UploadFileAsync(fileCarta, folder);
                }
            }

            FichaCalificacionDetalle fichaCalificacionDetalle = new FichaCalificacionDetalle()
            {
                ImgEstudiante = imageUrlEstudiante,
                ImgFichaCalificacion = imageUrlFicha,
                ImgCarta = imageUrlCarta,
            };

            await _fichaCalificacionDetalleService.UpdateImagenes(codigoFichaCalificacionDetalle, fichaCalificacionDetalle);

            return Ok(new { status = true, message = "Las imágnes se han actualizado correctamente." });
        }
        catch
        {
            return BadRequest("Hubo un error al actualizar las imágenes");
        }
    }

    //Aquí se crea otro curso y será asignado al bloque seleccionado
    [HttpPost("asignarNuevocurso")]
    public async Task<IActionResult> AsignarNuevosCursosBloques(CursoFichaCalificacion cursoFichaCalificacion)
    {
        try
        {
            int contador = 0;
            int sumaDeNotas = 0;
            int promedioNotas = 0;

            var nuevoCursoAsignado = await _cursoFichaCalificacionService.Create(cursoFichaCalificacion);
            int codigoFichaDetalle = nuevoCursoAsignado.CodigoFichaCalificacionDetalle ?? 0;
            if (codigoFichaDetalle == 0)
            {
                return BadRequest(new { status = false, message = "Hubo un eror al asignar el nuevo curso." });
            }
            //Actualizar promedio
            var cursos = await _cursoFichaCalificacionService.ObtenerCursosProBloque(codigoFichaDetalle);
            foreach (var curso in cursos)
            {
                sumaDeNotas += (int)curso.Nota;
                contador++;
            }
            promedioNotas = sumaDeNotas / contador;
            //Guardar promedio y codigoPromedio
            var rangosDePromedios = await _fichaCalificacionService.RangosDePromedios();
            int codigoPromedio = 0;
            foreach (var rangos in rangosDePromedios)
            {
                if (promedioNotas >= rangos.ValorMinimo && promedioNotas <= rangos.ValorMaximo)
                {
                    codigoPromedio = rangos.CodigoPromedio;
                }
            }

            await _fichaCalificacionService.UpdatePromedio(promedioNotas, codigoPromedio, codigoFichaDetalle);

            return Ok(new { status = true, message = "El nuevo curso fue asiganado correctamente." });
        }
        catch
        {
            return BadRequest("Hubo un error al asignar el nuevo curso.");
        }
    }

    [HttpPut("actualizarNotaCurso/{codigoCursoFichaCalificacion}")]
    public async Task<IActionResult> ActualizarNotaCurso(int codigoCursoFichaCalificacion, int notaCalificacion)
    {
        int contador = 0;
        int sumaDeNotas = 0;
        int promedioNotas = 0;

        var fichaCalificacionToUpdate = await _cursoFichaCalificacionService.GetById(codigoCursoFichaCalificacion);
        int codigoFichaDetalle = (int)fichaCalificacionToUpdate.CodigoFichaCalificacionDetalle;
        if (fichaCalificacionToUpdate is not null)
        {
            await _cursoFichaCalificacionService.ActualizarNotaCurso(codigoCursoFichaCalificacion, notaCalificacion);
            //Obtener la lista los cursos para actualizar promedio y rango

            var cursos = await _cursoFichaCalificacionService.ObtenerCursosProBloque(codigoFichaDetalle);
            foreach (var curso in cursos)
            {
                sumaDeNotas += (int)curso.Nota;
                contador++;
            }
            promedioNotas = sumaDeNotas / contador;
            //Guardar promedio y codigoPromedio
            var rangosDePromedios = await _fichaCalificacionService.RangosDePromedios();
            int codigoPromedio = 0;
            foreach (var rangos in rangosDePromedios)
            {
                if (promedioNotas >= rangos.ValorMinimo && promedioNotas <= rangos.ValorMaximo)
                {
                    codigoPromedio = rangos.CodigoPromedio;
                }
            }

            await _fichaCalificacionService.UpdatePromedio(promedioNotas, codigoPromedio, codigoFichaDetalle);


            return Ok(new
            {
                status = true,
                message = "La nota del curso fue modificado correctamente"
            });
        }
        else
        {
            return FichaCalificacionNotFound(codigoCursoFichaCalificacion);
        }
    }


    [HttpDelete("DeleteCurso/{codigoCursoFichaCalificacion}")]
    public async Task<IActionResult> DeleteCursoFichaCalificacion(int codigoCursoFichaCalificacion)
    {
        int contador = 0;
        int sumaDeNotas = 0;
        int promedioNotas = 0;

        var fichaCalificacionToUpdate = await _cursoFichaCalificacionService.GetById(codigoCursoFichaCalificacion);
        if (fichaCalificacionToUpdate is not null)
        {
            int? codigoFichaDetalle = await _cursoFichaCalificacionService.EliminarCursoFichaCalifiacion(codigoCursoFichaCalificacion);

            //Obtener la lista los cursos para actualizar promedio y rango

            var cursos = await _cursoFichaCalificacionService.ObtenerCursosProBloque(codigoFichaDetalle ?? 0);
            foreach (var curso in cursos)
            {
                sumaDeNotas += (int)curso.Nota;
                contador++;
            }
            promedioNotas = sumaDeNotas / contador;



            //Guardar promedio y codigoPromedio
            var rangosDePromedios = await _fichaCalificacionService.RangosDePromedios();
            int codigoPromedio = 0;
            foreach (var rangos in rangosDePromedios)
            {
                if (promedioNotas >= rangos.ValorMinimo && promedioNotas <= rangos.ValorMaximo)
                {
                    codigoPromedio = rangos.CodigoPromedio;
                }
            }

            await _fichaCalificacionService.UpdatePromedio(promedioNotas, codigoPromedio, codigoFichaDetalle ?? 0);

            return Ok(new
            {
                status = true,
                message = "La curso fue eliminado correctamente."
            });
        }
        else
        {
            return NotFound(new { status = false, message = "El curso no se econtró." });
        }
    }

    [HttpDelete("DeleteBloque/{codigoFichaCalificacionDetalle}")]
    public async Task<IActionResult> DeleteBloqueFichaCalificacion(int codigoFichaCalificacionDetalle)
    {
        try
        {
            var codigoBloque = await _fichaCalificacionDetalleService.GetById(codigoFichaCalificacionDetalle);
            if (codigoBloque is null)
            {
                return NotFound(new { status = false, message = "El bloque no se encontró." });
            }
            await _fichaCalificacionDetalleService.Delete(codigoFichaCalificacionDetalle);
            return Ok(new { status = true, message = "El bloque se borró correctamente." });
        }
        catch
        {
            return NotFound(new { status = false, message = "Hubo un error al eliminar el bloque." });
        }

    }
    //MODIFICAR IMAGENE}S EN EL BLOQUE
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
    public int CodigoModalidadEstudio { get; set; }
    public DateTime? CicloEscolar { get; set; }

    public IFormFile? ImgEstudiante { get; set; }
    public IFormFile? ImgFicha { get; set; }
    public IFormFile? ImgCarta { get; set; }
    public DateTime? FechaRegistro { get; set; }
    public int Bloque { get; set; }
    public string? Estatus { get; set; }

    public List<CursoNota> Cursos { get; set; } = new List<CursoNota>();

}

public class CursoNota
{
    public int CodigoCurso { get; set; }
    public int Nota { get; set; }
}


//MODELO PARA EL NUEVO BLOQUE
public class NuevoBloqueFichaEscolar
{

    public int CodigoFichaCalificacion { get; set; }
    public int Bloque { get; set; }

    public IFormFile? ImgEstudiante { get; set; }
    public IFormFile? ImgFicha { get; set; }
    public IFormFile? ImgCarta { get; set; }

    public List<CursoNota> Cursos { get; set; } = new List<CursoNota>();

}

//Probablemente ni sirva
public class CursosNuevoBloque
{
    public int CodigoCurso { get; set; }
    public int Nota { get; set; }
}

public class ImagenesBloque
{
    public IFormFile? ImgEstudiante { get; set; }
    public IFormFile? ImgFicha { get; set; }
    public IFormFile? ImgCarta { get; set; }

}