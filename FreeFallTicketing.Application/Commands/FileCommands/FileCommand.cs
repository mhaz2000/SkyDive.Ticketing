using Microsoft.AspNetCore.Http;

namespace SkyDiveTicketing.Application.Commands.FileCommands
{
    public class FileCommand
    {
        public IFormFile File { get; set; }
    }
}
