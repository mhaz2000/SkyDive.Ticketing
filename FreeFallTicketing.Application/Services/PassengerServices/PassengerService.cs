using SkyDiveTicketing.Application.Base;
using SkyDiveTicketing.Application.Commands.UserCommands;
using SkyDiveTicketing.Core.Entities;
using SkyDiveTicketing.Core.Repositories.Base;
using System.Linq.Expressions;

namespace SkyDiveTicketing.Application.Services.PassengerServices
{
    public class PassengerService : IPassengerService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PassengerService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task CheckPassengerDocument(Guid documentId, bool isConfirmed)
        {
            var passengerDocument = await _unitOfWork.PassengerDocumentRepository.GetByIdAsync(documentId);
            if (passengerDocument is null)
                throw new ManagedException("مدرک مورد نظر یافت نشد.");

            if (isConfirmed)
                passengerDocument.SetStatus(DocumentStatus.Confirmed);
            else
                passengerDocument.SetStatus(DocumentStatus.NotLoaded);

            var user = _unitOfWork.UserRepository.GetAllWithIncludes(c => c.Passenger.NationalCardDocumentFile.Id == documentId ||
            c.Passenger.AttorneyDocumentFile.Id == documentId ||
            c.Passenger.MedicalDocumentFile.Id == documentId ||
            c.Passenger.LogBookDocumentFile.Id == documentId).FirstOrDefault();

            var activeCondition = user is not null &&
                user.Passenger.AttorneyDocumentFile.Status == DocumentStatus.Confirmed &&
                user.Passenger.MedicalDocumentFile.Status == DocumentStatus.Confirmed &&
                user.Passenger.LogBookDocumentFile.Status == DocumentStatus.Confirmed &&
                user.Passenger.NationalCardDocumentFile.Status == DocumentStatus.Confirmed;

            if (activeCondition)
            {
                _unitOfWork.UserRepository.ChangeUserStatus(user, UserStatus.Active);
                _unitOfWork.UserRepository.AddMessage(user, $"{user.FirstName} {user.LastName} عزیز اطلاعات حساب کاربری شما تایید شد.");
            }
            await _unitOfWork.CommitAsync();
        }

        public async Task CheckPassengerDocumentExpirationDate()
        {
            await _unitOfWork.PassengerDocumentRepository.ExpireDocuments();

            await _unitOfWork.CommitAsync();
        }
    }
}
