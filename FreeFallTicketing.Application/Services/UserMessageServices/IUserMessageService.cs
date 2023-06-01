using SkyDiveTicketing.Application.DTOs.UserDTOs;

namespace SkyDiveTicketing.Application.Services.UserMessageServices
{
    public interface IUserMessageService
    {
        Task<IEnumerable<UserMessageDTO>> GetUserMessages(Guid? userId);
        Task<UserMessageDTO> GetUserMessage(Guid id);
        Task MessageHasBeenSeen(Guid id);
    }
}
