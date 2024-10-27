using Microsoft.AspNetCore.Mvc;
using sistemaQuchooch.Sevices;
using sistemaQuchooch.Data.QuchoochModels;
using sistemaQuchooch.Data.DTOs;
using Microsoft.AspNetCore.Authorization;


namespace sistemaQuchooch.Controllers;


[ApiController]
[Route("api/[controller]")]
public class GastoController : ControllerBase
{
    private readonly FileUploadService _fileUploadService;
    private readonly GastoService _gastoService;
    private readonly CompraService _compraService;
    private readonly ConvertirImagenBase64Service _convertirImagenBase64Service;


    //Constructor
    public GastoController(GastoService service,
                            CompraService compraService,
                            FileUploadService fileUploadService,
                            ConvertirImagenBase64Service convertirImagenBase64Service)
    {
        _gastoService = service;
        _compraService = compraService;
        _fileUploadService = fileUploadService;
         _convertirImagenBase64Service = convertirImagenBase64Service;
    }


    //Metodo para obtener la lista de Gastos
    [HttpGet("getall")]
    public async Task<IActionResult> GetAll(int pagina = 1, int elementosPorPagina = 10, int id = 0)
    {
        var gastos = await _gastoService.GetAll(pagina, elementosPorPagina, id);

        // Calcula la cantidad total de registros
        var totalRegistros = await _gastoService.CantidadTotalRegistros(); // Debes implementar este método en tu servicio

        // Calcula la cantidad total de páginas
        var totalPaginas = (int)Math.Ceiling((double)totalRegistros / elementosPorPagina);

        // Construye un objeto que incluye la lista de países y la información de paginación
        var resultado = new
        {
            Gastos = gastos,
            PaginaActual = pagina,
            TotalPaginas = totalPaginas,
            TotalRegistros = totalRegistros
        };

        return Ok(resultado);
    }

     [HttpGet("selectAll")]
         public async Task<IEnumerable<GastoOutAllDto>> SelectAll()
    {
        var gastos = await _gastoService.SelectAll();
        return gastos;
    }

      [HttpGet("buscarPorRangoFecha")]
    public async Task<IEnumerable<GastoOutAllDto>> BuscarPorRangoFecha([FromQuery] DateTime fechaInicio, [FromQuery] DateTime fechaFin)
    {
        var model = new RangoFecha { FechaInicio = fechaInicio, FechaFin = fechaFin };
        var gastos = await _gastoService.GastosPorRangoFecha(model);
        return gastos;
    }

    //Reporte, cantida de registros
    //Metodo para obtener la lista de Gastos
    [HttpGet("gastos&compras")]
    public async Task<IActionResult> CantidaGastoCompraActivos(int pagina = 1, int elementosPorPagina = 10)
    {
        var cantidadGastos = await _gastoService.CantidadTotalRegistrosActivos();

        // Calcula la cantidad total de registros
        var cantidadCompras = await _compraService.CantidadTotalRegistrosActivos(); // Debes implementar este método en tu servicio

        // Construye un objeto que incluye la lista de países y la información de paginación
        var resultado = new List<object>
    {
        new { bloque = "Compras", cantidad = cantidadCompras },
        new { bloque = "Gastos", cantidad = cantidadGastos }
    };

        return Ok(resultado);
    }

