using SkyDiveTicketing.Application.Base;
using SkyDiveTicketing.Application.Commands.UserCommands;
using SkyDiveTicketing.Application.DTOs.UserDTOs;
using SkyDiveTicketing.Core.Repositories.Base;

namespace SkyDiveTicketing.Application.Services.UserTypeServices
{
    public class UserTypeService : IUserTypeService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserTypeService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task AssignTicketType(AssignTicketTypeCommand command)
        {
            var userType = await _unitOfWork.UserTypeRepository.GetFirstWithIncludeAsync(c => c.Id == command.UserTypeId, c => c.AllowedTicketTypes);
            if (userType is null)
                throw new ManagedException("نوع کاربری مورد نظر یافت نشد.");

            foreach (var ticketTypeId in command.TicketTypes)
            {
                var ticketType = await _unitOfWork.SkyDiveEventTicketTypeRepository.GetByIdAsync(ticketTypeId);
                if (ticketType is null)
                    throw new ManagedException("نوع بلیط یافت نشد.");

                _unitOfWork.UserTypeRepository.AddTicketType(ticketType, userType);
            }

            await _unitOfWork.CommitAsync();
        }

        public async Task Create(UserTypeCommand command)
        {
            var duplicateTitle = await _unitOfWork.UserTypeRepository.AnyAsync(c => c.Title == command.Title);
            if (duplicateTitle)
                throw new ManagedException("عنوان تکراری است.");

            await _unitOfWork.UserTypeRepository.Create(command.Title);
            await _unitOfWork.CommitAsync();
        }

        public IEnumerable<UserTypeDTO> GetAllTypes(string search)
        {
            var userTypes = _unitOfWork.UserTypeRepository.Include(c=> c.AllowedTicketTypes)
                .Where(c => c.Title.Contains(search))
                .Select(userType => new UserTypeDTO(userType.Title, userType.IsDefault, userType.Id, userType.CreatedAt,
                userType.UpdatedAt, userType.AllowedTicketTypes.Select(ticketType => new UserTypeAllowedTicketTypeDTO(ticketType.Id, ticketType.CreatedAt, ticketType.UpdatedAt, ticketType.Title))));

            return userTypes;
        }

        public async Task<UserTypeDTO> GetUserType(Guid id)
        {
            var userType = await _unitOfWork.UserTypeRepository.GetFirstWithIncludeAsync(c=> c.Id == id, c=> c.AllowedTicketTypes);
            if (userType is null)
                throw new ManagedException("نوع کاربری مورد نظر یافت نشد.");

            return new UserTypeDTO(userType.Title, userType.IsDefault, userType.Id, userType.CreatedAt, userType.UpdatedAt,
                userType.AllowedTicketTypes.Select(ticketType => new UserTypeAllowedTicketTypeDTO(ticketType.Id, ticketType.CreatedAt, ticketType.UpdatedAt, ticketType.Title)));
        }

        public async Task Remove(Guid id)
        {
            var userType = await _unitOfWork.UserTypeRepository.GetByIdAsync(id);
            if (userType is null)
                throw new ManagedException("نوع کاربری مورد نظر یافت نشد.");

            var usedUserType = _unitOfWork.UserRepository.Include(c => c.UserType).Any(c => c.UserType.Id == id);
            if (usedUserType)
                throw new ManagedException("ابتدا نوع کاربری اشخاصی که این نوع را دارند، تغییر دهید.");

            _unitOfWork.UserTypeRepository.Remove(userType);
            await _unitOfWork.CommitAsync();
        }

        public async Task Update(UserTypeCommand command, Guid id)
        {
            var userType = await _unitOfWork.UserTypeRepository.GetByIdAsync(id);
            if (userType is null)
                throw new ManagedException("نوع کاربری مورد نظر یافت نشد.");

            _unitOfWork.UserTypeRepository.Update(userType, command.Title);
            await _unitOfWork.CommitAsync();
        }
    }
}
