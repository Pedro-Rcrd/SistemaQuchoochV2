using Microsoft.EntityFrameworkCore;
using sistemaQuchooch.Data;
using sistemaQuchooch.Data.DTOs;
using sistemaQuchooch.Data.QuchoochModels;


namespace sistemaQuchooch.Sevices;

public class CarreraService
{
    private readonly QuchoochContext _context;

    //constructor
    public CarreraService(QuchoochContext context)
    {
        _context = context;
    }

    //Metodo para obtener toda la información de Carrera
    public async Task<IEnumerable<CarreraDto>> GetAll(int pagina, int elementosPorPagina)
    {
        var carreras = await _context.Carreras.Select(a => new CarreraDto{
            CodigoCarrera = a.CodigoCarrera,
            NivelAcademico = a.CodigoNivelAcademicoNavigation != null ? a.CodigoNivelAcademicoNavigation.NombreNivelAcademico : "",
            NombreCarrera = a.NombreCarrera,
        }) .Skip((pagina - 1) * elementosPorPagina)
            .Take(elementosPorPagina)
            .ToListAsync();
        return carreras;
    }

    public async Task<int> CantidadTotalRegistros()
    {
        return await _context.Carreras.CountAsync();
    }

    //Metodo para obtener la información por ID.
    public async Task<CarreraDto?> GetByIdDto(int id) //Rol? = Indica que devuelve un objeto rol o un null
    {
        return await _context.Carreras.Where(a => a.CodigoCarrera == id).Select(a => new CarreraDto{
            CodigoCarrera = a.CodigoCarrera,
            NivelAcademico = a.CodigoNivelAcademicoNavigation != null ? a.CodigoNivelAcademicoNavigation.NombreNivelAcademico : "",
            NombreCarrera = a.NombreCarrera
        }).SingleOrDefaultAsync();
    }

      public async Task<Carrera?> GetById(int id) //Rol? = Indica que devuelve un objeto rol o un null
    {
        return await _context.Carreras.FindAsync(id);
    }

    //Método para crear nueva Carrera
    public async Task<Carrera> Create(Carrera newCarrera)
    {
        _context.Carreras.Add(newCarrera);
        await _context.SaveChangesAsync();

        return newCarrera;
    }

    //Metodo para actualizar datos de la ocmunidad
    public async Task Update(int id, Carrera carrera)
    {
        var existingCarrera = await GetById(id);

        if (existingCarrera is not null)
        {
            existingCarrera.CodigoNivelAcademico = carrera.CodigoNivelAcademico;
            existingCarrera.NombreCarrera = carrera.NombreCarrera;
            await _context.SaveChangesAsync();
        }
    }

    //Metodo para elminar una Carrera
    public async Task Delete(int id)
    {
        var carreraToDelete = await GetById(id);

        if (carreraToDelete is not null)
        {
            _context.Carreras.Remove(carreraToDelete);
            await _context.SaveChangesAsync();
        }
    }

}