    [HttpGet("ficha/{codigoGasto}")]
    public async Task<ActionResult<GastoOutAllDto>> GetByIdDto(int codigoGasto)
    {
        var gasto = await _gastoService.GetByIdDto(codigoGasto);
         if (!string.IsNullOrEmpty(gasto.ImgEstudiante) && gasto.ImgEstudiante.Length > 20)
            {
                var imgEstudianteBase64 = await _convertirImagenBase64Service.ConvertirImagenBase64(gasto.ImgEstudiante);
                if (imgEstudianteBase64 != null)
                {
                    gasto.ImgEstudiante = imgEstudianteBase64;
                }
            }

             if (!string.IsNullOrEmpty(gasto.ImgCheque) && gasto.ImgCheque.Length > 30)
            {
                var imgChequeBase64 = await _convertirImagenBase64Service.ConvertirImagenBase64(gasto.ImgCheque);
                if (imgChequeBase64 != null)
                {
                    gasto.ImgCheque = imgChequeBase64;
                }
            }
             if (!string.IsNullOrEmpty(gasto.ImgComprobante) && gasto.ImgComprobante.Length > 30)
            {
                var imgComprobanteBase64 = await _convertirImagenBase64Service.ConvertirImagenBase64(gasto.ImgComprobante);
                if (imgComprobanteBase64 != null)
                {
                    gasto.ImgComprobante = imgComprobanteBase64;
                }
            }

        if (gasto is null)
        {
            //Es un método para mostrar error explicito
            return GastoNotFound(codigoGasto);
        }

        return gasto;
    }

    [HttpGet("{codigoGasto}")]
    public async Task<ActionResult<GastoOutAllDto>> GetById(int codigoGasto)
    {
        var gasto = await _gastoService.GetByIdDto(codigoGasto);

        if (gasto is null)
        {
            //Es un método para mostrar error explicito
            return GastoNotFound(codigoGasto);
        }

        return gasto;
    }
    //Lista de los productos
      [HttpGet("listaProductos/{codigoGasto}")]
         public async Task<IEnumerable<GastoDetalle>> ObtenerListaProductos(int codigoGasto)
    {
        var gastos = await _gastoService.ObtenerListaProductos(codigoGasto);
        return gastos;
    }

    //Consulta para obtener información de actualización del gasto
    [HttpGet("getgastoupdate/{id}")]
    public async Task<ActionResult<GastoUpdateOutDto>> GetByIdUpdateGasto(int id)
    {
        var gasto = await _gastoService.GetByIdUpdateDto(id);

        if (gasto is null)
        {
            //Es un método para mostrar error explicito
            return GastoNotFound(id);
        }
        return gasto;
    }

    //Solo el administrador puede crear Gastos

    [HttpPost("create")]
    public async Task<IActionResult> CrearGasto([FromForm] GastoInputDto model)
    {
        try
        {
            if (model == null || model.Productos == null)
            {
                return BadRequest("Los datos del gasto no son válidos.");
            }

            //var img = model.ImgPatrocinador.FileName;
            var imgCheque = model.ImgCheque;
            var imgComprobante = model.ImgComprobante;
            var imgEstudiante = model.ImgEstudiante;
            string folder = "Gastos";
            string imageUrlCheque = "sin imagen";
            string imageUrlComprobante = "sin imagen";
            string imageUrlEstudiante = "sin imagen";

            //Cargando imagenes uno por uno con validaciones
            if (imgCheque != null)
            {
                var fileCheque = Request.Form.Files.FirstOrDefault(f => f.Name == nameof(model.ImgCheque));
                if (fileCheque != null)
                {
                    imageUrlCheque = await _fileUploadService.UploadFileAsync(fileCheque, folder);
                }

            }

            if (imgComprobante != null)
            {

                var fileComprobante = Request.Form.Files.FirstOrDefault(f => f.Name == nameof(model.ImgComprobante));
                if (fileComprobante != null)
                {
                    imageUrlComprobante = await _fileUploadService.UploadFileAsync(fileComprobante, folder);
                }
            }

            if (imgEstudiante != null)
            {
                var fileEstudiante = Request.Form.Files.FirstOrDefault(f => f.Name == nameof(model.ImgEstudiante));
                if (fileEstudiante != null)
                {
                    imageUrlEstudiante = await _fileUploadService.UploadFileAsync(fileEstudiante, folder);
                }
            }

            //INSTANCIANDO GASTO
            Gasto gasto = new Gasto(){
            CodigoEstudiante = model.CodigoEstudiante,
            FechaEntrega = model.FechaEntrega,
            Titulo = model.Titulo,
            Estado = model.Estado,
            TipoPago = model.TipoPago,
            NumeroCheque = model.NumeroCheque,
            Monto = model.Monto,
            PersonaRecibe = model.PersonaRecibe,
            Descripcion = model.Descripcion,
            FechaRecibirComprobante = model.FechaRecibirComprobante,
            NumeroComprobante = model.NumeroComprobante,
            ImgCheque = imageUrlCheque,
            ImgComprobante = imageUrlComprobante,
            ImgEstudiante = imageUrlEstudiante,
            };
            var nuevoGasto = await _gastoService.Create(gasto);
            int codigoNuevoGasto = nuevoGasto.CodigoGasto;

            foreach(var producto in model.Productos){
                producto.CodigoGasto = codigoNuevoGasto;
                producto.Estatus = "A";
                await _gastoService.CreateProducto(producto);
            }

            return Ok(new{status = true, message="El gasto fue creado correctamente."});

        }
        catch
        {
            return BadRequest(new{status = false, message="Hubo un error al intentar guardar nuevo gasto."});

        }

    }

