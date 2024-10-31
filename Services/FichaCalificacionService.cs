using Microsoft.EntityFrameworkCore;
using sistemaQuchooch.Data;
using sistemaQuchooch.Data.QuchoochModels;
using sistemaQuchooch.Data.DTOs;


namespace sistemaQuchooch.Sevices;

public class FichaCalificacionService
{
    private readonly QuchoochContext _context;

    //constructor
    public FichaCalificacionService(QuchoochContext context)
    {
        _context = context;
    }

    //Metodo para obtener toda la información de FichaCalificacion
    public async Task<IEnumerable<FichaCalificacionOutDto>> GetAll(int pagina, int elementosPorPagina, int codigoEstudiante)
    {
        var fichasCalificaciones = await _context.FichaCalificacions
        .Where(a => codigoEstudiante == 0 || a.CodigoEstudiante == codigoEstudiante)
        .OrderByDescending(a => a.CodigoFichaCalificacion)
        .Select(a => new FichaCalificacionOutDto
        {
            CodigoFichaCalificacion = a.CodigoFichaCalificacion,
            Estudiante = a.CodigoEstudianteNavigation != null ? a.CodigoEstudianteNavigation.NombreEstudiante : "",
            ApellidoEstudiante = a.CodigoEstudianteNavigation != null ? a.CodigoEstudianteNavigation.ApellidoEstudiante : "",
            Establecimiento = a.CodigoEstablecimientoNavigation != null ? a.CodigoEstablecimientoNavigation.NombreEstablecimiento : "",
            NivelAcademico = a.CodigoNivelAcademicoNavigation != null ? a.CodigoNivelAcademicoNavigation.NombreNivelAcademico : "",
            Grado = a.CodigoGradoNavigation.NombreGrado,
            Carrera = a.CodigoCarreraNavigation != null ? a.CodigoCarreraNavigation.NombreCarrera : "",
            CicloEscolar = a.CicloEscolar
        })
            .Skip((pagina - 1) * elementosPorPagina)
            .Take(elementosPorPagina)
            .ToListAsync();
        return fichasCalificaciones;
    }
    public async Task<IEnumerable<FichaCalificacionOutDto>> SelectAll()
    {
        var fichasCalificaciones = await _context.FichaCalificacions
         .OrderByDescending(a => a.CodigoFichaCalificacion)
        .Select(a => new FichaCalificacionOutDto
        {
            CodigoFichaCalificacion = a.CodigoFichaCalificacion,
            CodigoBecario = a.CodigoEstudianteNavigation.CodigoBecario,
            Estudiante = a.CodigoEstudianteNavigation != null ? a.CodigoEstudianteNavigation.NombreEstudiante : "",
            ApellidoEstudiante = a.CodigoEstudianteNavigation != null ? a.CodigoEstudianteNavigation.ApellidoEstudiante : "",
            Establecimiento = a.CodigoEstablecimientoNavigation != null ? a.CodigoEstablecimientoNavigation.NombreEstablecimiento : "",
            NivelAcademico = a.CodigoNivelAcademicoNavigation != null ? a.CodigoNivelAcademicoNavigation.NombreNivelAcademico : "",
            Grado = a.CodigoGradoNavigation.NombreGrado,
            Carrera = a.CodigoCarreraNavigation != null ? a.CodigoCarreraNavigation.NombreCarrera : "",
            CicloEscolar = a.CicloEscolar,
            ModalidadEstudio = a.CodigoModalidadEstudioNavigation.NombreModalidadEstudio,
            Estatus = a.Estatus
        })
       .ToListAsync();

        return fichasCalificaciones;
    }
    public async Task<IEnumerable<FichaCalificacionOutDto>> FichasPorEstudiante(int codigoEstudiante)
    {
        var fichasCalificaciones = await _context.FichaCalificacions
        .Where(a => a.CodigoEstudiante == codigoEstudiante)
        .OrderByDescending(a => a.CodigoFichaCalificacion)
        .Select(a => new FichaCalificacionOutDto
        {
            CodigoFichaCalificacion = a.CodigoFichaCalificacion,
            CodigoBecario = a.CodigoEstudianteNavigation.CodigoBecario,
            Estudiante = a.CodigoEstudianteNavigation != null ? a.CodigoEstudianteNavigation.NombreEstudiante : "",
            ApellidoEstudiante = a.CodigoEstudianteNavigation != null ? a.CodigoEstudianteNavigation.ApellidoEstudiante : "",
            Establecimiento = a.CodigoEstablecimientoNavigation != null ? a.CodigoEstablecimientoNavigation.NombreEstablecimiento : "",
            NivelAcademico = a.CodigoNivelAcademicoNavigation != null ? a.CodigoNivelAcademicoNavigation.NombreNivelAcademico : "",
            Grado = a.CodigoGradoNavigation.NombreGrado,
            Carrera = a.CodigoCarreraNavigation != null ? a.CodigoCarreraNavigation.NombreCarrera : "",
            CicloEscolar = a.CicloEscolar,
            ModalidadEstudio = a.CodigoModalidadEstudioNavigation.NombreModalidadEstudio,
            Estatus = a.Estatus
        })
       .ToListAsync();
        return fichasCalificaciones;
    }
    public async Task<IEnumerable<FichaCalificacionOutDto>> FichasPorRangoFecha(RangoFecha model)
    {
        var fichasCalificaciones = await _context.FichaCalificacions
         .Where(a => a.FechaRegistro >= model.FechaInicio && a.FechaRegistro <= model.FechaFin)
         .OrderByDescending(a => a.CodigoFichaCalificacion)
        .Select(a => new FichaCalificacionOutDto
        {
            CodigoFichaCalificacion = a.CodigoFichaCalificacion,
            CodigoBecario = a.CodigoEstudianteNavigation.CodigoBecario,
            Estudiante = a.CodigoEstudianteNavigation != null ? a.CodigoEstudianteNavigation.NombreEstudiante : "",
            ApellidoEstudiante = a.CodigoEstudianteNavigation != null ? a.CodigoEstudianteNavigation.ApellidoEstudiante : "",
            Establecimiento = a.CodigoEstablecimientoNavigation != null ? a.CodigoEstablecimientoNavigation.NombreEstablecimiento : "",
            NivelAcademico = a.CodigoNivelAcademicoNavigation != null ? a.CodigoNivelAcademicoNavigation.NombreNivelAcademico : "",
            Grado = a.CodigoGradoNavigation.NombreGrado,
            Carrera = a.CodigoCarreraNavigation != null ? a.CodigoCarreraNavigation.NombreCarrera : "",
            CicloEscolar = a.CicloEscolar,
            ModalidadEstudio = a.CodigoModalidadEstudioNavigation.NombreModalidadEstudio,
            Estatus = a.Estatus
        })
       .ToListAsync();

        return fichasCalificaciones;
    }


