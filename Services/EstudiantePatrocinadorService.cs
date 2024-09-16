using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sistemaQuchooch.Data;
using sistemaQuchooch.Data.DTOs;
using sistemaQuchooch.Data.QuchoochModels;


namespace sistemaQuchooch.Sevices;

public class EstudiantePatrocinadorService
{
    private readonly QuchoochContext _context;

    //constructor
    public EstudiantePatrocinadorService(QuchoochContext context)
    {
        _context = context;
    }

    //Metodo para listar patrocinadores con paginación
    public async Task<IEnumerable<EstudiantePatrocinadorOutAllDto>> GetAll(int idEstudiante, int pagina, int elementosPorPagina)
    {
        var estudiantesPatrocinadores = await _context.EstudiantePatrocinadors
        .Where(a => idEstudiante == 0 || a.CodigoEstudiante == idEstudiante)
        .Select(a => new EstudiantePatrocinadorOutAllDto
        {
            CodigoEstudiantePatrocinador = a.CodigoEstudiantePatrocinador,
            CodigoEstudiante = a.CodigoEstudiante,
            CodigoPatrocinador = a.CodigoPatrocinador,
            NombreEstudiante = a.CodigoEstudianteNavigation != null ? a.CodigoEstudianteNavigation.NombreEstudiante : "",
            ApellidoEstudiante = a.CodigoEstudianteNavigation != null ? a.CodigoEstudianteNavigation.ApellidoEstudiante : "",
            CodigoBecario = a.CodigoEstudianteNavigation != null ? a.CodigoEstudianteNavigation.CodigoBecario : "",
            NombrePatrocinador = a.CodigoPatrocinadorNavigation != null ? a.CodigoPatrocinadorNavigation.NombrePatrocinador : ""
        })
            .Skip((pagina - 1) * elementosPorPagina)
            .Take(elementosPorPagina)
            .ToListAsync();
        return estudiantesPatrocinadores;
    }

    //Metodo para buscar patrocinador
    public async Task<IEnumerable<PatrocinadorOutAllDto>> SelectAll()
    {
        var patrocinadores = await _context.Patrocinadors
        .Select(e => new PatrocinadorOutAllDto
        {
            CodigoPatrocinador = e.CodigoPatrocinador,
            CodigoPais = e.CodigoPais,
            NombrePais = e.CodigoPaisNavigation != null ? e.CodigoPaisNavigation.Nombre : "",
            NombrePatrocinador = e.NombrePatrocinador,
            ApellidoPatrocinador = e.ApellidoPatrocinador,
            Profesion = e.Profesion,
            Estado = e.Estado,
            FechaNacimiento = e.FechaNacimiento,
            FechaCreacion = e.FechaCreacion,
            FotoPerfil = e.FotoPerfil

        })
        .ToListAsync();

        return patrocinadores;
    }

    public async Task<int> CantidadTotalRegistros()
    {
        return await _context.EstudiantePatrocinadors.CountAsync();
    }

    public async Task<EstudiantePatrocinadorOutAllDto?> GetByIdDto(int id) //Rol? = Indica que devuelve un objeto rol o un null
    {
        return await _context.EstudiantePatrocinadors.Where(a => a.CodigoPatrocinador == id).Select(a => new EstudiantePatrocinadorOutAllDto
        {
            CodigoEstudiantePatrocinador = a.CodigoEstudiantePatrocinador,
            CodigoPatrocinador = a.CodigoPatrocinador,
            NombreEstudiante = a.CodigoEstudianteNavigation != null ? a.CodigoEstudianteNavigation.NombreEstudiante : "",
            ApellidoEstudiante = a.CodigoEstudianteNavigation != null ? a.CodigoEstudianteNavigation.ApellidoEstudiante : "",
            CodigoBecario = a.CodigoEstudianteNavigation != null ? a.CodigoEstudianteNavigation.CodigoBecario ?? "" : "",
            NombrePatrocinador = a.CodigoPatrocinadorNavigation != null ? a.CodigoPatrocinadorNavigation.NombrePatrocinador : ""
        }).SingleOrDefaultAsync();
    }

    //Metodo para obtener la información por ID.
    public async Task<EstudiantePatrocinador?> GetById(int id) //Usuario? = Indica que devuelve un objeto usuario o un null
    {
        return await _context.EstudiantePatrocinadors.FindAsync(id);
    }


    //Método para crear nuevo Estudiante Patrocinador
    public async Task<EstudiantePatrocinador> Create(EstudiantePatrocinador newEstudiantePatrocinador)
    {
        _context.EstudiantePatrocinadors.Add(newEstudiantePatrocinador);
        await _context.SaveChangesAsync();

        return newEstudiantePatrocinador;
    }

    //Metodo para actualizar datos del Patrocinador
    public async Task Update(int id, EstudiantePatrocinador estudiantePatrocinador)
    {
        var existingEstudiantePatrocinador = await GetById(id);

        if (existingEstudiantePatrocinador is not null)
        {
            existingEstudiantePatrocinador.CodigoEstudiante = estudiantePatrocinador.CodigoEstudiante;
            existingEstudiantePatrocinador.CodigoPatrocinador = estudiantePatrocinador.CodigoPatrocinador;
            await _context.SaveChangesAsync();
        }
    }

    //Metodo para elminar un rol
    public async Task Delete(int id)
    {
        var estPatrocinadorToDelete = await GetById(id);

        if (estPatrocinadorToDelete is not null)
        {
            _context.EstudiantePatrocinadors.Remove(estPatrocinadorToDelete);
            await _context.SaveChangesAsync();
        }
    }
}