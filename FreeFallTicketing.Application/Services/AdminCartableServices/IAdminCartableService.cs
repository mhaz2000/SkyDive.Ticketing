using SkyDiveTicketing.Application.DTOs.AdminCartableDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyDiveTicketing.Application.Services.AdminCartableServices
{
    public interface IAdminCartableService
    {
        IEnumerable<AdminCartableMessageDTO> GetAdminCartableMessages();
    }
}
