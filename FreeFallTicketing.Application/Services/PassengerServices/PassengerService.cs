using SkyDiveTicketing.Application.Base;
using SkyDiveTicketing.Core.Entities;
using SkyDiveTicketing.Core.Repositories.Base;
using System.Reflection.Metadata;

namespace SkyDiveTicketing.Application.Services.PassengerServices
{
    public class PassengerService : IPassengerService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PassengerService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task CheckUserDocument(Guid documentId, bool isConfirmed)
        {
            var medicalDocument = await _unitOfWork.PassengerMedicalDocumentRepository.GetByIdAsync(documentId);
            var attorneyDocument = await _unitOfWork.PassengerAttorneyDocumentRepository.GetByIdAsync(documentId);
            var logBookDocument = await _unitOfWork.PassengerLogBookDocumentRepository.GetByIdAsync(documentId);
            var nationalCardDocument = await _unitOfWork.PassengerNationalCardDocumentRepository.GetByIdAsync(documentId);

            if (medicalDocument is null && attorneyDocument is null && logBookDocument is null && nationalCardDocument is null)
                throw new ManagedException("مدرک مورد نظر یافت نشد.");

            if (isConfirmed)
            {
                medicalDocument?.SetStatus(DocumentStatus.Confirmed);
                attorneyDocument?.SetStatus(DocumentStatus.Confirmed);
                logBookDocument?.SetStatus(DocumentStatus.Confirmed);
                nationalCardDocument?.SetStatus(DocumentStatus.Confirmed);
            }
            else
            {
                medicalDocument?.SetStatus(DocumentStatus.NotLoaded);
                attorneyDocument?.SetStatus(DocumentStatus.NotLoaded);
                logBookDocument?.SetStatus(DocumentStatus.NotLoaded);
                nationalCardDocument?.SetStatus(DocumentStatus.NotLoaded);
            }

            var user = _unitOfWork.UserRepository.GetAllWithIncludes(c => c.Passenger!.NationalCardDocumentFiles.Any(c => c.Id == documentId) ||
            c.Passenger.AttorneyDocumentFiles.Any(c => c.Id == documentId) ||
            c.Passenger.MedicalDocumentFiles.Any(c=> c.Id == documentId) ||
            c.Passenger.LogBookDocumentFiles.Any(c=> c.Id == documentId)).FirstOrDefault();

            if (!isConfirmed)
            {
                _unitOfWork.UserRepository.ChangeUserStatus(user, UserStatus.AwaitingCompletion);
                await _unitOfWork.UserRepository.AddMessage(user, $"{user.FirstName} {user.LastName} عزیز مدرک بارگذاری شده تایید نشد.", "عدم تایید مدرک");

                //send sms
            }

            await _unitOfWork.CommitAsync();
        }

        public async Task RemoveDocument(Guid id)
        {
            var nationalCardDocument = await _unitOfWork.PassengerNationalCardDocumentRepository.GetByIdAsync(id);
            var attorneyDocument = await _unitOfWork.PassengerAttorneyDocumentRepository.GetByIdAsync(id);
            var logBookDocument = await _unitOfWork.PassengerLogBookDocumentRepository.GetByIdAsync(id);
            var medicalDocument = await _unitOfWork.PassengerMedicalDocumentRepository.GetByIdAsync(id);

            var user = _unitOfWork.UserRepository.GetAllWithIncludes(c => c.Passenger!.NationalCardDocumentFiles.Any(c => c.Id == id) ||
                c.Passenger.AttorneyDocumentFiles.Any(c => c.Id == id) ||
                c.Passenger.MedicalDocumentFiles.Any(c => c.Id == id) ||
                c.Passenger.LogBookDocumentFiles.Any(c => c.Id == id)).FirstOrDefault();

            if (nationalCardDocument is not null)
            {
                _unitOfWork.PassengerNationalCardDocumentRepository.ChangeUserStatusIfNeeded(nationalCardDocument, user);
                _unitOfWork.PassengerNationalCardDocumentRepository.Remove(nationalCardDocument);
            }
            else if (attorneyDocument is not null)
            {
                _unitOfWork.PassengerAttorneyDocumentRepository.ChangeUserStatusIfNeeded(attorneyDocument, user);
                _unitOfWork.PassengerAttorneyDocumentRepository.Remove(attorneyDocument);
            }
            else if (logBookDocument is not null)
            {
                _unitOfWork.PassengerLogBookDocumentRepository.ChangeUserStatusIfNeeded(logBookDocument, user);
                _unitOfWork.PassengerLogBookDocumentRepository.Remove(logBookDocument);
            }
            else if (medicalDocument is not null)
            {
                _unitOfWork.PassengerMedicalDocumentRepository.ChangeUserStatusIfNeeded(medicalDocument, user);
                _unitOfWork.PassengerMedicalDocumentRepository.Remove(medicalDocument);
            }
            else
                throw new ManagedException("مدرک مورد نظر یافت نشد.");

            await _unitOfWork.CommitAsync();
        }

        public async Task CheckPassengerDocumentExpirationDate()
        {
            await _unitOfWork.PassengerAttorneyDocumentRepository.ExpireDocuments();
            await _unitOfWork.PassengerMedicalDocumentRepository.ExpireDocuments();

            await _unitOfWork.CommitAsync();
        }
    }
}