    [HttpPut("update/{codigoGasto}")]
    public async Task<IActionResult> ActualizarGasto (int codigoGasto, [FromForm] GastoInputDto model)
    {
        try
        {
            if (model == null)
            {
                return BadRequest("Los datos del gasto no son válidos.");
            }

            //var img = model.ImgPatrocinador.FileName;
            var imgCheque = model.ImgCheque;
            var imgComprobante = model.ImgComprobante;
            var imgEstudiante = model.ImgEstudiante;
            string folder = "Gastos";
            string imageUrlCheque = null;
            string imageUrlComprobante = null;
            string imageUrlEstudiante = null;

            //Cargando imagenes uno por uno con validaciones
            if (imgCheque != null)
            {
                var fileCheque = Request.Form.Files.FirstOrDefault(f => f.Name == nameof(model.ImgCheque));
                if (fileCheque != null)
                {
                    imageUrlCheque = await _fileUploadService.UploadFileAsync(fileCheque, folder);
                }

            }

            if (imgComprobante != null)
            {

                var fileComprobante = Request.Form.Files.FirstOrDefault(f => f.Name == nameof(model.ImgComprobante));
                if (fileComprobante != null)
                {
                    imageUrlComprobante = await _fileUploadService.UploadFileAsync(fileComprobante, folder);
                }
            }

            if (imgEstudiante != null)
            {
                var fileEstudiante = Request.Form.Files.FirstOrDefault(f => f.Name == nameof(model.ImgEstudiante));
                if (fileEstudiante != null)
                {
                    imageUrlEstudiante = await _fileUploadService.UploadFileAsync(fileEstudiante, folder);
                }
            }

            //INSTANCIANDO GASTO
            Gasto gasto = new Gasto(){
            CodigoEstudiante = model.CodigoEstudiante,
            FechaEntrega = model.FechaEntrega,
            Titulo = model.Titulo,
            Estado = model.Estado,
            TipoPago = model.TipoPago,
            NumeroCheque = model.NumeroCheque,
            Monto = model.Monto,
            PersonaRecibe = model.PersonaRecibe,
            Descripcion = model.Descripcion,
            FechaRecibirComprobante = model.FechaRecibirComprobante,
            NumeroComprobante = model.NumeroComprobante,
            ImgCheque = imageUrlCheque,
            ImgComprobante = imageUrlComprobante,
            ImgEstudiante = imageUrlEstudiante,
            };
            await _gastoService.Update(codigoGasto, gasto);

            foreach(var producto in model.Productos){
                producto.CodigoGasto = codigoGasto;
                producto.Estatus = "A";
                await _gastoService.CreateProducto(producto);
            }

            return Ok(new{status = true, message="El gasto fue actualizado correctamente."});

        }
        catch
        {
            return BadRequest(new{status = false, message="Hubo un error al intentar al actualizar gasto."});

        }

    }
    [HttpPut("updateAntes/{codigoGasto}")]
    public async Task<IActionResult> ActualizarDatosGasto(int codigoGasto, [FromForm] GastoDto model)
    {
        try
        {
            if (model == null)
            {
                return BadRequest("Los datos del gasto no son válidos.");
            }

            var imgCheque = model.ImgCheque;
            var imgComprobante = model.ImgComprobante;
            var imgEstudiante = model.ImgEstudiante;
            string folder = "Gastos";
            string imageUrlCheque = null;
            string imageUrlComprobante = null;
            string imageUrlEstudiante = null;

            //Cargando imagenes uno por uno con validaciones
            if (imgCheque != null)
            {
                var fileCheque = Request.Form.Files.FirstOrDefault(f => f.Name == nameof(model.ImgCheque));
                if (fileCheque != null)
                {
                    imageUrlCheque = await _fileUploadService.UploadFileAsync(fileCheque, folder);
                }
            }
            if (imgComprobante != null)
            {
                var fileComprobante = Request.Form.Files.FirstOrDefault(f => f.Name == nameof(model.ImgComprobante));
                if (fileComprobante != null)
                {
                    imageUrlComprobante = await _fileUploadService.UploadFileAsync(fileComprobante, folder);
                }
            }
            if (imgEstudiante != null)
            {
                var fileEstudiante = Request.Form.Files.FirstOrDefault(f => f.Name == nameof(model.ImgEstudiante));
                if (fileEstudiante != null)
                {
                    imageUrlEstudiante = await _fileUploadService.UploadFileAsync(fileEstudiante, folder);
                }
            }

            //INSTANCIANDO GASTO
            Gasto gasto = new Gasto(){
            CodigoEstudiante = model.CodigoEstudiante,
            FechaEntrega = model.FechaEntrega,
            Titulo = model.Titulo,
            Estado = model.Estado,
            TipoPago = model.TipoPago,
            NumeroCheque = model.NumeroCheque,
            Monto = model.Monto,
            PersonaRecibe = model.PersonaRecibe,
            Descripcion = model.Descripcion,
            FechaRecibirComprobante = model.FechaRecibirComprobante,
            NumeroComprobante = model.NumeroComprobante,
            ImgCheque = imageUrlCheque,
            ImgComprobante = imageUrlComprobante,
            ImgEstudiante = imageUrlEstudiante,
            };
            await _gastoService.Update(codigoGasto, gasto);

            return Ok(new{status = true, message="El gasto fue actualizado correctamente."});
        }
        catch
        {
            return BadRequest(new{status = false, message="Hubo un error al intentar actualizar los datos del gasto."});
        }

    }
    [HttpPost("crearProducto")]
    public async Task<IActionResult> CrearProducto(GastoDetalle model)
    {
        try
        { 
                await _gastoService.CreateProducto(model);

            return Ok(new{status = true, message="El producto fue creado correctamente."});

        }
        catch
        {
            return BadRequest(new{status = false, message="Hubo un error al intentar guardar nuevo gasto."});

        }

    }
    
