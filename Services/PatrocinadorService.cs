using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sistemaQuchooch.Data;
using sistemaQuchooch.Data.DTOs;
using sistemaQuchooch.Data.QuchoochModels;


namespace sistemaQuchooch.Sevices;

public class PatrocinadorService
{
    private readonly QuchoochContext _context;

    //constructor
    public PatrocinadorService(QuchoochContext context)
    {
        _context = context;
    }

    //Metodo para listar patrocinadores con paginación
    public async Task<IEnumerable<PatrocinadorOutAllDto>> GetAll(int pagina, int elementosPorPagina, int codigoPatrocinador)
    {
        var Patrocinadors = await _context.Patrocinadors
        .Where(a => codigoPatrocinador == 0 || a.CodigoPatrocinador == codigoPatrocinador)
        .OrderByDescending(a => a.CodigoPatrocinador)
        .Select(a => new PatrocinadorOutAllDto
        {
            CodigoPatrocinador = a.CodigoPatrocinador,
            CodigoPais = a.CodigoPais,
            NombrePais = a.CodigoPaisNavigation != null ? a.CodigoPaisNavigation.Nombre : "",
            NombrePatrocinador = a.NombrePatrocinador,
            ApellidoPatrocinador = a.ApellidoPatrocinador,
            Profesion = a.Profesion,
            Estado = a.Estado,
            FechaNacimiento = a.FechaNacimiento,
            FechaCreacion = a.FechaCreacion,
            FotoPerfil = a.FotoPerfil

        }) 
            .Skip((pagina - 1) * elementosPorPagina)
            .Take(elementosPorPagina)
            .ToListAsync();
        return Patrocinadors;
    }

    //Metodo para buscar patrocinador
      public async Task<IEnumerable<PatrocinadorOutAllDto>> SelectAll()
    {
        var Patrocinadors = await _context.Patrocinadors
        .OrderByDescending(a => a.CodigoPatrocinador)
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

        return Patrocinadors;
    }
    //Metodo para buscar patrocinador
      public async Task<IEnumerable<PatrocinadorOutAllDto>> PatrocinadoresPorRangoFecha(RangoFecha model)
    {
        var Patrocinadors = await _context.Patrocinadors
        .Where(a => a.FechaCreacion >= model.FechaInicio && a.FechaCreacion <= model.FechaFin)
        .OrderByDescending(a => a.CodigoPatrocinador)
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

        return Patrocinadors;
    }

    public async Task<int> CantidadTotalRegistros()
    {
        return await _context.Patrocinadors.CountAsync();
    }

    public async Task<PatrocinadorOutAllDto?> GetByIdDto(int id) //Rol? = Indica que devuelve un objeto rol o un null
    {
        return await _context.Patrocinadors.Where(a => a.CodigoPatrocinador == id).Select(a => new PatrocinadorOutAllDto
        {
            CodigoPatrocinador = a.CodigoPatrocinador,
            CodigoPais = a.CodigoPais,
            NombrePais = a.CodigoPaisNavigation != null ? a.CodigoPaisNavigation.Nombre : "",
            NombrePatrocinador = a.NombrePatrocinador,
            ApellidoPatrocinador = a.ApellidoPatrocinador,
            Profesion = a.Profesion,
            Estado = a.Estado,
            FechaNacimiento = a.FechaNacimiento,
            FechaCreacion = a.FechaCreacion,
            FotoPerfil = a.FotoPerfil
        }).SingleOrDefaultAsync();
    }

    //Metodo para obtener la información por ID.
    public async Task<Patrocinador?> GetById(int id) //Usuario? = Indica que devuelve un objeto usuario o un null
    {
        return await _context.Patrocinadors.FindAsync(id);
    }


    //Método para crear nuevo Usuario
    public async Task<Patrocinador> Create(Patrocinador newPatrocinador)
    {
        _context.Patrocinadors.Add(newPatrocinador);
        await _context.SaveChangesAsync();

        return newPatrocinador;
    }

  

    //Metodo para actualizar datos del Patrocinador
    public async Task Update(int codigoPatrocinador, Patrocinador patrocinador)
    {
        var existingPatrocinador = await GetById(codigoPatrocinador);

        if (existingPatrocinador is not null)
        {
            existingPatrocinador.CodigoPais = patrocinador.CodigoPais;
            existingPatrocinador.NombrePatrocinador = patrocinador.NombrePatrocinador;
            existingPatrocinador.ApellidoPatrocinador = patrocinador.ApellidoPatrocinador;
            existingPatrocinador.Profesion = patrocinador.Profesion;
            existingPatrocinador.Estado = patrocinador.Estado;
            existingPatrocinador.FechaNacimiento = patrocinador.FechaNacimiento;
            existingPatrocinador.FechaCreacion = patrocinador.FechaCreacion;
               if(patrocinador.FotoPerfil != null){
                existingPatrocinador.FotoPerfil = patrocinador.FotoPerfil;
            }
            await _context.SaveChangesAsync();
        }
    }

    //Metodo para elminar un rol
      //Método para actualizar estado
    public async Task Delete(int codigoPatrocinador)
    {
        var existingPatrocinador = await GetById(codigoPatrocinador);

        if (existingPatrocinador is not null)
        {
            existingPatrocinador.Estado = "I";
            }
            await _context.SaveChangesAsync();
    }
}