    public async Task<IEnumerable<PromedioEstudianteDto>> ObtenerPromediosGenerales(int cantidad)
    {
        var promediosGenerales = await _context.FichaCalificacionDetalles
            .Where(fcd => fcd.CodigoFichaCalificacionNavigation.Estatus == "A" && fcd.Estatus == "A")
            .GroupBy(fcd => new
            {
                CodigoFichaCalificacion = fcd.CodigoFichaCalificacion ?? 0, // Manejar el valor nullable
                fcd.CodigoFichaCalificacionNavigation.CodigoEstudiante,
                NombreCompleto = fcd.CodigoFichaCalificacionNavigation.CodigoEstudianteNavigation.NombreEstudiante + " " + fcd.CodigoFichaCalificacionNavigation.CodigoEstudianteNavigation.ApellidoEstudiante,
                fcd.CodigoFichaCalificacionNavigation.CodigoNivelAcademicoNavigation.NombreNivelAcademico,
                fcd.CodigoFichaCalificacionNavigation.CodigoGradoNavigation.NombreGrado
            })
            .Select(g => new PromedioEstudianteDto
            {
                CodigoFichaCalificacion = g.Key.CodigoFichaCalificacion,
                CodigoEstudiante = g.Key.CodigoEstudiante,
                NombreEstudiante = g.Key.NombreCompleto,
                PromedioGeneral = (double)(g.Average(fcd => fcd.Promedio) ?? 0), // Conversión explícita a double
                NivelAcademico = g.Key.NombreNivelAcademico,
                Grado = g.Key.NombreGrado
            })
            .OrderByDescending(dto => dto.PromedioGeneral)
            .Take(cantidad)
            .ToListAsync();

        return promediosGenerales;
    }


