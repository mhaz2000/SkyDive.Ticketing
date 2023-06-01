using SkyDiveTicketing.Core.Entities;
using SkyDiveTicketing.Core.Repositories.Base;

namespace SkyDiveTicketing.Core.Repositories
{
    public interface IFileModelRepository : IRepository<FileModel>
    {
        Task<Guid> AddFile(Guid fileId, string filename);

    }
}
