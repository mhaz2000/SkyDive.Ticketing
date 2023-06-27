using SkyDiveTicketing.Application.Base;
using SkyDiveTicketing.Application.DTOs.UserDTOs;
using SkyDiveTicketing.Core.Repositories.Base;

namespace SkyDiveTicketing.Application.Services.UserMessageServices
{
    public class UserMessageService : IUserMessageService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserMessageService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<UserMessageDTO> GetUserMessage(Guid id)
        {
            var message = await _unitOfWork.MessageRepository.GetByIdAsync(id);
            if (message is null)
                throw new ManagedException("پیام مورد نظر یافت نشد.");

            return new UserMessageDTO(message.Id, message.CreatedAt, message.UpdatedAt, message.Text, message.Visited, message.Title);
        }

        public async Task<IEnumerable<UserMessageDTO>> GetUserMessages(Guid? userId)
        {
            var messages = userId is null ?
                await _unitOfWork.MessageRepository.GetAllAsync() :
                await _unitOfWork.MessageRepository.FindAsync(c => c.UserId == userId);

            return messages.Select(message => new UserMessageDTO(message.Id, message.CreatedAt, message.UpdatedAt, message.Text, message.Visited, message.Title));
        }

        public async Task MessageHasBeenSeen(Guid id)
        {
            var message = await _unitOfWork.MessageRepository.GetByIdAsync(id);
            if (message is null)
                throw new ManagedException("پیام مورد نظر یافت نشد.");

            _unitOfWork.MessageRepository.MessageHasBeenSeen(message);
            await _unitOfWork.CommitAsync();
        }
    }
}
