namespace SkyDiveTicketing.API.Jobs.JumpRecordJobs
{
    public interface IJumpRecordJob
    {
        Task CheckIfExpired();
    }
}
