using SkyDiveTicketing.Core.Entities;
using SkyDiveTicketing.Core.Repositories;
using SkyDiveTicketing.Infrastructure.Data;
using SkyDiveTicketing.Infrastructure.Repositories.Base;

namespace SkyDiveTicketing.Infrastructure.Repositories
{
    public class JumpRecordRepository : Repository<JumpRecord>, IJumpRecordRepository
    {
        public JumpRecordRepository(DataContext context) : base(context)
        {
        }

        public async Task AddJumpRecord(DateTime date, string location, string equipments, string planeType, float height,
            TimeSpan time, string description, User user, bool createdByAdmin)
        {
            var jumpRecord = new JumpRecord(date, location, equipments, planeType, height, time, description, user);
            if (createdByAdmin)
                jumpRecord.SetAsConfirmd();

            await Context.JumpRecords.AddAsync(jumpRecord);
        }

        public void ConfirmJumpRecord(JumpRecord jumpRecord)
        {
            jumpRecord.SetAsConfirmd();
        }

        public void ExpireJumpRecord(JumpRecord jumpRecord)
        {
            jumpRecord.SetAsExpired();
        }
    }
}
