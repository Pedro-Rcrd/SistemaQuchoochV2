using Microsoft.AspNetCore.Mvc;
using sistemaQuchooch.Sevices;
using sistemaQuchooch.Data.QuchoochModels;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

[ApiController]
[Route("api/upload")]
public class ImageUploadController : ControllerBase
{
    
    private readonly FileUploadService _fileUploadService;
     public ImageUploadController(FileUploadService fileUploadService)
    {
        _fileUploadService = fileUploadService;
    }

    [HttpPost("{folder}")]
    public async Task<IActionResult> UploadImage(string folder)
    {
        //Console.WriteLine($"El valor del 'file' es: {file}");
        //Console.WriteLine($"El valor del 'folder' es: {folder}");
       try
        {
            var file = Request.Form.Files[0];
            var imageUrl = await _fileUploadService.UploadFileAsync(file, folder);
            Console.WriteLine($"Que se esta obteniendo {imageUrl}");
            return Ok(new { imageUrl });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            // Manejar otros tipos de excepciones según sea necesario
            return StatusCode(500, "Se produjo un error interno.");
        }
    }
}
