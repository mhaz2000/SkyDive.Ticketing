using SkyDiveTicketing.Application.Base;
using SkyDiveTicketing.Application.Commands.SettingsCommands;
using SkyDiveTicketing.Application.DTOs.SettingsDTOs;
using SkyDiveTicketing.Core.Repositories.Base;

namespace SkyDiveTicketing.Application.Services.SettingsServices
{
    public class SettingsService : ISettingsService
    {
        private readonly IUnitOfWork _unitOfWork;

        public SettingsService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<SettingsDTO> GetSettings()
        {
            var settings = await _unitOfWork.SettingsRepository.GetFirstWithIncludeAsync(includeProperty: c => c.UserStatusInfo);
            if (settings is null)
                throw new ManagedException("تنظیماتی یافت نشد.");

            return new SettingsDTO(settings.Id, settings.CreatedAt, settings.UpdatedAt, 
                settings.UserStatusInfo.Select(s => new UserStatusInfoDTO(s.UserStatus, s.Description)), settings.TermsAndConditionsUrl!,
                settings.RegistrationTermsAndConditionsUrl!, settings.JumpDuration, settings.FileSizeLimitation);
        }

        public async Task Update(SettingsCommand command)
        {
            await _unitOfWork.SettingsRepository.Update(command.TermsAndConditionsUrl ?? string.Empty, command.FileSizeLimitation,
                command.RegistrationTermsAndConditionsUrl ?? string.Empty);

            foreach (var item in command.UserStatusInfo!)
            {
                await _unitOfWork.SettingsRepository.UpdateUserStatusInfo(item.Status, item.Description!);
            }
            
            await _unitOfWork.CommitAsync();
        }
    }
}
