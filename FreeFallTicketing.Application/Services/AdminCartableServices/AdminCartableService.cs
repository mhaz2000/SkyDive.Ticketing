using SkyDiveTicketing.Application.DTOs.AdminCartableDTOs;
using SkyDiveTicketing.Application.Helpers;
using SkyDiveTicketing.Core.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyDiveTicketing.Application.Services.AdminCartableServices
{
    public class AdminCartableService : IAdminCartableService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AdminCartableService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<AdminCartableMessageDTO> GetAdminCartableMessages()
        {
            return _unitOfWork.AdminCartableRepository.Include(c=>c.Applicant).Select(cartable => new AdminCartableMessageDTO(cartable.Id, cartable.CreatedAt, cartable.UpdatedAt,
                cartable.Title, cartable.Applicant.Id, cartable.Applicant.FullName, cartable.Done, cartable.RequestType, cartable.RequestType.GetDescription()));
        }
    }
}
