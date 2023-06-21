using SkyDiveTicketing.Application.Base;
using SkyDiveTicketing.Application.Commands.UserCommands;
using SkyDiveTicketing.Application.DTOs.UserDTOs;
using SkyDiveTicketing.Core.Entities;

namespace SkyDiveTicketing.Application.Services.UserServices
{
    public interface IUserService
    {
        Task<Guid> Register(CreateUserCommand command);

        Task<string> OtpRequest(OtpUserCommand command, string apiKey, string templateKey);

        Task Update(AdminUserCommand command, Guid userId);

        IEnumerable<UserDTO> GetUsers(string search, DateTime? minDate, DateTime? maxDate, UserStatus? userStatus);

        Task<UserLoginDto> OtpRegisterConfirmation(OtpUserConfirmationCommand command, JwtIssuerOptionsModel jwtIssuerOptions);

        Task CompeleteUserPersonalInformation(UserPersonalInformationCompletionCommand command, bool registration);

        Task CompeleteUserSecurityInformation(UserSecurityInformationCompletionCommand command);

        Task ResetPassword(UserResetPasswordCommand command, Guid userId);

        Task<UserLoginDto> LoginUser(LoginCommand command, JwtIssuerOptionsModel jwtIssuerOptions);

        Task<UserLoginDto> OtpLoginUser(OtpLoginCommand command, JwtIssuerOptionsModel jwtIssuerOptions);

        Task InactivateUser(UserCommand command);

        Task CheckPersonalInformation(UserCheckPersonalInformationCommand command);

        Task AssignUserType(AssignUserTypeCommand command);

        Task<bool> CheckIfUserIsActive(Guid id);

        Task<UserInformationDTO> GetUserInformation(Guid userId);

        Task<UserPersonalInformationDTO> GetPersonalInformation(Guid userId);

        Task<UserDocumentsDTO> GetUserDocuments(Guid userId);

        Task CheckUserExistence(string username);

        Task CreateUser(AdminUserCommand command);

        Task<UserLoginDto> OtpRequestConfirmation(OtpRequestConfirmationCommand command, JwtIssuerOptionsModel jwtIssuerOptions);

        Task AcceptingTermsAndConditions(Guid userId);
    }
}