    public async Task<IEnumerable<PromedioCursosPorFicha>?> ObtenerPromedioPorCursos(int codigoFichaCalificacion)
{
    // Obtener los detalles de la ficha de calificación
    var bloques = await _context.FichaCalificacionDetalles
        .Where(a => a.CodigoFichaCalificacion == codigoFichaCalificacion && a.Estatus == "A")
        .Select(a => a.CodigoFichaCalificacionDetalle)
        .ToListAsync();

    // Validar si hay bloques antes de continuar
    if (!bloques.Any()) 
        return null;

    // Obtener los cursos agrupados por código y nombre del curso
    var cursos = await _context.CursoFichaCalificacions
        .Where(a => a.CodigoFichaCalificacionDetalle.HasValue && 
            bloques.Contains(a.CodigoFichaCalificacionDetalle.Value) && a.Estatus == "A") // Uso de Contains
        .GroupBy(cfc => new
        {
            cfc.CodigoCurso,
            cfc.CodigoCursoNavigation.NombreCurso,
        })
        .Select(a => new PromedioCursosPorFicha
        {
            Curso = a.Key.NombreCurso,
            PromedioGeneral = (double)(a.Average(cfc => cfc.Nota) ?? 0), // Conversión segura a double
        })
        .OrderByDescending(dto => dto.PromedioGeneral)
        .ToListAsync();

    return cursos;
}



    public async Task<InformacionNuevoBloqueFichaDto?> InformacionFichaCalificacion(int codigoFichaCalificacion)
    {
        var encabezadoFichaCalificacion = await _context.FichaCalificacions
         .Where(a => a.CodigoFichaCalificacion == codigoFichaCalificacion)  // Verificar que CicloEscolar no sea nulo y filtrar por año
        .Select(a => new InformacionNuevoBloqueFichaDto
        {
            CodigoBecario = a.CodigoEstudianteNavigation.CodigoBecario,
            NombreEstudiante = a.CodigoEstudianteNavigation != null ? a.CodigoEstudianteNavigation.NombreEstudiante : "",
            ApellidoEstudiante = a.CodigoEstudianteNavigation != null ? a.CodigoEstudianteNavigation.ApellidoEstudiante : "",
            Establecimiento = a.CodigoEstablecimientoNavigation != null ? a.CodigoEstablecimientoNavigation.NombreEstablecimiento : "",
            NivelAcademico = a.CodigoNivelAcademicoNavigation != null ? a.CodigoNivelAcademicoNavigation.NombreNivelAcademico : "",
            CodigoNivelAcademico = a.CodigoNivelAcademicoNavigation.CodigoNivelAcademico,
            Grado = a.CodigoGradoNavigation.NombreGrado,
            Carrera = a.CodigoCarreraNavigation != null ? a.CodigoCarreraNavigation.NombreCarrera : "",
            CicloEscolar = a.CicloEscolar,
            Modalidad = a.CodigoModalidadEstudioNavigation.NombreModalidadEstudio
        })
       .SingleOrDefaultAsync();

        int cantidadBloques = await _context.FichaCalificacionDetalles
         .Where(f => f.CodigoFichaCalificacion == codigoFichaCalificacion)
         .OrderByDescending(f => f.CodigoFichaCalificacionDetalle)  // Ordenar de mayor a menor
         .Select(f =>
             f.Bloque ?? 0
         )
         .FirstOrDefaultAsync();

        encabezadoFichaCalificacion.NumeroBloque = cantidadBloques;

        return encabezadoFichaCalificacion;
    }


