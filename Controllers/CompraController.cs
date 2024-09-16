using Microsoft.AspNetCore.Mvc;
using sistemaQuchooch.Sevices;
using sistemaQuchooch.Data.QuchoochModels;
using sistemaQuchooch.Data.DTOs;
using Microsoft.AspNetCore.Authorization;


namespace sistemaQuchooch.Controllers;


[ApiController]
[Route("api/[controller]")]
public class CompraController : ControllerBase
{
    private readonly FileUploadService _fileUploadService;
    private readonly CompraService _compraService;
    private readonly ComunidadService _comunidadService;
    private readonly NivelAcademicoService _nivelAcademicoService;
    private readonly GradoService _gradoService;
    private readonly CarreraService _carreraService;
    private readonly EstablecimientoService _establecimientoService;


    //Constructor
    public CompraController(CompraService service,
                                ComunidadService comunidadService,
                                NivelAcademicoService nivelAcademicoService,
                                GradoService gradoService,
                                CarreraService carreraService,
                                EstablecimientoService establecimientoService,
                                FileUploadService fileUploadService)
    {
        _compraService = service;
        _comunidadService = comunidadService;
        _nivelAcademicoService = nivelAcademicoService;
        _gradoService = gradoService;
        _carreraService = carreraService;
        _establecimientoService = establecimientoService;
        _fileUploadService = fileUploadService;
    }


    //Metodo para obtener la lista de Compras
    [HttpGet("getall")]
    public async Task<IActionResult> GetAll(int pagina = 1, int elementosPorPagina = 10, int id = 0)
    {
        var compras = await _compraService.GetAll(pagina, elementosPorPagina, id);

        // Calcula la cantidad total de registros
        var totalRegistros = await _compraService.CantidadTotalRegistros(); // Debes implementar este método en tu servicio

        // Calcula la cantidad total de páginas
        var totalPaginas = (int)Math.Ceiling((double)totalRegistros / elementosPorPagina);

        // Construye un objeto que incluye la lista de países y la información de paginación
        var resultado = new
        {
            Compras = compras,
            PaginaActual = pagina,
            TotalPaginas = totalPaginas,
            TotalRegistros = totalRegistros
        };

        return Ok(resultado);
    }

