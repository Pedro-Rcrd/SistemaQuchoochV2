using Microsoft.EntityFrameworkCore;
using sistemaQuchooch.Data;
using sistemaQuchooch.Data.QuchoochModels;


namespace sistemaQuchooch.Sevices;

public class CursoFichaCalificacionService
{
    private readonly QuchoochContext _context;

    //constructor
    public CursoFichaCalificacionService(QuchoochContext context)
    {
        _context = context;
    }

    //Metodo para obtener toda la información de CursoFichaCalificaciones
    public async Task<IEnumerable<CursoNotaOutDto>> GetByCodigoCurso(List<int?> codigosCurso)
    {
        var cursosNotas = await _context.CursoFichaCalificacions
            .Where(curso => codigosCurso.Contains(curso.CodigoFichaCalificacionDetalle))
            .Select(a => new CursoNotaOutDto
            {
                CodigoFichaCalificacionDetalle = a.CodigoFichaCalificacionDetalle,
                CodigoCursoFichaCalificacion = a.CodigoCursoFichaCalificacion,
                Bloque = a.CodigoFichaCalificacionDetalleNavigation.Bloque,
                Curso = a.CodigoCursoNavigation != null ? a.CodigoCursoNavigation.NombreCurso : "",
                Nota = a.Nota,
                CodigoCurso = a.CodigoCurso

            })
            .ToListAsync();

        return cursosNotas;
    }

    //OBTENER LA LISTA DE CURSOS POR SU CODIGO DE FICHA DETALLE
    public async Task<IEnumerable<CursoNotaOutDto>> ListaCursoPorFichaDetalle(int codigoCurso)
    {
        var cursosNotas = await _context.CursoFichaCalificacions
            .Where(curso => curso.CodigoFichaCalificacionDetalle == codigoCurso)
            .Select(a => new CursoNotaOutDto
            {
                CodigoFichaCalificacionDetalle = a.CodigoFichaCalificacionDetalle,
                CodigoCursoFichaCalificacion = a.CodigoCursoFichaCalificacion,
                Bloque = a.CodigoFichaCalificacionDetalleNavigation.Bloque,
                Curso = a.CodigoCursoNavigation != null ? a.CodigoCursoNavigation.NombreCurso : "",
                Nota = a.Nota,
                CodigoCurso = a.CodigoCurso
            })
            .ToListAsync();

        return cursosNotas;
    }

    public async Task<List<CursoFichaCalificacion>> ObtenerCursosProBloque(int codigoFichaDetalle)
    {
        var cursos = await _context.CursoFichaCalificacions
        .Where(curso => curso.CodigoFichaCalificacionDetalle == codigoFichaDetalle && curso.Estatus == "A")
        .ToListAsync();

        return cursos;
    }



    //Metodo para obtener la información por ID.
    public async Task<CursoFichaCalificacion?> GetById(int id) //Rol? = Indica que devuelve un objeto rol o un null
    {
        return await _context.CursoFichaCalificacions.FindAsync(id);
    }
    //OBTNER EL VALOR MAXIMO DEL CODIGO
    public async Task<int> GetMaxCodigoCursoFichaCalificacion()
    {
        var maxCodigo = await _context.CursoFichaCalificacions.MaxAsync(f => f.CodigoCursoFichaCalificacion);
        return maxCodigo;
    }

    //Método para crear nueva CursoFichaCalificacion
    public async Task<CursoFichaCalificacion> Create(CursoFichaCalificacion newCursoFichaCalificacion)
    {
        _context.CursoFichaCalificacions.Add(newCursoFichaCalificacion);
        await _context.SaveChangesAsync();

        return newCursoFichaCalificacion;
    }

    //Metodo para actualizar datos de la ocmunidad
    public async Task Update(int id, CursoFichaCalificacion cursoFichaCalificacion)
    {
        var existingCursoFichaCalificacion = await GetById(id);

        if (existingCursoFichaCalificacion is not null)
        {
            existingCursoFichaCalificacion.CodigoCurso = cursoFichaCalificacion.CodigoCurso;
            existingCursoFichaCalificacion.Nota = cursoFichaCalificacion.Nota;
            await _context.SaveChangesAsync();
        }
    }

    //Metodo para actualizar datos de la ocmunidad
    public async Task ActualizarNotaCurso(int codigoCursoFichaCalificacion, int notaCalificacion)
    {
        var existingCursoFichaCalificacion = await GetById(codigoCursoFichaCalificacion);

        if (existingCursoFichaCalificacion is not null)
        {
            existingCursoFichaCalificacion.Nota = notaCalificacion;
            await _context.SaveChangesAsync();
        }
    }

    public async Task<int?> EliminarCursoFichaCalifiacion(int codigoCursoFichaCalificacion)
    {
        var existingCursoFichaCalificacion = await GetById(codigoCursoFichaCalificacion);

        if (existingCursoFichaCalificacion is not null)
        {
            existingCursoFichaCalificacion.Estatus = "I";  // Marcado como inactivo
            await _context.SaveChangesAsync();

            // Devuelve el CodigoFichaCalificacionDetalle del curso modificado
            return existingCursoFichaCalificacion.CodigoFichaCalificacionDetalle;
        }

        // Si no se encuentra, devuelve null
        return null;
    }


    //Metodo para elminar una CursoFichaCalificacion
    public async Task Delete(int id)
    {
        var cursoFichaCalificacionToDelete = await GetById(id);

        if (cursoFichaCalificacionToDelete is not null)
        {
            _context.CursoFichaCalificacions.Remove(cursoFichaCalificacionToDelete);
            await _context.SaveChangesAsync();
        }
    }

   
}