    public async Task<InformacionActualizarFichaDto?> InformacionActualizarFichaCalificacion(int codigoFichaCalificacion)
    {
        var encabezadoFichaCalificacion = await _context.FichaCalificacions
         .Where(a => a.CodigoFichaCalificacion == codigoFichaCalificacion)  // Verificar que CicloEscolar no sea nulo y filtrar por año
        .Select(a => new InformacionActualizarFichaDto
        {
            CodigoEstudiante = a.CodigoEstudiante,
            NombreEstudiante = a.CodigoEstudianteNavigation.NombreEstudiante,
            ApellidoEstudiante = a.CodigoEstudianteNavigation.ApellidoEstudiante,
            CodigoEstablecimiento = a.CodigoEstablecimiento,
            CodigoNivelAcademico = a.CodigoNivelAcademico,
            CodigoGrado = a.CodigoGrado,
            CodigoCarrera = a.CodigoCarrera,
            CicloEscolar = a.CicloEscolar,
            FechaRegistro = a.FechaRegistro,
            CodigoModalidadEstudio = a.CodigoModalidadEstudio,
            Estatus = a.Estatus,
        })
       .SingleOrDefaultAsync();

        int cantidadBloques = await _context.FichaCalificacionDetalles
         .Where(f => f.CodigoFichaCalificacion == codigoFichaCalificacion)
         .OrderByDescending(f => f.CodigoFichaCalificacionDetalle)  // Ordenar de mayor a menor
         .Select(f =>
             f.Bloque ?? 0
         )
         .FirstOrDefaultAsync();

        encabezadoFichaCalificacion.CantidadBloque = cantidadBloques;

        return encabezadoFichaCalificacion;
    }

    public async Task<IEnumerable<Curso>?> ObtenerCursosPorNivelAcademico(int codigoFichaCalificacion)
    {
        int codigoNivelAcademico = await _context.FichaCalificacions
        .Where(a => a.CodigoFichaCalificacion == codigoFichaCalificacion)
        .Select(a => a.CodigoNivelAcademico)
       .SingleOrDefaultAsync();

        var cursos = await _context.Cursos
        .Where(a => a.CodigoNivelAcademico == codigoNivelAcademico)
        .ToListAsync();

        return cursos;
    }
    public async Task<IEnumerable<Curso>?> ObtenerCursosPorFicha(int codigoFichaCalificacion)
    {
        int codigoUltimoBloqueFicha = await _context.FichaCalificacionDetalles
         .Where(f => f.CodigoFichaCalificacion == codigoFichaCalificacion && f.Estatus == "A")
         .OrderByDescending(f => f.CodigoFichaCalificacionDetalle)  // Ordenar de mayor a menor
         .Select(f =>
             f.CodigoFichaCalificacionDetalle
         )
         .FirstOrDefaultAsync();  // Obtener el primero (que es el mayor)

        var cursosDeFichas = await _context.CursoFichaCalificacions
        .Where(a => a.CodigoFichaCalificacionDetalle == codigoUltimoBloqueFicha && a.Estatus == "A")
        .Select(a => a.CodigoCurso)
        .ToListAsync();

        var cursos = await _context.Cursos
       .Where(a => cursosDeFichas.Contains(a.CodigoCurso) && a.Estatus == "A")
       .ToListAsync();

        return cursos;
    }

