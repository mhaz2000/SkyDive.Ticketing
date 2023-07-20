using SkyDiveTicketing.Application.Commands.UserCommands;
using SkyDiveTicketing.Application.DTOs.UserDTOs;

namespace SkyDiveTicketing.Application.Services.UserTypeServices
{
    public interface IUserTypeService
    {
        Task AssignTicketType(AssignTicketTypeCommand command);
        Task Create(UserTypeCommand command);
        IEnumerable<UserTypeDTO> GetAllTypes(string search);
        Task<UserTypeDTO> GetUserType(Guid id);
        Task Remove(Guid id);
        Task Update(UserTypeCommand command, Guid id);
        Task UnAssignTicketType(UnAssignTicketTypeCommand command);
    }
}
