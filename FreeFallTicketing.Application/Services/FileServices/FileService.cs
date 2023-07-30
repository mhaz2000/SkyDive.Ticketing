using SkyDiveTicketing.Application.Base;
using SkyDiveTicketing.Core.Entities;
using SkyDiveTicketing.Core.Repositories.Base;
using Microsoft.AspNetCore.Http;

namespace SkyDiveTicketing.Application.Services.FileServices
{
    public class FileService : IFileService
    {
        private readonly IUnitOfWork _unitOfWork;
        public FileService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<(FileStream, string)> GetFile(Guid id, string path)
        {
            var fileModel = await _unitOfWork.FileRepository.GetByIdAsync(id);
            if (fileModel is null)
                throw new ManagedException("فایل مورد نظر یافت نشد.");

            var filePath = Path.Combine(path, $"{id}.dat");

            if (!File.Exists(filePath))
                throw new ManagedException("فایل مورد نظر یافت نشد.");

            return (new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read), fileModel.Filename);
        }

        public async Task<Guid> StoreFile(IFormFile file, string path)
        {
            var settings = await _unitOfWork.SettingsRepository.FirstOrDefaultAsync(c => true);
            if (settings.FileSizeLimitiation != 0 && file.Length > settings.FileSizeLimitiation * 1000)
                throw new ManagedException($"حداکثر حجم فایل ارسالی {settings.FileSizeLimitiation} KB است.");

            var fileId = Guid.NewGuid();
            var dir = Path.Combine(path, $"{fileId}.dat");

            using (var fileStream = new FileStream(dir, FileMode.CreateNew, FileAccess.Write, FileShare.Write))
            {
                await file.CopyToAsync(fileStream);
            }

            await _unitOfWork.FileRepository.AddFile(fileId, file.FileName);
            await _unitOfWork.CommitAsync();
            return fileId;
        }
    }
}