    public async Task<IEnumerable<ImagenesFichaDto>?> ObtenerImagenesFicha(int codigoFichaCalificacion)
    {
        var imagenesBloques = await _context.FichaCalificacionDetalles
         .Where(f => f.CodigoFichaCalificacion == codigoFichaCalificacion && f.Estatus == "A")
         .OrderByDescending(f => f.CodigoFichaCalificacionDetalle)  // Ordenar de mayor a menor
         .Select(f => new ImagenesFichaDto
         {
             NumeroBloque = f.Bloque,
             CodigoFichaCalificacionDetalle = f.CodigoFichaCalificacionDetalle,
             ImgEstudiante = f.ImgEstudiante,
             ImgCarta = f.ImgCarta,
             ImgFichaCalificacion = f.ImgFichaCalificacion
         }
         )
         .ToListAsync();  // Obtener el primero (que es el mayor)

        return imagenesBloques;
    }

    public async Task<InformacionNuevoBloqueFichaDto> ObtenerBloquesYCursos(int codigoFichaCalificacion)
    {
        // Obtener los códigos de los bloques relacionados a la ficha escolar
        var bloques = await _context.FichaCalificacionDetalles
            .Where(f => f.CodigoFichaCalificacion == codigoFichaCalificacion && f.Estatus == "A")
            .Select(f => new
            {
                f.CodigoFichaCalificacionDetalle,
                f.Bloque,
                f.Promedio,
                f.CodigoPromedio
            })
            .ToListAsync();

        // Obtener los cursos relacionados a los bloques
        var cursosPorBloque = await _context.CursoFichaCalificacions
            .Where(cfc => bloques.Select(b => b.CodigoFichaCalificacionDetalle)
                         .Contains(cfc.CodigoFichaCalificacionDetalle ?? 0) && cfc.Estatus == "A") // Filtrar por bloques
            .Join(_context.Cursos, // Hacer join con la tabla de Cursos para obtener los nombres de los cursos
                cfc => cfc.CodigoCurso,
                c => c.CodigoCurso,
                (cfc, c) => new
                {
                    cfc.CodigoFichaCalificacionDetalle,
                    c.NombreCurso,
                    cfc.CodigoCurso,
                    cfc.Nota
                })
            .ToListAsync();

        // Organizar la información en una lista de BloqueDto
        var listaDeBloques = bloques
            .Select(bloque => new BloqueDto
            {
                Bloque = bloque.Bloque,
                Promedio = (float)(bloque.Promedio ?? 0.0m),
                CodigoPromedio = bloque.CodigoPromedio,
                Materias = cursosPorBloque
                    .Where(c => c.CodigoFichaCalificacionDetalle == bloque.CodigoFichaCalificacionDetalle)
                    .ToDictionary(c => c.NombreCurso, c => c.Nota.ToString()) // Mapear nombre del curso con su nota
            })
            .ToList();

        // Retornar la estructura con los bloques y sus cursos
        var fichaCalificacion = new InformacionNuevoBloqueFichaDto
        {
            Bloques = listaDeBloques
        };

        return fichaCalificacion;
    }

