using SkyDiveTicketing.Core.Entities.Base;

namespace SkyDiveTicketing.Core.Entities
{
    public class FileModel : BaseEntity
    {
        public FileModel(Guid fileId, string filename) : base()
        {
            Filename = filename;
            Id = fileId;
        }

        public string Filename { get; set; }
    }
}
