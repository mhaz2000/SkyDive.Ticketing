using SkyDiveTicketing.Application.Base;
using SkyDiveTicketing.Application.Commands.UserCommands;
using SkyDiveTicketing.Application.DTOs.UserDTOs;

namespace SkyDiveTicketing.Application.Services.UserServices
{
    public interface IUserService
    {
        Task<string> Register(CreateUserCommand command);

        Task OtpRequest(OtpUserCommand command);

        Task Update(string id, CreateUserCommand command);

        UserDTO GetUser(string id);

        IEnumerable<UserDTO> GetUsers(string search);

        Task<string> OtpRegisterConfirmation(OtpUserConfirmationCommand command);

        Task CompeleteUserPersonalInformation(UserPersonalInformationCompletionCommand command, bool registration);

        Task CompeleteUserSecurityInformation(UserSecurityInformationCompletionCommand command);

        Task ResetPassword(UserResetPasswordCommand command);

        Task<UserLoginDto> LoginUser(LoginCommand command, JwtIssuerOptionsModel jwtIssuerOptions);

        Task<UserLoginDto> OtpLoginUser(OtpUserConfirmationCommand command, JwtIssuerOptionsModel jwtIssuerOptions);

        Task InactivateUser(UserCommand command);

        Task CheckPersonalInformation(UserCheckPersonalInformationCommand command);

        Task AssignUserType(AssignUserTypeCommand command);

        Task<bool> CheckIfUserIsActive(Guid id);

        Task<UserInformationDTO> GetUserInformation(Guid userId);

        Task<UserPersonalInformationDTO> GetPersonalInformation(Guid userId);

        Task<UserDocumentsDTO> GetUserDocuments(Guid userId);
    }
}
