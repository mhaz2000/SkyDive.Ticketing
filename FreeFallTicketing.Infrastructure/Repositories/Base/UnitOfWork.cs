using SkyDiveTicketing.Core.Repositories;
using SkyDiveTicketing.Core.Repositories.Base;
using SkyDiveTicketing.Infrastructure.Data;

namespace SkyDiveTicketing.Infrastructure.Repositories.Base
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DataContext _context;

        private CityRepository? _cityRepository;
        private UserRepository? _userRepository;
        private TicketRepository? _ticketRepository;
        private MessageRepository? _messageRepository;
        private UserTypeRepository? _userTypeRepository;
        private SettingsRepository? _settingsRepository;
        private PassengerRepository? _passengerRepository;
        private FileModelRepository? _fileModelRepository;
        private FlightLoadRepository? _flightLoadRepository;
        private TransactionRepository? _transactionRepository;
        private ShoppingCartRepository? _shoppingCartRepository;
        private SkyDiveEventRepository? _skyDiveEventRepository;
        private AdminCartableRepository _adminCartableRepository;
        private SkyDiveEventItemRepository? _skyDiveEventItemRepository;
        private PassengerDocumentRepository? _passengerDocumentRepository;
        private SkyDiveEventStatusRepository? _skyDiveEventStatusRepository;
        private SkyDiveEventTicketTypeRepository? _skyDiveEventTicketTypeRepository;
        private FlightLoadCancellationTypeRepository? _flightLoadCancellationTypeRepository;

        public UnitOfWork(DataContext context) => _context = context;

        public ICityRepository CityRepository => _cityRepository ?? new CityRepository(_context);

        public IUserRepository UserRepository => _userRepository ?? new UserRepository(_context);

        public ITicketRepository TicketRepository => _ticketRepository ?? new TicketRepository(_context);

        public IMessageRepository MessageRepository => _messageRepository ?? new MessageRepository(_context);

        public IFileModelRepository FileRepository => _fileModelRepository ?? new FileModelRepository(_context);

        public IUserTypeRepository UserTypeRepository => _userTypeRepository ?? new UserTypeRepository(_context);

        public ISettingsRepository SettingsRepository => _settingsRepository ?? new SettingsRepository(_context);

        public IPassengerRepository PassengerRepository => _passengerRepository ?? new PassengerRepository(_context);

        public IFlightLoadRepository FlightLoadRepository => _flightLoadRepository ?? new FlightLoadRepository(_context);

        public ITransactionRepository TransactionRepositroy => _transactionRepository ?? new TransactionRepository(_context);

        public IShoppingCartRepository ShoppingCartRepository => _shoppingCartRepository ?? new ShoppingCartRepository(_context);

        public ISkyDiveEventRepository SkyDiveEventRepository => _skyDiveEventRepository ?? new SkyDiveEventRepository(_context);

        public IAdminCartableRepository AdminCartableRepository => _adminCartableRepository ?? new AdminCartableRepository(_context);

        public ISkyDiveEventItemRepository SkyDiveEventItemRepository => _skyDiveEventItemRepository ?? new SkyDiveEventItemRepository(_context);

        public IPassengerDocumentRepository PassengerDocumentRepository => _passengerDocumentRepository ?? new PassengerDocumentRepository(_context);

        public ISkyDiveEventStatusRepository SkyDiveEventStatusRepository => _skyDiveEventStatusRepository ??  new SkyDiveEventStatusRepository(_context);

        public ISkyDiveEventTicketTypeRepository SkyDiveEventTicketTypeRepository => _skyDiveEventTicketTypeRepository ?? new SkyDiveEventTicketTypeRepository(_context);

        public IFlightLoadCancellationTypeRepository FlightLoadCancellationTypeRepository => _flightLoadCancellationTypeRepository ?? new FlightLoadCancellationTypeRepository(_context);



        public async Task<int> CommitAsync()
        {
            var result = await _context.SaveChangesAsync();
            Dispose();
            return result;
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
