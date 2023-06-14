using SkyDiveTicketing.API.Base;
using SkyDiveTicketing.Application.Commands.FileCommands;
using SkyDiveTicketing.Application.Services.FileServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace SkyDiveTicketing.API.Controllers.Files
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ApiControllerBase
    {
        private readonly IFileService _fileService;
        private readonly string _fileStoragePath;

        public FileController(IFileService fileService, IConfiguration configuration)
        {
            _fileService = fileService;
            _fileStoragePath = configuration["FileStoragePath"] ??
                throw new ApplicationException("There is no path for storing files in project configuration.");
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromForm] FileCommand command)
        {
            var fileId = await _fileService.StoreFile(command.File, _fileStoragePath);

            return Ok(fileId);
        }

        [HttpGet("{id}")]
        public async Task<FileResult> Get(Guid id)
        {
            var file = await _fileService.GetFile(id, _fileStoragePath);
            var fileStream = file.stream;
            var fileName = file.filename;

            var provider = new FileExtensionContentTypeProvider();

            if (!provider.TryGetContentType(fileName, out var contentType))
            {
                contentType = "application/octet-stream";
            }

            var cdStr = $"inline; filename=\"{fileName}\"";

            Response.Headers.Add("Access-Control-Allow-Headers", "Content-Disposition");
            //Response.Headers.Add("Content-Disposition", cdStr);
            Response.Headers.Add("X-Content-Type-Options", "nosniff");

            return File(fileStream, contentType, fileName);
        }
    }
}
