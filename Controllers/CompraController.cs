using Microsoft.AspNetCore.Mvc;
using sistemaQuchooch.Sevices;
using sistemaQuchooch.Data.QuchoochModels;
using sistemaQuchooch.Data.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;


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
    private readonly ConvertirImagenBase64Service _convertirImagenBase64Service;


    //Constructor
    public CompraController(CompraService service,
                                ComunidadService comunidadService,
                                NivelAcademicoService nivelAcademicoService,
                                GradoService gradoService,
                                CarreraService carreraService,
                                EstablecimientoService establecimientoService,
                                FileUploadService fileUploadService,
                                 ConvertirImagenBase64Service convertirImagenBase64Service)
    {
        _compraService = service;
        _comunidadService = comunidadService;
        _nivelAcademicoService = nivelAcademicoService;
        _gradoService = gradoService;
        _carreraService = carreraService;
        _establecimientoService = establecimientoService;
        _fileUploadService = fileUploadService;
        _convertirImagenBase64Service = convertirImagenBase64Service;
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

    [HttpGet("buscarPorRangoFecha")]
    public async Task<IEnumerable<CompraOutAllDto>> BuscarPorRangoFecha([FromQuery] DateTime fechaInicio, [FromQuery] DateTime fechaFin)
    {
        var model = new RangoFecha { FechaInicio = fechaInicio, FechaFin = fechaFin };
        var compras = await _compraService.ComprasPorRangoFecha(model);
        return compras;
    }

    [HttpGet("listaProductos/{codigoCompra}")]
    public async Task<IEnumerable<CompraDetalle>> ListaProductos(int codigoCompra)
    {
        var compras = await _compraService.ListaProductos(codigoCompra);
        return compras;
    }

    [HttpGet("ficha/{id}")]
    public async Task<ActionResult<CompraOutAllDto>> GetByIdDto(int id)
    {
        var compra = await _compraService.GetByIdDto(id);

        if (!string.IsNullOrEmpty(compra.ImgEstudiante) && compra.ImgEstudiante.Length > 20)
        {
            var imgEstudianteBase64 = await _convertirImagenBase64Service.ConvertirImagenBase64(compra.ImgEstudiante);
            if (imgEstudianteBase64 != null)
            {
                compra.ImgEstudiante = imgEstudianteBase64;
            }
        }

        if (compra is null)
        {
            //Es un método para mostrar error explicito
            return CompraNotFound(id);
        }

        return compra;
    }

    [HttpGet("{codigoCompra}")]
    public async Task<ActionResult<CompraOutAllDto>> GetById(int codigoCompra)
    {
        var compra = await _compraService.GetByIdDto(codigoCompra);

        if (compra is null)
        {
            //Es un método para mostrar error explicito
            return CompraNotFound(codigoCompra);
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
            if (model == null || model.Productos == null)
            {
                return BadRequest("Los datos de la compra no son válidos.");
            }
            //var img = model.ImgPatrocinador.FileName;
            var imgEstudiante = model.ImgEstudiante;
            string folder = "Compras";
            string imageUrlEstudiante = "sin imagen";

            if (imgEstudiante != null)
            {
                var fileEstudiante = Request.Form.Files.FirstOrDefault(f => f.Name == nameof(model.ImgEstudiante));
                if (fileEstudiante != null)
                {
                    imageUrlEstudiante = await _fileUploadService.UploadFileAsync(fileEstudiante, folder);
                }
            }

            //INSTANCIANDO Compra
            OrdenCompra compra = new OrdenCompra()
            {
                CodigoEstudiante = model.CodigoEstudiante,
                CodigoProveedor = model.CodigoProveedor,
                FechaCreacion = model.FechaCreacion,
                Titulo = model.Titulo,
                Estado = model.Estado,
                PersonaCreacion = model.PersonaCreacion,
                Descripcion = model.Descripcion,
                FechaEntrega = model.FechaEntrega,
                Total = model.Total,
                ImgEstudiante = imageUrlEstudiante
            };


            var ordenCompraCreada = await _compraService.Create(compra);
            int codigoNuevoOrdenCompra = ordenCompraCreada.CodigoOrdenCompra;

            // Recorrer la lista de detalles de compra
            foreach (var detalle in model.Productos)
            {
                //Instancia
                //Guardar cada producto
                detalle.CodigoOrdenCompra = codigoNuevoOrdenCompra;
                detalle.Estatus = "A";
                await _compraService.CrearProducto(detalle);
            }

            return Ok(new { status = true, message = "La orden de compra fue creado correctamente." });
        }
        catch
        {
            return BadRequest(new { status = false, message = "Hubo un erro al intentar crear la orden de compra." });

        }
    }
    [HttpPut("update/{codigoCompra}")]
    public async Task<IActionResult> ActualizarCompra(int codigoCompra, [FromForm] CompraInputImgDto model)
    {
        try
        {
            if (model == null)
            {
                return BadRequest("Los datos de la compra no son válidos.");
            }
            //var img = model.ImgPatrocinador.FileName;
            var imgEstudiante = model.ImgEstudiante;
            string folder = "Compras";
            string imageUrlEstudiante = null;

            if (imgEstudiante != null)
            {
                var fileEstudiante = Request.Form.Files.FirstOrDefault(f => f.Name == nameof(model.ImgEstudiante));
                if (fileEstudiante != null)
                {
                    imageUrlEstudiante = await _fileUploadService.UploadFileAsync(fileEstudiante, folder);
                }
            }

            //INSTANCIANDO Compra
            OrdenCompra compra = new OrdenCompra()
            {
                CodigoEstudiante = model.CodigoEstudiante,
                CodigoProveedor = model.CodigoProveedor,
                FechaCreacion = model.FechaCreacion,
                Titulo = model.Titulo,
                Estado = model.Estado,
                PersonaCreacion = model.PersonaCreacion,
                Descripcion = model.Descripcion,
                FechaEntrega = model.FechaEntrega,
                Total = model.Total,
                ImgEstudiante = imageUrlEstudiante
            };


            await _compraService.Update(codigoCompra, compra);

            // Recorrer la lista de detalles de compra
            if (!model.Productos.IsNullOrEmpty())
            {
                foreach (var detalle in model.Productos)
                {
                    //Guardar cada producto
                    detalle.CodigoOrdenCompra = codigoCompra;
                    detalle.Estatus = "A";
                    await _compraService.CrearProducto(detalle);
                }

            }

            return Ok(new { status = true, message = "La orden de compra fue creado correctamente." });
        }
        catch
        {
            return BadRequest(new { status = false, message = "Hubo un erro al intentar crear la orden de compra." });

        }
    }

    [HttpPut("actualizarProducto/{codigoProducto}")]
    public async Task<IActionResult> ActualizarProducto(int codigoProducto, CompraDetalle model)
    {
        try
        {
            await _compraService.ActualizarProducto(codigoProducto, model);

            return Ok(new { status = true, message = "El producto fue actualizado correctamente." });

        }
        catch
        {
            return BadRequest(new { status = false, message = "Hubo un error al intentar actualizar el producto." });

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
    [HttpPut("updateId/{id}")]
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
    [HttpDelete("deleteProducto/{codigoProducto}")]
    public async Task<IActionResult> DeleteProducto(int codigoProducto)
    {
        try
        {
            await _compraService.Delete(codigoProducto);
            return Ok(new { status = true, message = "El producto se ha eliminado correctamente" });
        }
        catch
        {
            return NotFound(new { status = false, message = "Hubo un errro al intentar eliminar el producto." });
        }

    }

    //Definir un método para indicar el mensaje del NotFound
    public NotFoundObjectResult CompraNotFound(int id)
    {
        return NotFound(new { message = $"La compra con el ID {id} no existe" });
    }


}