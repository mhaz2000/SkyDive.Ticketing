namespace SkyDiveTicketing.API.Jobs.TicketJobs
{
    public interface ITicketJob
    {
        Task CheckTicketLockTime();
    }
}
