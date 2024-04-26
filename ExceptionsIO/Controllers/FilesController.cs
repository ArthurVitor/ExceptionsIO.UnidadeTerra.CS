using ExceptionsIO.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ExceptionsIO.Controllers;

[ApiController]
[Route("[controller]")]
public class FilesController : ControllerBase
{
  private IFileService _fileService;
  
  public FilesController(IFileService fileService)
  {
    _fileService = fileService;
  }

  [HttpGet("download")]
  public async Task<IActionResult> Download(string? fileName)
  {
    return await _fileService.DownloadFile(fileName);
  }

  [HttpPost("upload")]
  public async Task<IActionResult> Upload(IFormFile? file)
  {
    return await _fileService.UploadFile(file);
  }
}