    public async Task<InformacionActualizarFichaDto> ObtenerBloquesYCursosActualizarFicha(int codigoFichaCalificacion)
    {
        var bloques = await _context.FichaCalificacionDetalles
            .Where(f => f.CodigoFichaCalificacion == codigoFichaCalificacion && f.Estatus == "A")
            .Select(f => new
            {
                f.CodigoFichaCalificacionDetalle,
                f.Bloque,
                f.Promedio,
                f.CodigoPromedio
            })
            .ToListAsync();

        var cursosPorBloque = await _context.CursoFichaCalificacions
            .Where(cfc => bloques.Select(b => b.CodigoFichaCalificacionDetalle)
                         .Contains(cfc.CodigoFichaCalificacionDetalle ?? 0) && cfc.Estatus == "A")
            .Join(_context.Cursos,
                cfc => cfc.CodigoCurso,
                c => c.CodigoCurso,
                (cfc, c) => new
                {
                    cfc.CodigoFichaCalificacionDetalle,
                    c.NombreCurso,
                    cfc.CodigoCursoFichaCalificacion,
                    cfc.Nota,
                    cfc.CodigoCurso
                })
            .ToListAsync();

        var listaDeBloques = bloques
            .Select(bloque => new BloquesDto
            {
                CodigoFichaCalificacionDetalle = bloque.CodigoFichaCalificacionDetalle,
                Bloque = bloque.Bloque,
                Promedio = (float)(bloque.Promedio ?? 0.0m),
                CodigoPromedio = bloque.CodigoPromedio,
                Materias = cursosPorBloque
                    .Where(c => c.CodigoFichaCalificacionDetalle == bloque.CodigoFichaCalificacionDetalle)
                    .Select(c => new MateriaDto
                    {
                        NombreCurso = c.NombreCurso,
                        Nota = c.Nota.ToString(),
                        CodigoCursoFichaCalificacion = c.CodigoCursoFichaCalificacion,
                        CodigoCurso = c.CodigoCurso ?? 0
                    })
                    .ToList() // Cambiado a ToList()
            })
            .ToList();

        var fichaCalificacion = new InformacionActualizarFichaDto
        {
            Bloques = listaDeBloques
        };

        return fichaCalificacion;
    }



    public async Task<IEnumerable<int>> ObtnerBloquesDeFichaEscolar(int codigoFichaCalificacion)
    {
        var resultados = await _context.FichaCalificacionDetalles
            .Where(a => a.CodigoFichaCalificacion == codigoFichaCalificacion)
            .Select(a =>
                a.CodigoFichaCalificacionDetalle
            )
            .ToListAsync();

        return resultados;
    }



    public async Task<int> CantidadTotalRegistros()
    {
        return await _context.FichaCalificacions.CountAsync();
    }

    //CANTIDA DE REGISTROS POR NIVELES - GRÁFICA
    public async Task<IEnumerable<RegistroPorNivelAcademico>> CantidadRegistrosPorNivelAcademico(int año)
    {
        var resultados = await _context.FichaCalificacions
            .Where(f => f.CicloEscolar.HasValue && f.CicloEscolar.Value.Year == año)
            .GroupBy(f => new {
                f.CodigoNivelAcademico,
                f.CodigoNivelAcademicoNavigation.NombreNivelAcademico
            })
            .Select(g => new RegistroPorNivelAcademico
            {
                CodigoNivelAcademico = g.Key.CodigoNivelAcademico,
                NivelAcademico = g.Key.NombreNivelAcademico, // Utiliza una función para obtener el nombre
                Cantidad = g.Count()
            })
            .OrderBy(f => f.CodigoNivelAcademico)
            .ToListAsync();

        return resultados;
    }

   




    public async Task<EstudianteDto?> GetByIdDto(int id) //Rol? = Indica que devuelve un objeto rol o un null
    {
        return await _context.Estudiantes
        .Where(a => a.CodigoEstudiante == id)
        .Select(a => new EstudianteDto
        {
            CodigoEstudiante = a.CodigoEstudiante,
            NombreEstudiante = a.NombreEstudiante,
            ApellidoEstudiante = a.ApellidoEstudiante,
            FechaNacimieto = a.FechaNacimieto,
            Genero = a.Genero,
            Estado = a.Estado,
            TelefonoEstudiante = a.TelefonoEstudiante,
            Email = a.Email,
            Comunidad = a.CodigoComunidadNavigation != null ? a.CodigoComunidadNavigation.NombreComunidad : "",
            Sector = a.Sector,
            NumeroCasa = a.NumeroCasa,
            Descripcion = a.Descripcion,
            NivelAcademico = a.CodigoNivelAcademicoNavigation != null ? a.CodigoNivelAcademicoNavigation.NombreNivelAcademico : "",
            Grado = a.CodigoGradoNavigation != null ? a.CodigoGradoNavigation.NombreGrado : "",
            Carrera = a.CodigoCarreraNavigation != null ? a.CodigoCarreraNavigation.NombreCarrera : "",
            Establecimiento = a.CodigoEstablecimientoNavigation != null ? a.CodigoEstablecimientoNavigation.NombreEstablecimiento : "",
            NombrePadre = a.NombrePadre,
            TelefonoPadre = a.TelefonoPadre,
            OficioPadre = a.OficioPadre,
            NombreMadre = a.NombreMadre,
            TelefonoMadre = a.TelefonoMadre,
            OficioMadre = a.OficioMadre,
            FotoPerfil = a.FotoPerfil,
            FechaRegistro = a.FechaRegistro
        }).SingleOrDefaultAsync();
    }

