﻿using SkyDiveTicketing.Application.Base;
using SkyDiveTicketing.Core.Entities;
using SkyDiveTicketing.Core.Repositories.Base;

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

        public async Task CheckPassengerDocumentExpirationDate()
        {
            await _unitOfWork.PassengerAttorneyDocumentRepository.ExpireDocuments();
            await _unitOfWork.PassengerMedicalDocumentRepository.ExpireDocuments();

            await _unitOfWork.CommitAsync();
        }
    }
}
