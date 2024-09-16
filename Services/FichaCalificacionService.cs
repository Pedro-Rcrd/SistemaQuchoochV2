using Microsoft.EntityFrameworkCore;
using sistemaQuchooch.Data;
using sistemaQuchooch.Data.QuchoochModels;


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
        var fichasCalificaciones = await _context.FichaCalificacions.Select(a => new FichaCalificacionOutDto
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
       .ToListAsync();

        return fichasCalificaciones;
    }

    public async Task<int> CantidadTotalRegistros()
    {
        return await _context.FichaCalificacions.CountAsync();
    }

    //CANTIDA DE REGISTROS POR NIVELES
public async Task<IEnumerable<RegistroPorNivelAcademico>> CantidadRegistrosPorNivelAcademico(int año)
{
    var resultados = await _context.FichaCalificacions
        .Where(f => f.CicloEscolar.HasValue && f.CicloEscolar.Value.Year == año)
        .GroupBy(f => f.CodigoNivelAcademico)
        .Select(g => new RegistroPorNivelAcademico
        {
            NivelAcademico = GetNivelAcademicoLabel(g.Key), // Utiliza una función para obtener el nombre
            Cantidad = g.Count()
        })
        .ToListAsync();

    return resultados;
}

private static  string GetNivelAcademicoLabel(int codigoNivelAcademico)
{
    // Realiza la asignación de códigos a etiquetas aquí
    switch (codigoNivelAcademico)
    {
        case 1:
            return "Primaria";
        case 2:
            return "Básico";
        case 3:
            return "Diversificado";
        case 4:
            return "Universitario";
        default:
            return "Desconocido";
    }
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
    public string NivelAcademico { get; set; }
    public int Cantidad { get; set; }
}