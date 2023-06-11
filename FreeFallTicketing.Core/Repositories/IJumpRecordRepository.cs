using SkyDiveTicketing.Core.Entities;
using SkyDiveTicketing.Core.Repositories.Base;

namespace SkyDiveTicketing.Core.Repositories
{
    public interface IJumpRecordRepository : IRepository<JumpRecord>
    {
        Task AddJumpRecord(DateTime date, string location, string equipments, string planeType, float height, TimeOnly time, string description, User user);
        void ConfirmJumpRecord(JumpRecord jumpRecord);
        void ExpireJumpRecord(JumpRecord record);
    }
}
