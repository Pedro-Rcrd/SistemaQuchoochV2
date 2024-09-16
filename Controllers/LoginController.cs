using Microsoft.AspNetCore.Mvc;
using sistemaQuchooch.Sevices;
using sistemaQuchooch.Data.QuchoochModels;
using sistemaQuchooch.Data.DTOs;
using System.Linq.Expressions;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.AspNetCore.Authorization;
using System.Security.Cryptography;
using System.Text;


namespace sistemaQuchooch.Controllers;

//Incluir aquí la autenticación

[ApiController]
[Route("api/[controller]")]
public class LoginController : ControllerBase
{
    private readonly LoginService _service;
    private readonly RolService _rolService;
    private IConfiguration config;

    //Constructor
    public LoginController(LoginService service, IConfiguration config, RolService rolService)
    {
        _service = service;
        _rolService = rolService;
        this.config = config;
    }

    [HttpPost("authenticate")]
    public async Task<IActionResult> Login(AdminDto adminDto)
    {
        //Hashing de constraseña
        string hashPwd = GetSHA256(adminDto.Contrasenia);
        adminDto.Contrasenia = hashPwd;
        var admin = await _service.GetAdmin(adminDto);

        if (admin is null)
        {
            return BadRequest(new
            {
                status = false,
                message = "Credenciales invalidas"
            });
        }
        //Sí admin no es null, entonces se genera el token
        var nombreUsuario = admin.NombreUsuario;
        var codigoRol = admin.CodigoRol;

        //Obtener permisos del rol
        var permisos = await _service.GetInfoRol(Convert.ToInt32(codigoRol));

        //Generar token
        string jwtToken = GenerateToken(admin);


        return Ok(new
        {
            
            status = true,
            nombreUsuario,
            codigoRol,
            permisos,
            token = jwtToken
        });
    }

    private string GenerateToken(Usuario usuario)
    {
        string emailUsuario = usuario.Email ?? "email";
        //"JWT:Key" es una propiedad configurada en appsettings.json
        string jwtKey = config.GetSection("JWT:Key").Value ?? throw new InvalidOperationException("La clave JWT no está configurada correctamente.");
        //Arreglo para los claims o propiedades de identidad del usuario.
        var claims = new[]
        {
        new Claim(ClaimTypes.Name, emailUsuario),
        new Claim("Rol", usuario.CodigoRol.ToString()) //Claims o información que se integra en 
                                                       //el TOKEN
    };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
        //Encriptación de credenciales
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        var securityToken = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddHours(8), //Tiempo de expiración del token = 8 horas
            signingCredentials: creds);

        string token = new JwtSecurityTokenHandler().WriteToken(securityToken);

        return token;
    }
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