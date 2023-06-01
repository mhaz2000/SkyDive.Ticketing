namespace SkyDiveTicketing.Core.Repositories.Base
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository UserRepository { get; }
        IFlightLoadRepository FlightLoadRepository { get; }
        IFlightLoadCancellationTypeRepository FlightLoadCancellationTypeRepository { get; }
        IFileModelRepository FileRepository { get; }
        ITicketRepository TicketRepository { get; }
        ICityRepository CityRepository { get; }
        IMessageRepository MessageRepository { get; }
        IPassengerRepository PassengerRepository { get; }
        IPassengerDocumentRepository PassengerDocumentRepository { get; }
        IUserTypeRepository UserTypeRepository { get; }
        ISettingsRepository SettingsRepository { get; }
        ISkyDiveEventTicketTypeRepository SkyDiveEventTicketTypeRepository { get; }
        ISkyDiveEventRepository SkyDiveEventRepository { get; }
        ISkyDiveEventStatusRepository SkyDiveEventStatusRepository { get; }

        Task<int> CommitAsync();
    }
}
