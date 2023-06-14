using SkyDiveTicketing.Application.Base;
using SkyDiveTicketing.Application.Commands.UserCommands;
using SkyDiveTicketing.Application.DTOs.UserDTOs;

namespace SkyDiveTicketing.Application.Services.UserServices
{
    public interface IUserService
    {
        Task<Guid> Register(CreateUserCommand command);

        Task OtpRequest(OtpUserCommand command);

        Task Update(UpdateUserCommand command);

        UserDTO GetUser(Guid id);

        IEnumerable<UserDTO> GetUsers(string search);

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
    }
}
