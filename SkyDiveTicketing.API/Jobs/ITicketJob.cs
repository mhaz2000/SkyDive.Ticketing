namespace SkyDiveTicketing.API.Jobs
{
    public interface ITicketJob
    {
        Task CheckTicketLockTime();
    }
}
