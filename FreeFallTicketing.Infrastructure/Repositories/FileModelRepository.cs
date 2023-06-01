using SkyDiveTicketing.Core.Entities;
using SkyDiveTicketing.Core.Repositories;
using SkyDiveTicketing.Infrastructure.Data;
using SkyDiveTicketing.Infrastructure.Repositories.Base;

namespace SkyDiveTicketing.Infrastructure.Repositories
{
    public class FileModelRepository : Repository<FileModel>, IFileModelRepository
    {
        public FileModelRepository(DataContext context) : base(context)
        {
        }

        public async Task<Guid> AddFile(Guid fileId, string filename)
        {
            var entity = new FileModel(fileId, filename);

            await Context.Files.AddAsync(entity);

            return entity.Id;
        }
    }
}
