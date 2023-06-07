namespace SkyDiveTicketing.API.Jobs
{
    public interface IPassengerDocumentJob
    {
        Task CheckPassengerDocumentExpirationDate();
    }
}
