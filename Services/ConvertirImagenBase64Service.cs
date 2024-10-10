using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace sistemaQuchooch.Sevices;
public class ConvertirImagenBase64Service
{
    public async Task<string> ConvertirImagenBase64(string imageUrl)
    {
        try
        {
            using (HttpClient client = new HttpClient())
            {
                // Descarga los datos de la imagen desde la URL
                var imageBytes = await client.GetByteArrayAsync(imageUrl);

                 var mimeType = ObtenerMimeType(imageUrl);

                    // Convierte los bytes de la imagen a una cadena en formato Base64
                    string base64String = Convert.ToBase64String(imageBytes);

                    // Combina el prefijo "data:image/...;base64," con la cadena Base64
                    return $"data:{mimeType};base64,{base64String}";
            }
        }
        catch (Exception ex)
        {
            // Manejar cualquier error
            Console.WriteLine($"Error: {ex.Message}");
            return null;
        }
    }

    private string ObtenerMimeType(string imageUrl)
        {
            // Determinar el tipo MIME a partir de la extensión del archivo
            var extension = imageUrl.Split('.').Last().ToLower();
            return extension switch
            {
                "png" => "image/png",
                "jpg" => "image/jpeg",
                "jpeg" => "image/jpeg",
                "gif" => "image/gif",
                _ => "application/octet-stream", // Si no se reconoce, se usa un tipo genérico
            };
        }
}