    //Visualizar inoformación de la ficha
    public async Task<FichaCalificacionOutDto?> GetByIdFichaDto(int id) //Rol? = Indica que devuelve un objeto rol o un null
    {
        return await _context.FichaCalificacions
        .Where(a => a.CodigoFichaCalificacion == id)
        .Select(a => new FichaCalificacionOutDto
        {
            CodigoBecario = a.CodigoEstudiante != null ? a.CodigoEstudianteNavigation.CodigoBecario : "",
            Estudiante = a.CodigoEstudianteNavigation != null ? a.CodigoEstudianteNavigation.NombreEstudiante : "",
            ApellidoEstudiante = a.CodigoEstudianteNavigation != null ? a.CodigoEstudianteNavigation.ApellidoEstudiante : "",
            Establecimiento = a.CodigoEstablecimientoNavigation != null ? a.CodigoEstablecimientoNavigation.NombreEstablecimiento : "",
            NivelAcademico = a.CodigoNivelAcademicoNavigation != null ? a.CodigoNivelAcademicoNavigation.NombreNivelAcademico : "",
            Grado = a.CodigoGradoNavigation.NombreGrado,
            Carrera = a.CodigoCarreraNavigation != null ? a.CodigoCarreraNavigation.NombreCarrera : "",
            CicloEscolar = a.CicloEscolar
        }).SingleOrDefaultAsync();
    }

    //Este metodo se crea para mostrar el nombre del estudiante en el input cuando se edita
    //los datos de la ficha de calificacion
    public async Task<FichaUpdateOutDto?> GetByIdUpdateDto(int id) //Rol? = Indica que devuelve un objeto rol o un null
    {
        return await _context.FichaCalificacions.Where(a => a.CodigoFichaCalificacion == id).Select(a => new FichaUpdateOutDto
        {
            CodigoFichaCalificacion = a.CodigoFichaCalificacion,
            CodigoEstudiante = a.CodigoEstudiante,
            nombreEstudiante = a.CodigoEstudianteNavigation != null ? a.CodigoEstudianteNavigation.NombreEstudiante : "",
            CodigoEstablecimiento = a.CodigoEstablecimiento,
            CodigoNivelAcademico = a.CodigoNivelAcademico,
            CodigoGrado = a.CodigoGrado,
            CodigoCarrera = a.CodigoCarrera,
            CicloEscolar = a.CicloEscolar,
            FechaRegistro = a.FechaRegistro
        }).SingleOrDefaultAsync();
    }

    //Metodo para obtener la información por ID.
    public async Task<FichaCalificacion?> GetById(int id) //Rol? = Indica que devuelve un objeto rol o un null
    {
        return await _context.FichaCalificacions.FindAsync(id);
    }

