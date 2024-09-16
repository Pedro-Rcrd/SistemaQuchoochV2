using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

public class FileUploadService
{
    private readonly Cloudinary _cloudinary;

    public FileUploadService()
    {
        string cloudName = "ddxnadexi";
        string apiKey = "822983787533177";
        string apiSecret = "kXxNIEGQi2SwV71mmtT5XGfmiso";
        var account = new Account(cloudName, apiKey, apiSecret);
        _cloudinary = new Cloudinary(account);
    }

    public async Task<string> UploadFileAsync(IFormFile file, string folder)
    {
        if (file == null || file.Length == 0)
        {
            throw new ArgumentException("No se ha proporcionado un archivo válido.");
        }

        // Genera un nombre único para la imagen basado en la fecha y hora actual
        var timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
        var fileName = $"{timestamp}_{file.FileName}";

        var uploadParams = new ImageUploadParams
        {
            File = new FileDescription(file.FileName, file.OpenReadStream()),
            Folder = folder, // Almacena en la carpeta especificada en Cloudinary
            PublicId = fileName, // Define el nombre de la imagen en Cloudinary
        };
        //Cargar imagen
        var uploadResult = await _cloudinary.UploadAsync(uploadParams);

        // Almacena la URL de la imagen cargada en tu base de datos o responde al cliente
        var imageUrl = uploadResult.Uri.ToString();
        //Retornando URL
        return imageUrl;
    }
}