    [HttpPut("actualizarProducto/{codigoProducto}")]
    public async Task<IActionResult> ActualizarProducto(int codigoProducto, GastoDetalle model)
    {
        try
        { 
                await _gastoService.ActualizarProducto(codigoProducto, model);

            return Ok(new{status = true, message="El producto fue actualizado correctamente."});

        }
        catch
        {
            return BadRequest(new{status = false, message="Hubo un error al intentar actualizar el producto."});

        }

    }

    [HttpPut("updateimg/{id}")]
    public async Task<IActionResult> ModificarGasto([FromForm] GastoInputDto model, int id)
    {
        try
        {
            if (model == null)
            {
                return BadRequest("Los datos del Patrocinador son inválidos.");
            }

            //var img = model.ImgPatrocinador.FileName;
            var imgCheque = model.ImgCheque;
            var imgComprobante = model.ImgComprobante;
            var imgEstudiante = model.ImgEstudiante;
            string folder = "Becarios";
            string imageUrlCheque = null;
            string imageUrlComprobante = null;
            string imageUrlEstudiante = null;

            //Cargando imagenes uno por uno con validaciones
            if (imgCheque != null)
            {
                var fileCheque = Request.Form.Files[0];
                imageUrlCheque = await _fileUploadService.UploadFileAsync(fileCheque, folder);
            }

            if (imgComprobante != null)
            {
                var fileComprobante = Request.Form.Files[1];
                imageUrlComprobante = await _fileUploadService.UploadFileAsync(fileComprobante, folder);
            }

            if (imgEstudiante != null)
            {
                var fileEstudiante = Request.Form.Files[2];
                imageUrlCheque = await _fileUploadService.UploadFileAsync(fileEstudiante, folder);
            }

            //INSTANCIANDO GASTO
            Gasto gasto = new Gasto();
            gasto.CodigoEstudiante = model.CodigoEstudiante;
            gasto.FechaEntrega = model.FechaEntrega;
            gasto.Titulo = model.Titulo;
            gasto.Estado = model.Estado;
            gasto.TipoPago = model.TipoPago;
            gasto.NumeroCheque = model.NumeroCheque;
            gasto.Monto = model.Monto;
            gasto.PersonaRecibe = model.PersonaRecibe;
            gasto.Descripcion = model.Descripcion;
            gasto.FechaRecibirComprobante = model.FechaRecibirComprobante;
            gasto.NumeroComprobante = model.NumeroComprobante;
            gasto.ImgCheque = imageUrlCheque;
            gasto.ImgComprobante = imageUrlComprobante;
            gasto.ImgEstudiante = imageUrlEstudiante;

            await _gastoService.Update(id, gasto);

            return Ok("Estudiante creado exitosamente.");

        }
        catch
        {
            return BadRequest("No se pudo registrar gasto");

        }

    }



