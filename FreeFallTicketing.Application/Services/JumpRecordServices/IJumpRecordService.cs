using SkyDiveTicketing.Application.Commands.JumpRecordCommands;
using SkyDiveTicketing.Application.DTOs.JumpRecordDTOs;

namespace SkyDiveTicketing.Application.Services.JumpRecordServices
{
    public interface IJumpRecordService
    {
        Task CheckIfExpired();
        Task ConfirmJumpRecord(Guid id, bool isConfirmed);
        Task Create(JumpRecordCommand command, Guid userId, Guid createdById);
        Task<IEnumerable<JumpRecordDTO>> GetAllJumpRecords();
        Task<IEnumerable<JumpRecordDTO>> GetJumpRecords(Guid userId);
        Task Remove(Guid id);
    }
}
