using Microsoft.AspNetCore.Mvc;
using sistemaQuchooch.Sevices;
using sistemaQuchooch.Data.QuchoochModels;
using Microsoft.AspNetCore.Authorization;
using System.Security.Cryptography;
using sistemaQuchooch.Data.DTOs;

namespace sistemaQuchooch.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class RolController : ControllerBase
{
    private readonly RolService _service;

    //Constructor
    public RolController(RolService service)
    {
        _service = service;
    }

    //Metodo para obtener la lista de roles
    [HttpGet("selectall")]
    public async Task<IEnumerable<Rol>> Get()
    {
        return await _service.SelectAll();
    }

    [HttpGet("permisosrol/{codigoRol}")]
    public async Task<IActionResult> GetInfoRol(int codigoRol)
    {
        try
        {
            // Llama al método del servicio
            var permisos = await _service.GetInfoRol(codigoRol);

            // Retorna una respuesta OK con los permisos obtenidos
            return Ok(permisos);
        }
        catch (Exception ex)
        {
            // Manejo de excepciones (puedes personalizar este manejo según tus necesidades)
            return StatusCode(500, $"Se produjo un error al procesar la solicitud: {ex.Message}");
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Rol>> GetById(int id)
    {
        var rol = await _service.GetById(id);

        if (rol is null)
        {
            //Es un método para mostrar error explicito
            return RolNotFound(id);
        }

        return rol;
    }

    [HttpPost("create")]
    public async Task<IActionResult> Create([FromBody] RolPermisoDto rolPermisoDto)
    {
        Console.WriteLine("Solicitud recibida");
        Rol rol = new Rol();
        rol.NombreRol = rolPermisoDto.NombreRol;
        rol.Estatus = rolPermisoDto.Estatus;
        var newRol = await _service.Create(rol);
        int codigoNuevoRol = Convert.ToInt32(newRol.CodigoRol);

        foreach (var permisoDto in rolPermisoDto.Permisos)
        {
            var nuevoPermiso = new Permiso
            {
                CodigoRol = codigoNuevoRol, // ID del rol creado
                CodigoModulo = permisoDto.CodigoModulo,
                CodigoAccion = permisoDto.CodigoAccion
            };
            await _service.CreatePermission(nuevoPermiso);
        }

        return Ok("Rol creado correctamente");
    }

    [HttpPut("update/{codigoRol}")]
    public async Task<IActionResult> Update(int codigoRol, RolPermisoDto rolPermisoDto)
    {
        var updateRol = new Rol()
        {
            NombreRol = rolPermisoDto.NombreRol,
            Estatus = rolPermisoDto.Estatus
        };

        var rolToUpdate = await _service.GetById(codigoRol);
        if (rolToUpdate is not null)
        {
            await _service.Update(codigoRol, updateRol);
            var permisosRolToDelete = await _service.GetByIdPermisosRol(codigoRol);

            if (permisosRolToDelete is not null)
            {
                await _service.DeletePermisosRol(codigoRol);

                foreach (var permisoDto in rolPermisoDto.Permisos)
                {
                    var nuevoPermiso = new Permiso
                    {
                        CodigoRol = codigoRol, // ID del rol creado
                        CodigoModulo = permisoDto.CodigoModulo,
                        CodigoAccion = permisoDto.CodigoAccion
                    };
                    await _service.CreatePermission(nuevoPermiso);
                }
            }
            else
            {
                Console.WriteLine(permisosRolToDelete);
                return RolNotFound(codigoRol);
            }

            return Ok();
        }
        else
        {
            return RolNotFound(codigoRol);
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var rolToDelete = await _service.GetById(id);

        if (rolToDelete is not null)
        {
            await _service.Delete(id);
            return Ok();
        }
        else
        {
            return RolNotFound(id);
        }
    }

    //Definir un método para indicar el mensaje del NotFound
    public NotFoundObjectResult RolNotFound(int id)
    {
        return NotFound(new { message = $"El rol con el ID {id} no existe" });
    }

}