    [HttpGet("selectAll")]
    public async Task<IEnumerable<CompraOutAllDto>> SelectAll()
    {
        var compras = await _compraService.SelectAll();
        return compras;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CompraOutAllDto>> GetByIdDto(int id)
    {
        var compra = await _compraService.GetByIdDto(id);

        if (compra is null)
        {
            //Es un método para mostrar error explicito
            return CompraNotFound(id);
        }

        return compra;
    }

    [HttpGet("getbyid/{id}")]
    public async Task<ActionResult<OrdenCompra>> GetById(int id)
    {
        var compra = await _compraService.GetById(id);

        if (compra is null)
        {
            //Es un método para mostrar error explicito
            return CompraNotFound(id);
        }

        return compra;
    }

    //Consulta para obtener información de actualización del gasto
    [HttpGet("getcompraupdate/{id}")]
    public async Task<ActionResult<CompraUpdateOutDto>> GetByIdUpdateCompra(int id)
    {
        var compra = await _compraService.GetByIdUpdateDto(id);

        if (compra is null)
        {
            //Es un método para mostrar error explicito
            return CompraNotFound(id);
        }
        return compra;
    }

    //Solo el administrador puede crear Compras

    [HttpPost("create")]
    public async Task<IActionResult> CrearCompra([FromForm] CompraInputImgDto model)
    {
        try
        {
            if (model == null)
            {
                return BadRequest("Los datos de la compra son inválidos.");
            }
            //var img = model.ImgPatrocinador.FileName;
            var imgEstudiante = model.ImgEstudiante;
            string folder = "Becarios";
            string imageUrlEstudiante = "sin imagen";

            if (imgEstudiante != null)
            {
                try
                {
                    var fileEstudiante = Request.Form.Files[0];
                    imageUrlEstudiante = await _fileUploadService.UploadFileAsync(fileEstudiante, folder);
                    if (imageUrlEstudiante == null)
                    {
                        return BadRequest("No se pudo cargar la imagen");
                    }
                }
                catch
                {
                    return BadRequest("No se pudo cargar la imagen correctamente");
                }
            }

            //INSTANCIANDO Compra
            OrdenCompra compra = new OrdenCompra();
            compra.CodigoEstudiante = model.CodigoEstudiante;
            compra.CodigoProveedor = model.CodigoProveedor;
            compra.FechaCreacion = model.FechaCreacion;
            compra.Titulo = model.Titulo;
            compra.Estado = model.Estado;
            compra.PersonaCreacion = model.PersonaCreacion;
            compra.Descripcion = model.Descripcion;
            compra.FechaEntrega = model.FechaEntrega;
            compra.Total = model.Total;
            compra.ImgEstudiante = imageUrlEstudiante;

            var ordenCompraCreada = await _compraService.Create(compra);

            if (model.DetallesCompra != null)
            {
                // Recorrer la lista de detalles de compra
                foreach (var detalle in model.DetallesCompra)
                {
                    //Instancia
                    OrdenCompraDetalle detalleCompra = new OrdenCompraDetalle();
                    detalleCompra.CodigoOrdenCompra = ordenCompraCreada.CodigoOrdenCompra;
                    detalleCompra.NombreProducto = detalle.NombreProducto;
                    detalleCompra.Cantidad = detalle.Cantidad;
                    detalleCompra.Precio = detalle.Precio;

                    //Guardar cada producto
                    await _compraService.CreateDetalle(detalleCompra);
                }
            }
            return Ok("Orden de compra creado exitosamente.");

        }
        catch
        {
            return BadRequest();

        }
    }

    [HttpPut("updateimg/{id}")]
    public async Task<IActionResult> ModificarCompra([FromForm] CompraInputImgDto model, int id)
    {
        try
        {
            if (model == null)
            {
                return BadRequest("Los datos del Patrocinador son inválidos.");
            }

            //var img = model.ImgPatrocinador.FileName;
            var imgEstudiante = model.ImgEstudiante;
            string folder = "Becarios";
            string imageUrlEstudiante = null;

            if (imgEstudiante != null)
            {
                var fileEstudiante = Request.Form.Files[0];
                imageUrlEstudiante = await _fileUploadService.UploadFileAsync(fileEstudiante, folder);
            }

            //INSTANCIANDO Compra
            OrdenCompra compra = new OrdenCompra();
            compra.CodigoEstudiante = model.CodigoEstudiante;
            compra.CodigoProveedor = model.CodigoProveedor;
            compra.FechaCreacion = model.FechaCreacion;
            compra.Titulo = model.Titulo;
            compra.Estado = model.Estado;
            compra.PersonaCreacion = model.PersonaCreacion;
            compra.Descripcion = model.Descripcion;
            compra.FechaEntrega = model.FechaEntrega;
            compra.Total = model.Total;
            compra.ImgEstudiante = imageUrlEstudiante;

            await _compraService.Create(compra);

            return Ok("Orden de compra exitosamente.");

        }
        catch
        {
            return BadRequest();

        }
    }



    //Solo el administrador puede actualizar Estudiantes
    //[Authorize(Policy = "Administrador")]
    [HttpPut("update/{id}")]
    public async Task<IActionResult> Update(int id, CompraInputDto compraInputDto)
    {
        var compraToUpdate = await _compraService.GetById(id);

        if (compraToUpdate is not null)
        {
            //INSTANCIANDO Compra
            OrdenCompra compra = new OrdenCompra();
            compra.CodigoEstudiante = compraInputDto.CodigoEstudiante;
            compra.CodigoProveedor = compraInputDto.CodigoProveedor;
            compra.FechaCreacion = compraInputDto.FechaCreacion;
            compra.Titulo = compraInputDto.Titulo;
            compra.Estado = compraInputDto.Estado;
            compra.PersonaCreacion = compraInputDto.PersonaCreacion;
            compra.Descripcion = compraInputDto.Descripcion;
            compra.FechaEntrega = compraInputDto.FechaEntrega;
            compra.Total = compraInputDto.Total;
            compra.ImgEstudiante = compraInputDto.ImgEstudiante;
            await _compraService.Update(id, compra);
            return Ok(new
            {
                status = true,
                message = "Compra modificado correctamente"
            });
        }
        else
        {
            return CompraNotFound(id);
        }
    }


    //Solo el administrador puede eliminar Gastos
    //[Authorize(Policy = "Administrador")]
    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var compraToDelete = await _compraService.GetById(id);

        if (compraToDelete is not null)
        {
            await _compraService.Delete(id);
            return Ok();
        }
        else
        {
            return CompraNotFound(id);
        }
    }

    //Definir un método para indicar el mensaje del NotFound
    public NotFoundObjectResult CompraNotFound(int id)
    {
        return NotFound(new { message = $"La compra con el ID {id} no existe" });
    }


}