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

        public IEnumerable<AdminCartableMessageDTO> GetAdminCartableMessages(RequestType? requestType)
        {
            return _unitOfWork.AdminCartableRepository.GetAdminCartables(c=> requestType == null? true : c.RequestType == requestType)
                .Select(cartable => new AdminCartableMessageDTO(cartable.Id, cartable.CreatedAt, cartable.UpdatedAt,
                cartable.Title, cartable.Applicant.Id, cartable.Applicant.FullName, cartable.Done, cartable.RequestType, cartable.RequestType.GetDescription(),
                cartable.Applicant.Code, cartable.Applicant.UserType.Title, cartable.Applicant.UserType.Id));
        }
    }
}
