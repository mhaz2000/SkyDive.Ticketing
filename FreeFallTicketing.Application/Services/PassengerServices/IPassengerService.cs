namespace SkyDiveTicketing.Application.Services.PassengerServices
{
    public interface IPassengerService
    {
        Task CheckUserDocument(Guid documentId, bool isConfirmed);
        Task CheckPassengerDocumentExpirationDate();
        Task RemoveDocument(Guid id);
    }
}
