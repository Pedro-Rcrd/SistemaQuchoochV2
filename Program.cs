using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using sistemaQuchooch.Data;
using sistemaQuchooch.Sevices;
using Microsoft.AspNetCore.Authentication.JwtBearer;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

//Nueva cors
//builder.Services.AddCors(options =>
//{
//options.AddPolicy("AllowLocalhost",
//builder =>
//{
// builder.WithOrigins("https://sistema-ficha-calificacion-production.up.railway.app")
//   .AllowAnyHeader()
//   .AllowAnyMethod();
//});
//});


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


//Inicio Cors2
var myOrigins = "_myOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: myOrigins,
    policy =>
    {
        policy.WithOrigins("http://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

//Fin cors2

//Esto es DB CONTEXT
builder.Services.AddSqlServer<QuchoochContext>(builder.Configuration.GetConnectionString("QuchoochContext"));

//Inyectar servicios
builder.Services.AddScoped<RolService>();
builder.Services.AddScoped<UsuarioService>();
builder.Services.AddScoped<LoginService>();
builder.Services.AddScoped<ComunidadService>();
builder.Services.AddScoped<EstablecimientoService>();
builder.Services.AddScoped<GradoService>();
builder.Services.AddScoped<NivelAcademicoService>();
builder.Services.AddScoped<PaisService>();
builder.Services.AddScoped<CursoService>();
builder.Services.AddScoped<ProveedorService>();
builder.Services.AddScoped<FichaCalificacionService>();
builder.Services.AddScoped<IPaisService, PaisServices>();
builder.Services.AddScoped<CarreraService>();
builder.Services.AddScoped<EstudianteService>();
builder.Services.AddScoped<FileUploadService>();
builder.Services.AddScoped<GastoService>();
builder.Services.AddScoped<CompraService>();
builder.Services.AddScoped<FichaCalificacionDetalleService>();
builder.Services.AddScoped<CursoFichaCalificacionService>();
builder.Services.AddScoped<PatrocinadorService>();
builder.Services.AddScoped<EstudiantePatrocinadorService>();




//Para la autenticación
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"])),
        ValidateIssuer = false,
        ValidateAudience = false
    };

});

//Autorización por tipo de rol basado en claims
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Administrador", policy => policy.RequireClaim("Rol", "1"));
});


var app = builder.Build();

//---------------
app.UseCors(myOrigins);

//CORS NUEVA-----------
//app.UseCors("AllowLocalhost");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // global cors policy
    app.UseCors(x => x
        .AllowAnyMethod()
        .AllowAnyHeader()
        .SetIsOriginAllowed(origin => true) // allow any origin 
        .AllowCredentials());
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//Incluir la opcion de autenticación
app.UseAuthentication();
app.UseAuthorization();



app.MapControllers();

app.Run();
