﻿using Microsoft.AspNetCore.Http;

namespace SkyDiveTicketing.Application.Services.FileServices
{
    public interface IFileService
    {
        Task<Guid> StoreFile(IFormFile file, string path, bool IgnoreFileLimitation);

        Task<(FileStream stream, string filename)> GetFile(Guid id, string path);
    }
}
