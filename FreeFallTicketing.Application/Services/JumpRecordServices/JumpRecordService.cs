using SkyDiveTicketing.Application.Base;
using SkyDiveTicketing.Application.Commands.JumpRecordCommands;
using SkyDiveTicketing.Application.DTOs.JumpRecordDTOs;
using SkyDiveTicketing.Core.Repositories.Base;

namespace SkyDiveTicketing.Application.Services.JumpRecordServices
{
    public class JumpRecordService : IJumpRecordService
    {
        private readonly IUnitOfWork _unitOfWork;
        public JumpRecordService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task CheckIfExpired()
        {
            var setting = await _unitOfWork.SettingsRepository.FirstOrDefaultAsync(c => true);
            var expiredJumpRecords = _unitOfWork.JumpRecordRepository.Include(c => c.User)
                .Where(c => Math.Abs((c.Date - DateTime.Now).TotalDays) > setting.JumpDuration * 30);

            foreach (var record in expiredJumpRecords)
            {
                _unitOfWork.UserRepository.AddMessage(record.User,
                    $"{record.User.FirstName} {record.User.LastName} عزیز از آخرین پرش شما {setting.JumpDuration} ماه گذشته است. لطفا اقدامات لازم جهت بارگذاری سوابق پرش را انجام دهید.");

                _unitOfWork.JumpRecordRepository.ExpireJumpRecord(record);
            }

            await _unitOfWork.CommitAsync();
        }

        public async Task ConfirmJumpRecord(Guid id, bool isConfirmed)
        {
            var jumpRecord = await _unitOfWork.JumpRecordRepository.GetFirstWithIncludeAsync(c => c.Id == id, c => c.User);
            if (jumpRecord is null)
                throw new ManagedException("سابقه پرش یافت نشد.");

            if (isConfirmed)
            {
                _unitOfWork.JumpRecordRepository.ConfirmJumpRecord(jumpRecord);
                _unitOfWork.UserRepository.AddMessage(jumpRecord.User, "سابقه پرش توسط ادمین تایید شد.");
            }
            else
                _unitOfWork.UserRepository.AddMessage(jumpRecord.User, "سابقه پرش توسط ادمین رد شد.");

            await _unitOfWork.CommitAsync();
        }

        public async Task Create(JumpRecordCommand command, Guid userId)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(userId);
            if (user is null)
                throw new ManagedException("کاربر مورد نظر یافت نشد.");

            await _unitOfWork.JumpRecordRepository.AddJumpRecord(command.Date, command.Location, command.Equipments, command.PlaneType, command.Height,
                new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, command.Time.Hour, command.Time.Minute, command.Time.Second), command.Description, user);
            await _unitOfWork.CommitAsync();
        }

        public async Task<IEnumerable<JumpRecordDTO>> GetJumpRecords(Guid userId)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(userId);
            if (user is null)
                throw new ManagedException("کاربر مورد نظر یافت نشد.");

            var jumpRecords = await _unitOfWork.JumpRecordRepository.GetListWithIncludeAsync("User", c => c.User == user);

            return jumpRecords.Select(jumpRecord => new JumpRecordDTO(jumpRecord.Id, jumpRecord.CreatedAt,
                jumpRecord.UpdatedAt, jumpRecord.Date, jumpRecord.Location, jumpRecord.Equipments, jumpRecord.PlaneType, jumpRecord.Height,
                new TimeOnly(jumpRecord.Time.Hour, jumpRecord.Time.Minute, jumpRecord.Time.Second), jumpRecord.Description, jumpRecord.Confirmed));
        }
    }
}
