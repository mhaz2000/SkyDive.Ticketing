using SkyDiveTicketing.Application.DTOs.AdminCartableDTOs;

namespace SkyDiveTicketing.Application.Services.AdminCartableServices
{
    public interface IAdminCartableService
    {
        IEnumerable<AdminCartableMessageDTO> GetAdminCartableMessages(Core.Entities.RequestType? requestType, string? search);
        Task RemoveMessageFromCartable(Guid id);
    }
}