    //Método para crear nueva FichaCalificacion
    public async Task<FichaCalificacion> Create(FichaCalificacion newFichaCalificacion)
    {
        _context.FichaCalificacions.Add(newFichaCalificacion);
        await _context.SaveChangesAsync();

        return newFichaCalificacion;
    }
    public async Task<FichaCalificacion> UpdateFicha(int codigoFichaCalificacion, FichaCalificacion ficha)
    {
        var existingFicha = await GetById(codigoFichaCalificacion);
        if (existingFicha != null)
        {
            existingFicha.CodigoEstudiante = ficha.CodigoEstudiante;
            existingFicha.CodigoEstablecimiento = ficha.CodigoEstablecimiento;
            existingFicha.CodigoNivelAcademico = ficha.CodigoNivelAcademico;
            existingFicha.CodigoGrado = ficha.CodigoGrado;
            existingFicha.CodigoCarrera = ficha.CodigoCarrera;
            existingFicha.CicloEscolar = ficha.CicloEscolar;
            existingFicha.FechaRegistro = ficha.FechaRegistro;
            existingFicha.CodigoModalidadEstudio = ficha.CodigoModalidadEstudio;
            existingFicha.Estatus = ficha.Estatus;
            await _context.SaveChangesAsync();

        }
        return ficha;
    }

    //Obtener datos de promedios
    public async Task<IEnumerable<Promedio>> RangosDePromedios()
    {
        var resultados = await _context.Promedios
            .ToListAsync();
        return resultados;
    }


    //Metodo para actualizar datos de la ficha
    public async Task Update(int id, FichaCalificacion fichaCalificacion)
    {
        var existingFichaCalificacion = await GetById(id);

        if (existingFichaCalificacion is not null)
        {
            existingFichaCalificacion.CodigoEstudiante = fichaCalificacion.CodigoEstudiante;
            existingFichaCalificacion.CodigoEstablecimiento = fichaCalificacion.CodigoEstablecimiento;
            existingFichaCalificacion.CodigoNivelAcademico = fichaCalificacion.CodigoNivelAcademico;
            existingFichaCalificacion.CodigoGrado = fichaCalificacion.CodigoGrado;
            existingFichaCalificacion.CodigoCarrera = fichaCalificacion.CodigoCarrera;
            existingFichaCalificacion.CicloEscolar = fichaCalificacion.CicloEscolar;
            existingFichaCalificacion.FechaRegistro = fichaCalificacion.FechaRegistro;
            await _context.SaveChangesAsync();
        }
    }
    public async Task UpdatePromedio(float promedio, int codigoPromedio, int codigoFichaDetalle)
    {
        var existingFichaDetalle = await _context.FichaCalificacionDetalles.FindAsync(codigoFichaDetalle);

        if (existingFichaDetalle is not null)
        {
            existingFichaDetalle.Promedio = (decimal?)promedio;
            existingFichaDetalle.CodigoPromedio = codigoPromedio;
            await _context.SaveChangesAsync();
        }
    }
    public async Task ActualizarPerfilEstudiante(int codigoEstudiante, Estudiante estudiante)
    {
        var existingEstudiante = await _context.Estudiantes.FindAsync(codigoEstudiante);

        if (existingEstudiante is not null)
        {
            existingEstudiante.CodigoNivelAcademico = estudiante.CodigoNivelAcademico;
            existingEstudiante.CodigoGrado = estudiante.CodigoGrado;
            existingEstudiante.CodigoCarrera = estudiante.CodigoCarrera;
            existingEstudiante.CodigoEstablecimiento = estudiante.CodigoEstablecimiento;
            existingEstudiante.CodigoModalidadEstudio = estudiante.CodigoModalidadEstudio;
            await _context.SaveChangesAsync();
        }
    }
    //Metodo para elminar una fichaCalificacion
    public async Task Delete(int id)
    {
        var fichaCalificacionToDelete = await GetById(id);

        if (fichaCalificacionToDelete is not null)
        {
            _context.FichaCalificacions.Remove(fichaCalificacionToDelete);
            await _context.SaveChangesAsync();
        }
    }

}

public class RegistroPorNivelAcademico
{
    public int CodigoNivelAcademico {get; set;}
    public string? NivelAcademico { get; set; }
    public int Cantidad { get; set; }
}