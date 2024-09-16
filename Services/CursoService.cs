using Microsoft.EntityFrameworkCore;
using sistemaQuchooch.Data;
using sistemaQuchooch.Data.QuchoochModels;


namespace sistemaQuchooch.Sevices;

public class CursoService 
{
    private readonly QuchoochContext _context;

    //constructor
    public CursoService(QuchoochContext context)
    {
        _context = context;
    }

    //Metodo para obtener toda la información de Curso
             public async Task<IEnumerable<Curso>> GetAll(int pagina, int elementosPorPagina)
{
    var cursos = await _context.Cursos
        .Skip((pagina - 1) * elementosPorPagina)
        .Take(elementosPorPagina)
        .ToListAsync();
    return cursos;
}

    public async Task<int> CantidadTotalRegistros()
    {
        return await _context.Cursos.CountAsync();
    }


    //Metodo para obtener la información por ID.
    public async Task<Curso?> GetById(int id) //Rol? = Indica que devuelve un objeto rol o un null
    {
        return await _context.Cursos.FindAsync(id);
    }

    //Método para crear nueva Curso
    public async Task<Curso> Create (Curso newCurso)
    {
        _context.Cursos.Add(newCurso);
        await _context.SaveChangesAsync();

        return newCurso;
    }

    //Metodo para actualizar datos de la ocmunidad
    public async Task Update (int id, Curso curso)
    {
        var existingCurso = await GetById(id);

        if(existingCurso is not null)
        {
            existingCurso.NombreCurso = curso.NombreCurso;
            existingCurso.CodigoNivelAcademico = curso.CodigoNivelAcademico;

            await _context.SaveChangesAsync();
        }
    }

    //Metodo para elminar una Curso
    public async Task Delete (int id)
    {
        var cursoToDelete = await GetById(id);

        if(cursoToDelete is not null)
        {
            _context.Cursos.Remove(cursoToDelete);
            await _context.SaveChangesAsync();
        }
    }

}