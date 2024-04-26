using Microsoft.AspNetCore.Mvc;

namespace ExceptionsIO.Interfaces;

public interface IFileService
{
    Task<IActionResult> DownloadFile(string? fileName);
    
    Task<IActionResult> UploadFile(IFormFile? file);
}