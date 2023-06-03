namespace SkyDiveTicketing.Core.Repositories.Base
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository UserRepository { get; }
        ICityRepository CityRepository { get; }
        ITicketRepository TicketRepository { get; }
        IFileModelRepository FileRepository { get; }
        IMessageRepository MessageRepository { get; }
        IUserTypeRepository UserTypeRepository { get; }
        ISettingsRepository SettingsRepository { get; }
        IPassengerRepository PassengerRepository { get; }
        IFlightLoadRepository FlightLoadRepository { get; }
        IShoppingCartRepository ShoppingCartRepository { get; }
        ISkyDiveEventRepository SkyDiveEventRepository { get; }
        ISkyDiveEventItemRepository SkyDiveEventItemRepository { get; }
        IPassengerDocumentRepository PassengerDocumentRepository { get; }
        ISkyDiveEventStatusRepository SkyDiveEventStatusRepository { get; }
        ISkyDiveEventTicketTypeRepository SkyDiveEventTicketTypeRepository { get; }
        IFlightLoadCancellationTypeRepository FlightLoadCancellationTypeRepository { get; }

        Task<int> CommitAsync();
    }
}
