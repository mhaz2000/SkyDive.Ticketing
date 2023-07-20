using SkyDiveTicketing.Application.Base;
using SkyDiveTicketing.Application.DTOs.AdminCartableDTOs;
using SkyDiveTicketing.Application.Helpers;
using SkyDiveTicketing.Core.Entities;
using SkyDiveTicketing.Core.Repositories.Base;

namespace SkyDiveTicketing.Application.Services.AdminCartableServices
{
    public class AdminCartableService : IAdminCartableService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AdminCartableService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<AdminCartableMessageDTO> GetAdminCartableMessages(RequestType? requestType, string? search)
        {
            search = string.IsNullOrEmpty(search) ? string.Empty : search;

            return _unitOfWork.AdminCartableRepository
                .GetAdminCartables(c => requestType == null ? true : c.RequestType == requestType).AsEnumerable().Where(c=> c.Applicant.FullName.Contains(search))
                .OrderByDescending(c => c.CreatedAt)
                .Select(cartable => new AdminCartableMessageDTO(cartable.Id, cartable.CreatedAt, cartable.UpdatedAt,
                    cartable.Title, cartable.Applicant.Id, cartable.Applicant.FullName, cartable.Done, cartable.RequestType, cartable.RequestType.GetDescription(),
                    cartable.Applicant.Code, cartable.Applicant.UserType.Title, cartable.Applicant.UserType.Id));
        }

        public async Task RemoveMessageFromCartable(Guid id)
        {
            var cartableMessage = await _unitOfWork.AdminCartableRepository.GetByIdAsync(id);
            if (cartableMessage is null)
                throw new ManagedException("پیام مورد نظر یافت نشد.");

            _unitOfWork.AdminCartableRepository.Remove(cartableMessage);
            await _unitOfWork.CommitAsync();
        }
    }
}