    //Solo el administrador puede actualizar Estudiantes
    //[Authorize(Policy = "Administrador")]
    [HttpPut("update45/{id}")]
    public async Task<IActionResult> Update(int id, GastoDto gastoDto)
    {
        var gastoToUpdate = await _gastoService.GetById(id);

        if (gastoToUpdate is not null)
        {
            Gasto gasto = new Gasto();
            gasto.CodigoEstudiante = gastoDto.CodigoEstudiante;
            gasto.FechaEntrega = gastoDto.FechaEntrega;
            gasto.Titulo = gastoDto.Titulo;
            gasto.Estado = gastoDto.Estado;
            gasto.TipoPago = gastoDto.TipoPago;
            gasto.NumeroCheque = gastoDto.NumeroCheque;
            gasto.Monto = gastoDto.Monto;
            gasto.PersonaRecibe = gastoDto.PersonaRecibe;
            gasto.Descripcion = gastoDto.Descripcion;
            gasto.FechaRecibirComprobante = gastoDto.FechaRecibirComprobante;
            gasto.NumeroComprobante = gastoDto.NumeroComprobante;
            gasto.ImgCheque = gastoDto.ImgCheque;
            gasto.ImgComprobante = gastoDto.ImgComprobante;
            gasto.ImgEstudiante = gasto.ImgEstudiante;
            await _gastoService.Update(id, gasto);
            return Ok(new
            {
                status = true,
                message = "Gasto modificado correctamente"
            });
        }
        else
        {
            return GastoNotFound(id);
        }
    }


    //Solo el administrador puede eliminar Gastos
    //[Authorize(Policy = "Administrador")]
    [HttpDelete("deleteProducto/{codigoProducto}")]
    public async Task<IActionResult> Delete(int codigoProducto)
    {
        var gastoToDelete = await _gastoService.GetByIdProducto(codigoProducto);

        if (gastoToDelete is not null)
        {
            await _gastoService.Delete(codigoProducto);
            return Ok(new{status = true, message="El producto ha sido eliminado correctamente."});
        }
        else
        {
            return NotFound(new{status = false, message ="Hubo un error al intentar eliminar el producto"});
        }
    }

    //Definir un método para indicar el mensaje del NotFound
    public NotFoundObjectResult GastoNotFound(int id)
    {
        return NotFound(new { message = $"El Gasto con el ID {id} no existe" });
    }

}