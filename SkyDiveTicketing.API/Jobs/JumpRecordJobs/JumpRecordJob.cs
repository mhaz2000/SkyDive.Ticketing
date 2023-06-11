using SkyDiveTicketing.Application.Services.JumpRecordServices;

namespace SkyDiveTicketing.API.Jobs.JumpRecordJobs
{
    public class JumpRecordJob : IJumpRecordJob
    {
        private readonly IJumpRecordService _jumpRecordService;

        public JumpRecordJob(IJumpRecordService jumpRecordService)
        {
            _jumpRecordService = jumpRecordService;
        }

        public async Task CheckIfExpired()
        {
            await _jumpRecordService.CheckIfExpired();
        }
    }
}
