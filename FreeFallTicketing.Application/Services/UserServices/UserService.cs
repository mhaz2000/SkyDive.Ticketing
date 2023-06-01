using SkyDiveTicketing.Application.Base;
using SkyDiveTicketing.Application.Commands.UserCommands;
using SkyDiveTicketing.Application.DTOs.UserDTOs;
using SkyDiveTicketing.Application.Helpers;
using SkyDiveTicketing.Core.Entities;
using SkyDiveTicketing.Core.Repositories.Base;
using Microsoft.AspNetCore.Identity;
using System.Linq.Expressions;

namespace SkyDiveTicketing.Application.Services.UserServices
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITokenGenerator _tokenGenerator;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserService(IUnitOfWork unitOfWork, RoleManager<IdentityRole> roleManager,
            UserManager<User> userManager, ITokenGenerator tokenGenerator)
        {
            _unitOfWork = unitOfWork;
            _roleManager = roleManager;
            _userManager = userManager;
            _tokenGenerator = tokenGenerator;
        }

        public async Task CompeleteUserPersonalInformation(UserPersonalInformationCompletionCommand command, bool registration)
        {
            Expression<Func<User, object>>[] includeExpressions = {
                c => c.Passenger.AttorneyDocumentFile,
                c => c.Passenger.NationalCardDocumentFile,
                c=> c.Passenger.LogBookDocumentFile,
                c=> c.Passenger.MedicalDocumentFile
            };

            var user = _unitOfWork.UserRepository.Include(includeExpressions).FirstOrDefault(c => c.Id == command.Id && c.Status != UserStatus.Inactive);
            if (user is null)
                throw new ManagedException("کاربری یافت نشد.");

            if (!user.PhoneNumberConfirmed)
                throw new ManagedException("تلفن همراه شما تایید نشده است.");

            _unitOfWork.UserRepository.CompeleteUserPersonalInfo(command.FirstName ?? string.Empty, command.LastName ?? string.Empty, command.NationalCode ?? string.Empty, command.BirthDate, user);

            await OtherPersonalInformation(user, command);
            UploadDocument(user, command);

            if (registration)
                _unitOfWork.UserRepository.AddMessage(user, $"{command.FirstName} {command.LastName} عزیز لطفا جهت تهیه بلیت اطلاعات کاربری خود را تکمیل نمایید.");
            else
            {
                _unitOfWork.UserRepository.AddMessage(user, "اطلاعات حساب کاربری جهت رسیدگی و تایید ارسال شد.");
                _unitOfWork.UserRepository.ChangeUserStatus(user, UserStatus.Pending);
            }


            await _unitOfWork.CommitAsync();
        }

        public async Task CompeleteUserSecurityInformation(UserSecurityInformationCompletionCommand command)
        {
            var user = await _unitOfWork.UserRepository.FirstOrDefaultAsync(c => c.Id == command.Id && c.Status != UserStatus.Inactive);
            if (user is null)
                throw new ManagedException("کاربری یافت نشد.");

            if (!user.PhoneNumberConfirmed)
                throw new ManagedException("تلفن همراه شما تایید نشده است.");

            _unitOfWork.UserRepository.CompeleteUserSecurityInfo(command.Username, command.Password, user);
            await _unitOfWork.CommitAsync();
        }

        public UserDTO GetUser(string id)
        {
            var user = _unitOfWork.UserRepository.Include(c => c.UserType).FirstOrDefault(c => c.Id == id);

            return new UserDTO(user.Id, user.UserName, user.PhoneNumber, user.Email, user.FirstName,
                user.LastName, user.Status, user.Code, user.UserType.Title, user.CreatedAt, user.UpdatedAt);
        }

        public IEnumerable<UserDTO> GetUsers(string search)
        {
            return _unitOfWork.UserRepository.Include(c => c.UserType)
                 .Where(c => c.UserName.Contains(search) || c.FullName.Contains(search))
                 .Select(user => new UserDTO(user.Id, user.UserName, user.PhoneNumber, user.Email, user.FirstName, user.LastName, user.Status, user.Code, user.UserType.Title, user.CreatedAt, user.UpdatedAt));
        }

        public async Task<UserLoginDto> LoginUser(LoginCommand command, JwtIssuerOptionsModel jwtIssuerOptions)
        {
            var user = await _unitOfWork.UserRepository
                .FirstOrDefaultAsync(c => c.PhoneNumber == command.Username && c.Status != UserStatus.Inactive);

            if (user is null)
                throw new ManagedException("کاربری یافت نشد.");

            if (user.LoginFailedAttempts >= 5)
                throw new ManagedException("ورود شما 5 مرتبه با خطا مواجه گشت، لطفا از طریق شماره موبایل اقدام به ورود فرمایید.");

            if (!await _userManager.CheckPasswordAsync(user, command.Password))
            {
                _unitOfWork.UserRepository.LoginFailed(user);
                throw new ManagedException("نام کاربری یا رمز عبور اشتباه می‌باشد.");
            }

            _unitOfWork.UserRepository.LoginSucceeded(user);

            var userRoles = await _userManager.GetRolesAsync(user);
            var token = _tokenGenerator.TokenGeneration(user, jwtIssuerOptions, _roleManager.Roles.Where(c => userRoles.Any(t => t == c.Name)).ToList());

            await _unitOfWork.CommitAsync();

            return new UserLoginDto()
            {
                TokenType = token.TokenType,
                ExpiresIn = token.expires_in,
                AuthToken = token.AuthToken,
                RefreshToken = token.RefreshToken
            };
        }

        public async Task<string> OtpRegisterConfirmation(OtpUserConfirmationCommand command)
        {
            var user = await _unitOfWork.UserRepository.FirstOrDefaultAsync(c => c.PhoneNumber == command.Phone && c.Status != UserStatus.Inactive);
            if (user is null)
                throw new ManagedException("کاربری با این شماره موبایل یافت نشد.");

            if (user.OtpCode == command.Code && (user.OtpRequestTime - DateTime.Now).Value.TotalMinutes < 1)
            {
                _unitOfWork.UserRepository.ChangeUserStatus(user, UserStatus.AwaitingCompletion);
                _unitOfWork.UserRepository.PhoneConfirmed(user);
            }
            else
                throw new ManagedException("کد وارد شده اشتباه است.");

            return user.Id;
        }

        public async Task<UserLoginDto> OtpLoginUser(OtpUserConfirmationCommand command, JwtIssuerOptionsModel jwtIssuerOptions)
        {
            var user = await _unitOfWork.UserRepository.FirstOrDefaultAsync(c => c.PhoneNumber == command.Phone && c.Status != UserStatus.Inactive);
            if (user is null)
                throw new ManagedException("کاربری با این شماره موبایل یافت نشد.");

            if (user.OtpCode == command.Code && (user.OtpRequestTime - DateTime.Now).Value.TotalMinutes < 1)
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                var token = _tokenGenerator.TokenGeneration(user, jwtIssuerOptions, _roleManager.Roles.Where(c => userRoles.Any(t => t == c.Name)).ToList());

                return new UserLoginDto()
                {
                    TokenType = token.TokenType,
                    ExpiresIn = token.expires_in,
                    AuthToken = token.AuthToken,
                    RefreshToken = token.RefreshToken
                };
            }
            else
                throw new ManagedException("کد وارد شده اشتباه است.");
        }

        public async Task OtpRequest(OtpUserCommand command)
        {
            var user = await _unitOfWork.UserRepository.FirstOrDefaultAsync(c => c.PhoneNumber == command.Phone && c.Status != UserStatus.Inactive);
            if (user is null)
                throw new ManagedException("کاربری با این شماره موبایل یافت نشد.");

            if (user.OtpRequestTime is not null && (user.OtpRequestTime - DateTime.Now).Value.TotalMinutes < 1)
            {
                throw new ManagedException("از زمان درخواست شما کمتر از 1 دقیقه گذشته است.");
            }

            Random random = new Random();
            var otpCode = random.NextInt64(100000, 999999).ToString();

            //send via otp

            _unitOfWork.UserRepository.SetOtpCode(user, otpCode);
            await _unitOfWork.CommitAsync();
        }

        public async Task<string> Register(CreateUserCommand command)
        {
            var duplicationCheck = await _unitOfWork.UserRepository.AnyAsync(c => c.PhoneNumber.ToLower() == command.Phone.ToLower()
                && (c.Status == UserStatus.Active || c.Status == UserStatus.AwaitingCompletion));

            if (duplicationCheck)
                throw new ManagedException("این شماره قبلا ثبت شده است.");

            var userId = await _unitOfWork.UserRepository.CreateAsync(command.Phone);
            await _unitOfWork.CommitAsync();

            return userId;
        }

        public async Task ResetPassword(UserResetPasswordCommand command)
        {
            var user = await _unitOfWork.UserRepository.FirstOrDefaultAsync(c => c.Id == command.Id && c.Status != UserStatus.Inactive);
            if (user is null)
                throw new ManagedException("کاربری یافت نشد.");

            _unitOfWork.UserRepository.UpdateUserPassword(command.Password, user);
            await _unitOfWork.CommitAsync();
        }

        public Task Update(string id, CreateUserCommand command)
        {
            throw new NotImplementedException();
        }

        public async Task InactivateUser(UserCommand command)
        {
            var user = await _unitOfWork.UserRepository.FirstOrDefaultAsync(c => c.Id == command.Id && c.Status != UserStatus.Inactive);
            if (user is null)
                throw new ManagedException("کاربری یافت نشد.");

            _unitOfWork.UserRepository.InactivateUser(user);
            await _unitOfWork.CommitAsync();
        }

        public async Task CheckPersonalInformation(UserCheckPersonalInformationCommand command)
        {
            var user = await _unitOfWork.UserRepository.FirstOrDefaultAsync(c => c.Id == command.Id && c.Status != UserStatus.Inactive);
            if (user is null)
                throw new ManagedException("کاربری یافت نشد.");

            _unitOfWork.UserRepository.CheckPersonalInformation(user, command.IsConfirmed);
            await _unitOfWork.CommitAsync();
        }

        private void UploadDocument(User user, UserPersonalInformationCompletionCommand command)
        {
            if (command.NationalCardDocument is not null)
                _unitOfWork.PassengerRepository.AddNationalCardDocument(user.Passenger, command.NationalCardDocument.FileId);

            if (command.AttorneyDocument is not null)
                _unitOfWork.PassengerRepository.AddAttorneyDocument(user.Passenger, command.AttorneyDocument.FileId, command.AttorneyDocument.ExpirationDate);

            if (command.LogBookDocument is not null)
                _unitOfWork.PassengerRepository.AddLogBookDocument(user.Passenger, command.LogBookDocument.FileId);

            if (command.MedicalDocument is not null)
                _unitOfWork.PassengerRepository.AddMedicalDocument(user.Passenger, command.MedicalDocument.FileId, command.MedicalDocument.ExpirationDate);
        }

        private async Task OtherPersonalInformation(User user, UserPersonalInformationCompletionCommand command)
        {
            var city = command.CityId is not null ? await _unitOfWork.CityRepository.GetByIdAsync(command.CityId.Value) : null;
            if (city is null && command.CityId is not null)
                throw new ManagedException("شهر مورد نظر یافت نشد.");

            _unitOfWork.UserRepository.CompeleteOtherUserPersonalInfo(command.Email ?? string.Empty, city, command.Address ?? string.Empty, command.EmergencyContact ?? string.Empty,
                command.EmergencyPhone ?? string.Empty, command.Height, command.Weight, user);
        }
    }
}
