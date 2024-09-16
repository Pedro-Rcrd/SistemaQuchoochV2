using Microsoft.EntityFrameworkCore;
using sistemaQuchooch.Data;
using sistemaQuchooch.Data.DTOs;
using sistemaQuchooch.Data.QuchoochModels;


namespace sistemaQuchooch.Sevices;

public class NivelAcademicoService 
{
    private readonly QuchoochContext _context;

    //constructor
    public NivelAcademicoService(QuchoochContext context)
    {
        _context = context;
    }

    //Metodo para obtener toda la información de NivelAcademico
    public async Task<IEnumerable<NivelAcademico>> GetAll(int pagina, int elementosPorPagina)
    {
        var nivelesAcademicos = await _context.NivelAcademicos
            .Skip((pagina - 1) * elementosPorPagina)
            .Take(elementosPorPagina)
            .ToListAsync();

        return nivelesAcademicos;
    }

    public async Task<int> CantidadTotalRegistros()
    {
        return await _context.NivelAcademicos.CountAsync();
    }

    //Metodo para obtener la información por ID.
    public async Task<NivelAcademico?> GetById(int id) //Rol? = Indica que devuelve un objeto rol o un null
    {
        return await _context.NivelAcademicos.FindAsync(id);
    }

    //Método para crear nueva NivelAcademico
    public async Task<NivelAcademico> Create (NivelAcademico newNivelAcademico)
    {
        _context.NivelAcademicos.Add(newNivelAcademico);
        await _context.SaveChangesAsync();

        return newNivelAcademico;
    }

    //Metodo para actualizar datos de la ocmunidad
    public async Task Update (int id, NivelAcademicoDto nivelAcademicoDto)
    {
        var existingNivelAcademico = await GetById(id);

        if(existingNivelAcademico is not null)
        {
            existingNivelAcademico.NombreNivelAcademico = nivelAcademicoDto.NombreNivelAcademico;
            await _context.SaveChangesAsync();
        }
    }

    //Metodo para elminar una NivelAcademico
    public async Task Delete (int id)
    {
        var nivelAcademicoToDelete = await GetById(id);

        if(nivelAcademicoToDelete is not null)
        {
            _context.NivelAcademicos.Remove(nivelAcademicoToDelete);
            await _context.SaveChangesAsync();
        }
    }

}