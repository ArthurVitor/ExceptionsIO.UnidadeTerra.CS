using ExceptionsIO.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace ExceptionsIO.Services;

public class FileService : IFileService
{
    private const string DefaultDir = "Files";
    private const string DefaultContentType = "application/octet-stream";

    private readonly string _basePath;
    private readonly FileExtensionContentTypeProvider _contentTypeProvider;

    public FileService(IWebHostEnvironment environment)
    {
        // O path base convencionado para o armazenamento dos arquivos é "{RaizProjeto}/Files/"
        _basePath = Path.Combine(environment.ContentRootPath, DefaultDir);

        _contentTypeProvider = new FileExtensionContentTypeProvider();
    }


    public async Task<IActionResult> DownloadFile(string? fileName)
    {
        try
        {
            var filePath = Path.Combine(_basePath, fileName!);

            if (!_contentTypeProvider.TryGetContentType(fileName!, out string? contentType)) contentType = DefaultContentType;

            var fileBytes = await File.ReadAllBytesAsync(filePath);

            var result = new FileContentResult(fileBytes, contentType);
            result.FileDownloadName = fileName;
            return result;

        }
        catch (ArgumentNullException)
        {
            return new BadRequestObjectResult($"O nome do arquivo não pode ser null");
        }
        catch (FileNotFoundException)
        {
            return new NotFoundObjectResult($"Arquivo \"{fileName}\" não encontrado."); // 404
        }
    }

    public async Task<IActionResult> UploadFile(IFormFile? file)
    {
        try
        {
            if (!Directory.Exists(_basePath))
                Directory.CreateDirectory(_basePath);

            var filePath = Path.Combine(_basePath, file?.FileName!);

            using (var fileStream = new FileStream(filePath, FileMode.CreateNew))
            {
                await file!.CopyToAsync(fileStream);
            }

            return new OkObjectResult($"Arquivo \"{file.FileName}\" enviado com sucesso!"); // 200
        }
        catch (ArgumentNullException)
        {
            return new BadRequestObjectResult($"Arquivo não pode ser null"); // 400
        }
        catch (IOException)
        {
            return new ConflictObjectResult($"Arquivo já existe no servidor."); // 409
        }
    }
}