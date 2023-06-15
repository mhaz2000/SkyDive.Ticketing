using SkyDiveTicketing.Application.Commands.UserCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyDiveTicketing.Application.Services.PassengerServices
{
    public interface IPassengerService
    {
        Task CheckUserDocument(Guid documentId, bool isConfirmed);
        Task CheckPassengerDocumentExpirationDate();
    }
}
