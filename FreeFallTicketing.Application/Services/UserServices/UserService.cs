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
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;

        public UserService(IUnitOfWork unitOfWork, RoleManager<IdentityRole<Guid>> roleManager,
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
                c => c.Passenger!.AttorneyDocumentFiles,
                c => c.Passenger!.NationalCardDocumentFiles,
                c => c.Passenger!.LogBookDocumentFiles,
                c => c.Passenger!.MedicalDocumentFiles,
                c => c.Messages!
            };

            var user = _unitOfWork.UserRepository.Include(includeExpressions).FirstOrDefault(c => c.Id == command.Id && c.Status != UserStatus.Inactive);
            if (user is null)
                throw new ManagedException("کاربری یافت نشد.");

            _unitOfWork.UserRepository.CompeleteUserPersonalInfo(command.FirstName ?? string.Empty, command.LastName ?? string.Empty, command.NationalCode ?? string.Empty,
                command.BirthDate, user);

            if (registration)
            {
                await _unitOfWork.UserRepository.AddMessage(user, $"{command.FirstName} {command.LastName} عزیز لطفا جهت تهیه بلیت اطلاعات کاربری خود را تکمیل نمایید.", "تکمیل پروفایل کاربری");
                //send message to user for compleing personal information
            }
            else
            {
                await OtherPersonalInformation(user, command);
                UploadDocument(user, command.MedicalDocument, command.LogBookDocument, command.AttorneyDocument, command.NationalCardDocument);

                await _unitOfWork.UserRepository.AddMessage(user, "اطلاعات حساب کاربری جهت رسیدگی و تایید ارسال شد.", "در خواست تایید پروفایل کاربری");
                await _unitOfWork.AdminCartableRepository.AddToCartable($"اطلاعات حساب کاربری {user.FirstName} {user.LastName} ثبت شد، لطفا نسبت به تایید یا رد آن ها اقدام فرمایید.",
                    user, RequestType.UserInformationConfirmation);

                _unitOfWork.UserRepository.ChangeUserStatus(user, UserStatus.Pending);
            }

            await _unitOfWork.CommitAsync();
        }

        public async Task CompeleteUserSecurityInformation(UserSecurityInformationCompletionCommand command)
        {
            var user = await _unitOfWork.UserRepository.FirstOrDefaultAsync(c => c.Id == command.Id && c.Status != UserStatus.Inactive);
            if (user is null)
                throw new ManagedException("کاربری یافت نشد.");

            var checkUsernameDuplication = await _unitOfWork.UserRepository.AnyAsync(c => c.UserName == command.Username);
            if (checkUsernameDuplication)
                throw new ManagedException("نام کاربری تکراری است.");

            _unitOfWork.UserRepository.CompeleteUserSecurityInfo(command.Username, command.Password, user);
            await _unitOfWork.CommitAsync();
        }

        public IEnumerable<UserDTO> GetUsers(string search, DateTime? minDate, DateTime? maxDate, UserStatus? userStatus)
        {
            var users = _unitOfWork.UserRepository.Include(c => c.UserType)
                 .Where(c => (c.NationalCode?.Contains(search) ?? false) || (c.FullName?.Contains(search) ?? false));

            var adminUsers = _unitOfWork.RoleRepository.GetAdminUsers();
            users = users.Where(c => !adminUsers.Contains(c.Id));

            if (minDate is not null && maxDate is not null)
                users = users.Where(c => c.CreatedAt >= minDate && c.CreatedAt <= maxDate);

            if (userStatus is not null)
                users = users.Where(c => c.Status == userStatus);

            return users.Select(user => new UserDTO(user.Id, user.UserName, user.PhoneNumber, user.Email, user.FirstName,
                user.LastName, user.Status, user.Status.GetDescription(), user.Code, user.UserType?.Title, user.CreatedAt,
                user.UpdatedAt, user.NationalCode, user.BirthDate));
        }

        public async Task<UserLoginDto> LoginUser(LoginCommand command, JwtIssuerOptionsModel jwtIssuerOptions)
        {
            string username = command.Username!;
            if ((command.Username.StartsWith("09") || command.Username.StartsWith("+98") || command.Username.StartsWith("0098")) && !command.Username.Contains("@"))
                username = FixingPhoneNumber(username);

            var user = await _unitOfWork.UserRepository
                .FirstOrDefaultAsync(c => (c.UserName == username || c.PhoneNumber == username || c.Email == username) &&
                c.Status != UserStatus.Inactive);

            if (user is null)
                throw new ManagedException("کاربری یافت نشد.");

            if (user.LoginFailedAttempts >= 5)
                throw new ManagedException("ورود شما 5 مرتبه با خطا مواجه گشت، لطفا از طریق شماره موبایل اقدام به ورود فرمایید.");

            if (!await _userManager.CheckPasswordAsync(user, command.Password))
            {
                _unitOfWork.UserRepository.LoginFailed(user);
                await _unitOfWork.CommitAsync();

                throw new ManagedException("رمز عبور اشتباه می‌باشد.");
            }

            _unitOfWork.UserRepository.LoginSucceeded(user);

            var userRoles = _unitOfWork.RoleRepository.GetUserRoles(user).ToList();
            var token = _tokenGenerator.TokenGeneration(user, jwtIssuerOptions, userRoles);

            await _unitOfWork.CommitAsync();

            return new UserLoginDto()
            {
                IsAdmin = userRoles.Any(c => c.Name.ToLower() == "admin"),
                TokenType = token.TokenType,
                ExpiresIn = token.expires_in,
                AuthToken = token.AuthToken,
                RefreshToken = token.RefreshToken,
                PersonalInformationCompleted = user.PersonalInformationIsCompeleted,
                SecurityInformationCompleted = user.SecurityInformationIsCompeleted,
            };
        }

        public async Task<UserLoginDto> OtpRegisterConfirmation(OtpUserConfirmationCommand command, JwtIssuerOptionsModel jwtIssuerOptions)
        {
            string fixNumber = FixingPhoneNumber(command.Phone!);

            var user = _unitOfWork.UserRepository.FirstOrDefault(c => c.PhoneNumber == fixNumber && c.Status != UserStatus.Inactive);
            if (user is null)
                throw new ManagedException("کاربری با این شماره موبایل یافت نشد.");

            if (user.OtpCode == command.Code && Math.Abs((user.OtpRequestTime - DateTime.Now).Value.TotalMinutes) < 1)
            {
                _unitOfWork.UserRepository.ChangeUserStatus(user, UserStatus.AwaitingCompletion);
                _unitOfWork.UserRepository.PhoneConfirmed(user);
            }
            else
                throw new ManagedException("کد وارد شده اشتباه است.");

            var userRoles = _unitOfWork.RoleRepository.GetUserRoles(user).ToList();
            var token = _tokenGenerator.TokenGeneration(user, jwtIssuerOptions, userRoles);

            await _unitOfWork.CommitAsync();

            return new UserLoginDto()
            {
                IsAdmin = userRoles.Any(c => c.Name.ToLower() == "admin"),
                TokenType = token.TokenType,
                ExpiresIn = token.expires_in,
                AuthToken = token.AuthToken,
                RefreshToken = token.RefreshToken,
                PersonalInformationCompleted = user.PersonalInformationIsCompeleted,
                SecurityInformationCompleted = user.SecurityInformationIsCompeleted,
            };
        }

        public async Task<UserLoginDto> OtpLoginUser(OtpLoginCommand command, JwtIssuerOptionsModel jwtIssuerOptions)
        {
            string username = command.Username;
            if ((command.Username.StartsWith("09") || command.Username.StartsWith("+98") || command.Username.StartsWith("0098")) && !command.Username.Contains("@"))
                username = FixingPhoneNumber(username);

            var user = _unitOfWork.UserRepository
                .FirstOrDefault(c => (c.PhoneNumber == username || c.UserName == username || c.Email == username)
                && c.Status != UserStatus.Inactive);

            if (user is null)
                throw new ManagedException("کاربری با این اطلاعات یافت نشد یافت نشد.");

            if (user.OtpCode == command.Code && Math.Abs((user.OtpRequestTime - DateTime.Now).Value.TotalMinutes) < 1)
            {
                _unitOfWork.UserRepository.ResetFailedAttempts(user);

                var userRoles = _unitOfWork.RoleRepository.GetUserRoles(user).ToList();
                var token = _tokenGenerator.TokenGeneration(user, jwtIssuerOptions, userRoles);

                await _unitOfWork.CommitAsync();

                return new UserLoginDto()
                {
                    IsAdmin = userRoles.Any(c => c.Name.ToLower() == "admin"),
                    TokenType = token.TokenType,
                    ExpiresIn = token.expires_in,
                    AuthToken = token.AuthToken,
                    RefreshToken = token.RefreshToken,
                    PersonalInformationCompleted = user.PersonalInformationIsCompeleted,
                    SecurityInformationCompleted = user.SecurityInformationIsCompeleted,
                };

            }
            else
                throw new ManagedException("کد وارد شده اشتباه است.");
        }

        public async Task<string> OtpRequest(OtpUserCommand command, string apiKey, string templateKey)
        {
            string username = command.Username;
            if ((command.Username.StartsWith("09") || command.Username.StartsWith("+98") || command.Username.StartsWith("0098")) && !command.Username.Contains("@"))
                username = FixingPhoneNumber(username);

            var user = await _unitOfWork.UserRepository
                .FirstOrDefaultAsync(c => (c.PhoneNumber == username || c.Email == username || c.UserName == username)
                && c.Status != UserStatus.Inactive);

            if (user is null)
                throw new ManagedException("کاربری با این اطلاعات یافت نشد.");

            if (user.OtpRequestTime is not null && Math.Abs((user.OtpRequestTime - DateTime.Now).Value.TotalMinutes) < 1)
            {
                throw new ManagedException("از زمان درخواست شما کمتر از 1 دقیقه گذشته است.");
            }

            Random random = new Random();
            var otpCode = random.NextInt64(100000, 999999).ToString();

            //send via otp
            SMSHelpler.SendOtp(user.PhoneNumber, otpCode, apiKey, templateKey);

            _unitOfWork.UserRepository.SetOtpCode(user, otpCode);
            await _unitOfWork.CommitAsync();

            var phone = user.PhoneNumber.Substring(0, user.PhoneNumber.Length - 7) + "*****" + user.PhoneNumber.Substring(user.PhoneNumber.Length - 2);

            return phone;
        }

        public async Task<Guid> Register(CreateUserCommand command)
        {
            string fixNumber = FixingPhoneNumber(command.Phone!);

            var duplicationCheck = await _unitOfWork.UserRepository.AnyAsync(c => c.PhoneNumber == fixNumber
                && c.PhoneNumberConfirmed);

            if (duplicationCheck)
                throw new ManagedException("این شماره قبلا ثبت شده است.");

            var userId = await _unitOfWork.UserRepository.CreateAsync(fixNumber);
            await _unitOfWork.CommitAsync();

            return userId;
        }

        public async Task ResetPassword(UserResetPasswordCommand command, Guid userId, bool adminPrivileged = false)
        {
            var user = await _unitOfWork.UserRepository.FirstOrDefaultAsync(c => c.Id == userId && (c.Status != UserStatus.Inactive || adminPrivileged));
            if (user is null)
                throw new ManagedException("کاربری یافت نشد.");

            _unitOfWork.UserRepository.UpdateUserPassword(command.Password, user);
            await _unitOfWork.CommitAsync();
        }

        public async Task Update(AdminUserCommand command, Guid userId)
        {
            var errors = new List<string>();

            var user = await _unitOfWork.UserRepository.GetFirstWithIncludeAsync(c => c.Id == userId, c => c.Passenger);
            if (user is null)
                throw new ManagedException("کاربر مورد نظر یافت نشد.");

            var userType = await _unitOfWork.UserTypeRepository.GetByIdAsync(command.UserTypeId);
            if (userType is null)
                throw new ManagedException("نوع کاربری یافت نشد.");

            var phoneDuplicationCheck = await _unitOfWork.UserRepository.AnyAsync(c => c.PhoneNumber.ToLower() == command.Phone.ToLower() &&
                c.Status != UserStatus.Inactive && c.Id != userId);

            var usernameDuplicationCheck = await _unitOfWork.UserRepository.AnyAsync(c => c.UserName == command.Username && c.Id != userId);

            var emailDuplicationCheck = await _unitOfWork.UserRepository.AnyAsync(c => !string.IsNullOrEmpty(command.Email) && c.Email.ToLower() == command.Email.ToLower()
                && c.Status != UserStatus.Inactive && c.Id != userId);

            if (phoneDuplicationCheck)
                errors.Add("شماره موبایل تکراری است.");

            if (usernameDuplicationCheck)
                errors.Add("نام کاربری تکراری است.");

            if (emailDuplicationCheck)
                errors.Add("ایمیل تکراری است.");

            if (errors.Any())
                throw new ManagedException(string.Join("\n", errors));

            _unitOfWork.UserRepository.UpdateUser(user, command.Weight, command.Height, command.CityAndState, command.LastName, command.FirstName,
                command.NationalCode, command.EmergencyPhone, command.Address, command.BirthDate, command.EmergencyContact, command.Email,
                command.Phone, command.Username);

            _unitOfWork.UserRepository.AssignUserType(user, userType);

            await _unitOfWork.CommitAsync();
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

            if (command.IsConfirmed)
            {
                await _unitOfWork.UserRepository.AddMessage(user, $"{user.FirstName} {user.LastName} عزیز اطلاعات حساب کاربری شما تایید شد.", "تایید حساب کاربری");
                //sending sms
            }
            else
                await _unitOfWork.UserRepository.AddMessage(user, $"{command.Message}", "عدم تایید حساب کاربری");

            await _unitOfWork.CommitAsync();
        }

        public async Task<bool> CheckIfUserIsActive(Guid id)
        {
            var user = await _unitOfWork.UserRepository.FirstOrDefaultAsync(c => c.Id == id && c.Status != UserStatus.Inactive);
            if (user is null || user.Status != UserStatus.Active)
                return false;

            return true;
        }

        public async Task<UserInformationDTO> GetUserInformation(Guid userId)
        {
            var user = await _unitOfWork.UserRepository.GetFirstWithIncludeAsync(c => c.Id == userId && c.Status != UserStatus.Inactive, c => c.UserType);
            if (user is null)
                throw new ManagedException("کاربری یافت نشد.");

            return new UserInformationDTO(userId, user.CreatedAt, user.UpdatedAt, user.Code, user.UserName, user.PhoneNumber,
                user.Status.GetDescription(), user.Status, user.UserType?.Title, user.FirstName, user.LastName);
        }

        public async Task<UserPersonalInformationDTO> GetPersonalInformation(Guid userId)
        {
            var user = await _unitOfWork.UserRepository.GetFirstWithIncludeAsync(c => c.Id == userId && c.Status != UserStatus.Inactive, c => c.Passenger);
            if (user is null)
                throw new ManagedException("کاربری یافت نشد.");

            return new UserPersonalInformationDTO(user.Id, user.CreatedAt, user.UpdatedAt, user.NationalCode, user.BirthDate, user.FirstName, user.LastName,
                user.Email, user.Passenger?.CityAndState, user.Passenger?.Address, user.Passenger?.Weight,
                user.Passenger?.Height, user.Passenger?.EmergencyContact, user.Passenger?.EmergencyPhone);
        }

        public async Task<UserDocumentsDTO> GetUserDocuments(Guid userId)
        {
            Expression<Func<User, object>>[] includeExpressions = {
                c=> c.Passenger!,
                c => c.Passenger!.AttorneyDocumentFiles,
                c => c.Passenger!.NationalCardDocumentFiles,
                c=> c.Passenger!.LogBookDocumentFiles,
                c=> c.Passenger!.MedicalDocumentFiles
            };

            var user = await _unitOfWork.UserRepository.GetFirstWithIncludeAsync(c => c.Id == userId && c.Status != UserStatus.Inactive, includeExpressions);
            if (user is null)
                throw new ManagedException("کاربری یافت نشد.");

            return new UserDocumentsDTO(userId, user.CreatedAt, user.UpdatedAt)
            {
                AttorneyDocuments = user.Passenger?.AttorneyDocumentFiles.Select(document =>
                    new UserDocumentDetailDTO(document.Id, document.CreatedAt, document.UpdatedAt, document.FileId, document.ExpirationDate,
                    document.Status.GetDescription(), document.Status))!,

                MedicalDocuments = user.Passenger?.MedicalDocumentFiles.Select(document =>
                    new UserDocumentDetailDTO(document.Id, document.CreatedAt, document.UpdatedAt, document.FileId, document.ExpirationDate,
                    document.Status.GetDescription(), document.Status))!,

                LogBookDocuments = user.Passenger?.LogBookDocumentFiles.Select(document =>
                    new UserDocumentDetailDTO(document.Id, document.CreatedAt, document.UpdatedAt, document.FileId, document.ExpirationDate,
                    document.Status.GetDescription(), document.Status))!,

                NationalCardDocuments = user.Passenger?.NationalCardDocumentFiles.Select(document =>
                    new UserDocumentDetailDTO(document.Id, document.CreatedAt, document.UpdatedAt, document.FileId, document.ExpirationDate,
                    document.Status.GetDescription(), document.Status))!
            };
        }

        public async Task CheckUserExistence(string username)
        {
            var user = await _unitOfWork.UserRepository.FirstOrDefaultAsync(c => c.UserName == username || c.PhoneNumber == username || c.Email == username);
            if (user is null)
                throw new ManagedException("کاربری یافت نشد.");

        }

        public async Task CreateUser(AdminUserCommand command)
        {
            var errors = new List<string>();

            var phoneDuplicationCheck = await _unitOfWork.UserRepository.AnyAsync(c => c.PhoneNumber.ToLower() == command.Phone.ToLower() && c.Status != UserStatus.Inactive);
            var usernameDuplicationCheck = await _unitOfWork.UserRepository.AnyAsync(c => c.UserName == command.Username);
            var emailDuplicationCheck = await _unitOfWork.UserRepository
                .AnyAsync(c => !string.IsNullOrEmpty(command.Email) && c.Email.ToLower() == command.Email.ToLower() && c.Status != UserStatus.Inactive);

            if (phoneDuplicationCheck)
                errors.Add("شماره موبایل تکراری است.");

            if (usernameDuplicationCheck)
                errors.Add("نام کاربری تکراری است.");

            if (emailDuplicationCheck)
                errors.Add("ایمیل تکراری است.");

            var userType = await _unitOfWork.UserTypeRepository.GetByIdAsync(command.UserTypeId);
            if (userType is null)
                throw new ManagedException("نوع کاربری یافت نشد.");

            if (errors.Any())
                throw new ManagedException(string.Join("\n", errors));

            var user = await _unitOfWork.UserRepository.AddUser(command.Password, command.NationalCode, command.Height, command.Weight, command.FirstName, command.LastName,
                command.Email, command.BirthDate, command.Phone, command.Username, command.Address, command.EmergencyContact, command.EmergencyPhone,
                command.CityAndState);

            _unitOfWork.UserRepository.AssignUserType(user, userType);

            await _unitOfWork.CommitAsync();
        }

        public async Task<UserLoginDto> OtpRequestConfirmation(OtpRequestConfirmationCommand command, JwtIssuerOptionsModel jwtIssuerOptions)
        {
            var user = _unitOfWork.UserRepository.FirstOrDefault(c => c.PhoneNumber == command.Phone && c.Status != UserStatus.Inactive);

            if (user is null)
                throw new ManagedException("کاربری با این اطلاعات یافت نشد یافت نشد.");

            if (user.OtpCode == command.Code && Math.Abs((user.OtpRequestTime - DateTime.Now).Value.TotalMinutes) < 1)
            {
                var userRoles = _unitOfWork.RoleRepository.GetUserRoles(user).ToList();
                var token = _tokenGenerator.TokenGeneration(user, jwtIssuerOptions, userRoles);

                await _unitOfWork.CommitAsync();

                return new UserLoginDto()
                {
                    IsAdmin = userRoles.Any(c => c.Name.ToLower() == "admin"),
                    TokenType = token.TokenType,
                    ExpiresIn = token.expires_in,
                    AuthToken = token.AuthToken,
                    RefreshToken = token.RefreshToken,
                    PersonalInformationCompleted = user.PersonalInformationIsCompeleted,
                    SecurityInformationCompleted = user.SecurityInformationIsCompeleted,
                };

            }
            else
                throw new ManagedException("کد وارد شده اشتباه است.");
        }

        public async Task AcceptingTermsAndConditions(Guid userId)
        {
            var user = _unitOfWork.UserRepository.FirstOrDefault(c => c.Id == userId && c.Status != UserStatus.Inactive);

            if (user is null)
                throw new ManagedException("کاربری با این اطلاعات یافت نشد یافت نشد.");

            _unitOfWork.UserRepository.AcceptingTermsAndConditions(user);

            await _unitOfWork.CommitAsync();
        }

        public async Task<UserDetailDTO> GetUserDetail(Guid id)
        {
            var user = await _unitOfWork.UserRepository.GetUserWithInclude(c => c.Id == id);
            if (user is null)
                throw new ManagedException("کاربر مورد نظر یافت نشد.");

            var attorneyDocumentFile = user.Passenger.AttorneyDocumentFiles.OrderByDescending(c => c.CreatedAt).FirstOrDefault();
            var medicalDocumentFile = user.Passenger.MedicalDocumentFiles.OrderByDescending(c => c.CreatedAt).FirstOrDefault();
            var logBookDocumentFile = user.Passenger.LogBookDocumentFiles.OrderByDescending(c => c.CreatedAt).FirstOrDefault();
            var nationalCardDocumentFile = user.Passenger.NationalCardDocumentFiles.OrderByDescending(c => c.CreatedAt).FirstOrDefault();

            var documentsConfirmed = attorneyDocumentFile?.Status == DocumentStatus.Confirmed &&
                                     medicalDocumentFile?.Status == DocumentStatus.Confirmed &&
                                     logBookDocumentFile?.Status == DocumentStatus.Confirmed &&
                                     nationalCardDocumentFile.Status == DocumentStatus.Confirmed;

            return new UserDetailDTO(id, user.CreatedAt, user.UpdatedAt, user.UserName, user.FirstName!, user.LastName, user.UserType!.Title, user.UserType.Id,
                user.NationalCode!, user.BirthDate, user.Passenger?.CityAndState!, user.Passenger?.Address!,
                user.Code, user.Email, user.PhoneNumber, user.Passenger?.Height, user.Passenger?.Weight, user.Status, user.Status.GetDescription(),
                user.Passenger?.EmergencyContact, user.Passenger?.EmergencyPhone, documentsConfirmed);
        }

        private string FixingPhoneNumber(string phoneNumber)
        {
            string fixNumber = phoneNumber;

            if (phoneNumber.Contains("+98"))
                fixNumber = phoneNumber.Replace("+98", "0");

            if (phoneNumber.StartsWith("0098"))
                fixNumber = "0" + phoneNumber.Substring(4);

            return fixNumber;
        }

        public async Task UploadDocument(AdminUploadUserDocumentCommand command, Guid id)
        {
            Expression<Func<User, object>>[] includeExpressions = {
                c => c.Passenger!.AttorneyDocumentFiles,
                c => c.Passenger!.NationalCardDocumentFiles,
                c => c.Passenger!.LogBookDocumentFiles,
                c => c.Passenger!.MedicalDocumentFiles,
                c => c.Messages!
            };

            var user = _unitOfWork.UserRepository.Include(includeExpressions).FirstOrDefault(c => c.Id == id && c.Status != UserStatus.Inactive);
            if (user is null)
                throw new ManagedException("کاربری یافت نشد.");

            UploadDocument(user, command.MedicalDocument, command.LogBookDocument, command.AttorneyDocument, command.NationalCardDocument);

            await _unitOfWork.CommitAsync();
        }

        private async Task UploadDocument(User user, UploadDocumentDetailCommand? medicalDocument, UploadDocumentDetailCommand? logBookDocument,
            UploadDocumentDetailCommand? attorneyDocument, UploadDocumentDetailCommand? nationalCardDocument)
        {
            if (nationalCardDocument is not null && nationalCardDocument.FileId is not null)
                await _unitOfWork.PassengerRepository.AddNationalCardDocument(user.Passenger, nationalCardDocument.FileId.Value);

            if (attorneyDocument is not null && attorneyDocument.FileId is not null)
            {
                if (attorneyDocument.ExpirationDate is null)
                    throw new ManagedException("تاریخ انقضای وکالتنامه محضری الزامی است.");

                await _unitOfWork.PassengerRepository.AddAttorneyDocumentAsync(user.Passenger, attorneyDocument.FileId.Value, attorneyDocument.ExpirationDate);
            }

            if (logBookDocument is not null && logBookDocument.FileId is not null)
                await _unitOfWork.PassengerRepository.AddLogBookDocumentAsync(user.Passenger, logBookDocument.FileId.Value);

            if (medicalDocument is not null)
            {
                if (medicalDocument.ExpirationDate is null)
                    throw new ManagedException("تاریخ انقضای مدارک پزشکی الزامی است.");

                await _unitOfWork.PassengerRepository.AddMedicalDocumentAsync(user.Passenger, medicalDocument.FileId.Value, medicalDocument.ExpirationDate);
            }
        }

        private async Task OtherPersonalInformation(User user, UserPersonalInformationCompletionCommand command)
        {
            _unitOfWork.UserRepository.CompeleteOtherUserPersonalInfo(command.Email ?? string.Empty, command.CityAndState, command.Address ?? string.Empty,
                command.EmergencyContact ?? string.Empty, command.EmergencyPhone ?? string.Empty, command.Height, command.Weight, user);

            await Task.CompletedTask;
        }
    }
}
