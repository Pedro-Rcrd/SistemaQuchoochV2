using Microsoft.AspNetCore.Mvc;
using sistemaQuchooch.Sevices;
using sistemaQuchooch.Data.QuchoochModels;
using sistemaQuchooch.Data.DTOs;
using Microsoft.AspNetCore.Authorization;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace sistemaQuchooch.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class UsuarioController : ControllerBase
{
    private readonly UsuarioService _service;
    private readonly RolService _rolService;

    //Constructor
    public UsuarioController(UsuarioService service, RolService rolService)
    {
        _service = service;
        _rolService = rolService;
    }
    [HttpGet("getsha256")]
    public IActionResult metodoPwd(string plainText)
    {
        string hash = GetSHA256(plainText);
        return Ok(hash);
    }

    //Metodo para obtener la lista de usuarios
    [HttpGet("getall")]
    public async Task<IActionResult> GetAll(int pagina = 1, int elementosPorPagina = 10)
    {
        var usuarios = await _service.GetAll(pagina, elementosPorPagina);

        // Calcula la cantidad total de registros
        var totalRegistros = await _service.CantidadTotalRegistros(); // Debes implementar este método en tu servicio

        // Calcula la cantidad total de páginas
        var totalPaginas = (int)Math.Ceiling((double)totalRegistros / elementosPorPagina);

        // Construye un objeto que incluye la lista de países y la información de paginación
        var resultado = new
        {
            Usuarios = usuarios,
            PaginaActual = pagina,
            TotalPaginas = totalPaginas,
            TotalRegistros = totalRegistros
        };

        return Ok(resultado);
    }

    [HttpGet("selectall")]
    public async Task<IEnumerable<UsuarioDtoOut>> SelectAll()
    {

        //Obtener permisos del rol
        //var permisos = await _rolService.GetInfoRol(Convert.ToInt32(codigoRol));

        return await _service.SelectAll();
    }


    [HttpGet("userInformation/{codigoUsuario}")]
    public async Task<IActionResult> GetUsuario(int codigoUsuario)
    {
        var usuario = await _service.FichaUsuario(codigoUsuario);

        if (usuario == null)
        {
            return NotFound();
        }

        return Ok(usuario);
    }


    [HttpGet("{id}")]
    public async Task<ActionResult<UsuarioDtoOut>> GetById(int id)
    {
        var usuario = await _service.GetDtoById(id);

        if (usuario is null)
        {
            //Es un método para mostrar error explicito
            return UsuarioNotFound(id);
        }

        return usuario;
    }

    //Solo el administrador puede crear usuarios
    [Authorize(Policy = "Administrador")]
    [HttpPost("create")]
    public async Task<IActionResult> Create(Usuario usuario)
    {
        string validationResult = await ValidateNewUser(usuario);

        if (!validationResult.Equals("valid"))
        {
            return BadRequest(new
            {
                status = false,
                message = validationResult
            });
        }
        string hashPwd = GetSHA256(usuario.Contrasenia);
        usuario.Contrasenia = hashPwd;
        var newUsuario = await _service.Create(usuario);

        return Ok(new
        {
            status = true,
            message = "Usuario creado correctamente"
        });
        //CreatedAtAction(nameof(GetById), new {id = newUsuario.CodigoUsuario}, newUsuario);
    }


    //Solo el administrador puede actualizar usuarios
    [Authorize(Policy = "Administrador")]
    [HttpPut("update/{id}")]
    public async Task<IActionResult> Update(int id, UsuarioDtoInput usuario)
    {
        var usuarioToUpdate = await _service.GetById(id);
        string validarContraseñaActual = usuarioToUpdate.Contrasenia;

        if (string.IsNullOrWhiteSpace(usuario.ContraseniaNueva) ||
    string.Equals(usuario.ContraseniaNueva.Trim(), "null", StringComparison.OrdinalIgnoreCase))
        {
            Console.WriteLine("La nueva contraseña es nula, vacía, contiene solo espacios o contiene 'null' en cualquier formato.");
            usuario.ContraseniaNueva = null;
        }



        string hashPwd = string.Empty;

        if (usuarioToUpdate is not null)
        {
            string validationResult = await ValidateUser(usuario, validarContraseñaActual);
            if (!validationResult.Equals("valid"))
            {
                return BadRequest(new { message = validationResult });
            }

            if (usuario.ContraseniaNueva is not null)
            {
                hashPwd = GetSHA256(usuario.ContraseniaNueva);
                usuario.ContraseniaNueva = hashPwd;
            }
            await _service.Update(id, usuario);
            return Ok(new
            {
                status = true,
                message = "Usuario modificado correctamente"
            });
        }
        else
        {
            return UsuarioNotFound(id);
        }
    }


    //Solo el administrador puede eliminar usuarios
    [Authorize(Policy = "Administrador")]
    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var usuarioToDelete = await _service.GetById(id);

        if (usuarioToDelete is not null)
        {
            await _service.Delete(id);
            return Ok();
        }
        else
        {
            return UsuarioNotFound(id);
        }
    }

    //Definir un método para indicar el mensaje del NotFound
    public NotFoundObjectResult UsuarioNotFound(int id)
    {
        return NotFound(new { message = $"El rol con el ID {id} no existe" });
    }

    public async Task<string> ValidateUser(UsuarioDtoInput usuario, string pwdUser)
    {
        string result = "valid";
        var usuarioRol = usuario.CodigoRol.GetValueOrDefault(); //Para validar que el codigoRol no sea Nulo
        var tipoRol = await _rolService.GetById(usuarioRol);
        string hashPwdActual = GetSHA256(usuario.Contrasenia);
        if (pwdUser != hashPwdActual)
        {
            result = "La contraseña actual es incorrecta. ";
        }
        if (tipoRol is null)
        {
            result += $"El rol {tipoRol} no existe";
        }
        return result;
    }
    //Validador para crear nuevo usuario
    public async Task<string> ValidateNewUser(Usuario usuario)
    {

        string result = "valid";
        var usuarioRol = usuario.CodigoRol.GetValueOrDefault(); //Para validar que el codigoRol no sea Nulo
        var tipoRol = await _rolService.GetById(usuarioRol);

        if (tipoRol is null)
        {
            result = $"El rol {tipoRol} no existe";
        }
        return result;
    }

    //Método de encriptación
    public static string GetSHA256(string plainText)
    {
        SHA256 sha256 = SHA256Managed.Create();
        ASCIIEncoding encoding = new ASCIIEncoding();
        byte[] stream = null;
        StringBuilder sb = new StringBuilder();
        stream = sha256.ComputeHash(encoding.GetBytes(plainText));
        for (int i = 0; i < stream.Length; i++) sb.AppendFormat("{0:x2}", stream[i]);
        return sb.ToString();
    }
}