using Microsoft.EntityFrameworkCore;
using sistemaQuchooch.Data;
using sistemaQuchooch.Data.DTOs;
using sistemaQuchooch.Data.QuchoochModels;


namespace sistemaQuchooch.Sevices;

public class EstudianteService
{
    private readonly QuchoochContext _context;

    //constructor
    public EstudianteService(QuchoochContext context)
    {
        _context = context;
    }

    //Metodo para obtener toda la información de Estudiantes
    public async Task<IEnumerable<EstudianteDto>> GetAll(int pagina, int elementosPorPagina, int codigoEstudiante)
    {
        var estudiantes = await _context.Estudiantes
        .Where(a => codigoEstudiante == 0 || a.CodigoEstudiante == codigoEstudiante)
        .OrderByDescending(a => a.CodigoEstudiante)
        .Select(a => new EstudianteDto
        {
            CodigoBecario = a.CodigoBecario,
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
        })
            .Skip((pagina - 1) * elementosPorPagina)
            .Take(elementosPorPagina)
            .ToListAsync();
        return estudiantes;
    }
    public async Task<IEnumerable<EstudianteDto>> SelectAll()
    {
        var estudiantes = await _context.Estudiantes
        .Select(a => new EstudianteDto
        {
            CodigoBecario = a.CodigoBecario,
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
        })
        .ToListAsync();

        return estudiantes;
    }

    public async Task<int> CantidadTotalRegistros()
    {
        return await _context.Estudiantes.CountAsync();
    }

    public async Task<EstudianteDto?> Ficha (int codigoEstudiante) //Rol? = Indica que devuelve un objeto rol o un null
    {
        return await _context.Estudiantes
        .Where(a => a.CodigoEstudiante == codigoEstudiante)
        .Select(a => new EstudianteDto
        {
            CodigoBecario = a.CodigoBecario,
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
            ModalidadEstudio = a.CodigoModalidadEstudioNavigation != null ? a.CodigoModalidadEstudioNavigation.NombreModalidadEstudio : "",
            NombrePadre = a.NombrePadre,
            TelefonoPadre = a.TelefonoPadre,
            OficioPadre = a.OficioPadre,
            NombreMadre = a.NombreMadre,
            TelefonoMadre = a.TelefonoMadre,
            OficioMadre = a.OficioMadre,
            FotoPerfil = a.FotoPerfil,
            FechaRegistro = a.FechaRegistro,
        }).SingleOrDefaultAsync();
    }

    //Metodo para obtener la información por ID.
    public async Task<Estudiante?> GetById(int id) //Usuario? = Indica que devuelve un objeto usuario o un null
    {
        return await _context.Estudiantes.FindAsync(id);
    }


    //Método para crear nuevo Usuario
    public async Task<Estudiante> Create(Estudiante newEstudiante)
    {
        _context.Estudiantes.Add(newEstudiante);
        await _context.SaveChangesAsync();

        return newEstudiante;
    }

    //Metodo para actualizar datos del Estudiante
    public async Task Update(int id, Estudiante estudiante)
    {
        var existingEstudiante = await GetById(id);

        if (existingEstudiante is not null)
        {
            existingEstudiante.NombreEstudiante = estudiante.NombreEstudiante;
            existingEstudiante.ApellidoEstudiante = estudiante.ApellidoEstudiante;
            existingEstudiante.FechaNacimieto = estudiante.FechaNacimieto;
            existingEstudiante.Genero = estudiante.Genero;
            existingEstudiante.Estado = estudiante.Estado;
            existingEstudiante.TelefonoEstudiante = estudiante.TelefonoEstudiante;
            existingEstudiante.Email = estudiante.Email;
            existingEstudiante.CodigoComunidad = estudiante.CodigoComunidad;
            existingEstudiante.Sector = estudiante.Sector;
            existingEstudiante.NumeroCasa = estudiante.NumeroCasa;
            existingEstudiante.Descripcion = estudiante.Descripcion;
            existingEstudiante.CodigoNivelAcademico = estudiante.CodigoNivelAcademico;
            existingEstudiante.CodigoGrado = estudiante.CodigoGrado;
            existingEstudiante.CodigoCarrera = estudiante.CodigoCarrera;
            existingEstudiante.CodigoEstablecimiento = estudiante.CodigoEstablecimiento;
            existingEstudiante.NombrePadre = estudiante.NombrePadre;
            existingEstudiante.TelefonoPadre = estudiante.TelefonoPadre;
            existingEstudiante.OficioPadre = estudiante.OficioPadre;
            existingEstudiante.NombreMadre = estudiante.NombreMadre;
            existingEstudiante.TelefonoMadre = estudiante.TelefonoMadre;
            existingEstudiante.OficioMadre = estudiante.OficioMadre;
            existingEstudiante.CodigoModalidadEstudio = estudiante.CodigoModalidadEstudio;
            if (!string.IsNullOrEmpty(estudiante.FotoPerfil))
            {
                existingEstudiante.FotoPerfil = estudiante.FotoPerfil;
            }
            existingEstudiante.FechaRegistro = estudiante.FechaRegistro;
            existingEstudiante.CodigoBecario = estudiante.CodigoBecario;
            await _context.SaveChangesAsync();
        }
    }

    //Metodo para elminar un rol
    public async Task Delete(int id)
    {
        var estudianteToDelete = await GetById(id);

        if (estudianteToDelete is not null)
        {
            _context.Estudiantes.Remove(estudianteToDelete);
            await _context.SaveChangesAsync();
        }
    }
}