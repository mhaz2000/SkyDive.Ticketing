namespace SkyDiveTicketing.API.Jobs.PassengerDocumentJobs
{
    public interface IPassengerDocumentJob
    {
        Task CheckPassengerDocumentExpirationDate();
    }
}
