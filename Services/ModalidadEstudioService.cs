using Microsoft.EntityFrameworkCore;
using sistemaQuchooch.Data;
using sistemaQuchooch.Data.DTOs;
using sistemaQuchooch.Data.QuchoochModels;


namespace sistemaQuchooch.Sevices;

public class ModalidadEstudioService
{
    private readonly QuchoochContext _context;

    //constructor
    public ModalidadEstudioService(QuchoochContext context)
    {
        _context = context;
    }

    //Metodo para obtener toda la información de modalidades
    
    public async Task<IEnumerable<ModalidadEstudio>> SelectAll()
    {
        var modalidades = await _context.ModalidadEstudios.ToListAsync();
        return modalidades;
    }


    //Metodo para obtener la información por ID.
    public async Task<ModalidadEstudio?> GetById(int codigoModalidadEstudio) //Usuario? = Indica que devuelve un objeto usuario o un null
    {
        return await _context.ModalidadEstudios.FindAsync(codigoModalidadEstudio);
    }


    //Método para crear nueva modalidad
    public async Task<ModalidadEstudio> Create(ModalidadEstudio newModalidadEstudio)
    {
        _context.ModalidadEstudios.Add(newModalidadEstudio);
        await _context.SaveChangesAsync();

        return newModalidadEstudio;
    }

    //Metodo para actualizar datos modalidadEstudio
    public async Task Update(int codigoModalidadEstudio, ModalidadEstudio modalidadEstudio)
    {
        var existingModalidadEstudio = await GetById(codigoModalidadEstudio);

        if (existingModalidadEstudio is not null)
        {
            existingModalidadEstudio.NombreModalidadEstudio = modalidadEstudio.NombreModalidadEstudio;
            existingModalidadEstudio.Estatus = modalidadEstudio.Estatus;
            await _context.SaveChangesAsync();
        }
    }

    //Metodo para elminar un modalidad
    public async Task Delete(int codigoModalidadEstudio)
    {
        var modalidadEstudioToDelete = await GetById(codigoModalidadEstudio);

        if (modalidadEstudioToDelete is not null)
        {
            modalidadEstudioToDelete.Estatus = "I";
            await _context.SaveChangesAsync();
        }
    }
}