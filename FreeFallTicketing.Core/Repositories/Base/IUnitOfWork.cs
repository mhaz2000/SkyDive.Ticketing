namespace SkyDiveTicketing.Core.Repositories.Base
{
    public interface IUnitOfWork : IDisposable
    {
        IRoleRepository RoleRepository { get; }
        IUserRepository UserRepository { get; }
        ICityRepository CityRepository { get; }
        IWalletRepository WalletRepository { get; }
        ITicketRepository TicketRepository { get; }
        IFileModelRepository FileRepository { get; }
        IMessageRepository MessageRepository { get; }
        IUserTypeRepository UserTypeRepository { get; }
        ISettingsRepository SettingsRepository { get; }
        IPassengerRepository PassengerRepository { get; }
        IFlightLoadRepository FlightLoadRepository { get; }
        IJumpRecordRepository JumpRecordRepository { get; }
        ITransactionRepository TransactionRepositroy { get; }
        IShoppingCartRepository ShoppingCartRepository { get; }
        ISkyDiveEventRepository SkyDiveEventRepository { get; }
        IAdminCartableRepository AdminCartableRepository { get; }
        ISkyDiveEventItemRepository SkyDiveEventItemRepository { get; }
        ISkyDiveEventStatusRepository SkyDiveEventStatusRepository { get; }
        ISkyDiveEventTicketTypeRepository SkyDiveEventTicketTypeRepository { get; }
        IPassengerMedicalDocumentRepository PassengerMedicalDocumentRepository { get; }
        IPassengerLogBookDocumentRepository PassengerLogBookDocumentRepository { get; }
        IPassengerAttorneyDocumentRepository PassengerAttorneyDocumentRepository { get; }
        IFlightLoadCancellationTypeRepository FlightLoadCancellationTypeRepository { get; }
        IPassengerNationalCardDocumentRepository PassengerNationalCardDocumentRepository { get; }

        Task<int> CommitAsync();
    }
}
