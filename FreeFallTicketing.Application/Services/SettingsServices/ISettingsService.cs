using SkyDiveTicketing.Application.Commands.SettingsCommands;
using SkyDiveTicketing.Application.DTOs.SettingsDTOs;

namespace SkyDiveTicketing.Application.Services.SettingsServices
{
    public interface ISettingsService
    {
        Task<SettingsDTO> GetSettings();
        Task Update(SettingsCommand command);
    }
}
