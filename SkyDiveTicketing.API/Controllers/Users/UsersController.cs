using FluentValidation;
using SkyDiveTicketing.API.Base;
using SkyDiveTicketing.Application.Base;
using SkyDiveTicketing.Application.Commands.UserCommands;
using SkyDiveTicketing.Application.Services.UserServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkyDiveTicketing.Application.Services.PassengerServices;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using Newtonsoft.Json.Linq;

namespace SkyDiveTicketing.API.Controllers.Users
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ApiControllerBase
    {
        private readonly IUserService _userService;
        private readonly string _apiKey;
        private readonly string _templateKey;
        private readonly JwtIssuerOptionsModel _jwtIssuerOptions;

        public UsersController(IUserService userService)
        {
            var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appSettings.json")
            .Build();

            _userService = userService;
            _jwtIssuerOptions = config.Get<AppSettingsModel>().JwtIssuerOptions;
            _apiKey = config.Get<AppSettingsModel>().KavenegarApiKey;
            _templateKey = config.Get<AppSettingsModel>().VerificationTemplateKey;
        }

        [HttpPost("Register")]
        [AllowAnonymous]
        public async Task<IActionResult> Create(CreateUserCommand command)
        {
            try
            {
                command.Validate();

                return OkResult("ثبت نام شما با موفقیت انجام شد.", await _userService.Register(command));
            }
            catch (ManagedException e)
            {
                return BadRequest(e.Message);
            }
            catch (ValidationException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("OtpRequest")]
        [AllowAnonymous]
        public async Task<IActionResult> OtpRequest(OtpUserCommand command)
        {
            try
            {
                command.Validate();

                var phone = await _userService.OtpRequest(command, _apiKey, _templateKey);
                return OkResult("ارسال پیامک با موفقیت انجام شد.", phone);
            }
            catch (ManagedException e)
            {
                return BadRequest(e.Message);
            }
            catch (ValidationException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("OtpRequestConfirmation")]
        [AllowAnonymous]
        public async Task<IActionResult> OtpRequestConfirmation(OtpRequestConfirmationCommand command)
        {
            try
            {
                command.Validate();

                var token = await _userService.OtpRequestConfirmation(command, _jwtIssuerOptions);
                return OkResult("کد وارد شده صحیح می‌باشد.", token);
            }
            catch (ManagedException e)
            {
                return BadRequest(e.Message);
            }
            catch (ValidationException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("OtpRegisterConfirmation")]
        [AllowAnonymous]
        public async Task<IActionResult> OtpRegisterConfirmation(OtpUserConfirmationCommand command)
        {
            try
            {
                command.Validate();

                var token = await _userService.OtpRegisterConfirmation(command, _jwtIssuerOptions);
                return OkResult("کد وارد شده صحیح می‌باشد.", token);
            }
            catch (ManagedException e)
            {
                return BadRequest(e.Message);
            }
            catch (ValidationException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("UserSecurityInformationCompletion")]
        public async Task<IActionResult> UserSecurityInformationCompletion(UserSecurityInformationCompletionCommand command)
        {
            try
            {
                command.Validate();

                await _userService.CompeleteUserSecurityInformation(command);
                return OkResult("اطلاعات کاربری شما با موفقیت به ثبت رسید.");
            }
            catch (ManagedException e)
            {
                return BadRequest(e.Message);
            }
            catch (ValidationException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("ResetPassword")]
        public async Task<IActionResult> ResetPassword(UserResetPasswordCommand command)
        {
            try
            {
                command.Validate();

                await _userService.ResetPassword(command, UserId);
                return OkResult("رمز عبور شما با موفقیت تغییر یافت.");
            }
            catch (ManagedException e)
            {
                return BadRequest(e.Message);
            }
            catch (ValidationException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("Inactivate")]
        public async Task<IActionResult> InactiveUser()
        {
            try
            {
                await _userService.InactivateUser(new UserCommand() { Id = UserId });
                return OkResult("حساب کاربری غیر فعال گردید..");
            }
            catch (ManagedException e)
            {
                return BadRequest(e.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginCommand command)
        {
            try
            {
                command.Validate();

                var loginDto = await _userService.LoginUser(command, _jwtIssuerOptions);
                return OkResult("شما با موفقیت وارد شدید.", loginDto);
            }
            catch (NotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (ManagedException e)
            {
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        /// ورود بر اساس sms
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        /// <exception cref="SystemException"></exception>
        [AllowAnonymous]
        [HttpPost("OtpLogin")]
        public async Task<IActionResult> OtpLogin(OtpLoginCommand command)
        {
            try
            {
                command.Validate();

                var loginDto = await _userService.OtpLoginUser(command, _jwtIssuerOptions);
                return OkResult("شما با موفقیت وارد شدید.", loginDto);
            }
            catch (NotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (ManagedException e)
            {
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        /// ارسال اطلاعات تکمیلی کاربر برای فعال سازی حساب کاربری/
        /// </summary>
        /// <param name="command"></param>
        /// <param name="registration"></param>
        /// <returns></returns>
        /// <exception cref="SystemException"></exception>
        [HttpPost("UserPersonalInformationCompletion/{registration}")]
        public async Task<IActionResult> UserPersonalInformationCompletion(UserPersonalInformationCompletionCommand command, bool registration)
        {
            try
            {
                command.Validate();

                if (command.Id is null)
                    command.Id = UserId;

                await _userService.CompeleteUserPersonalInformation(command, registration);
                return OkResult("اطلاعات شخصی شما با موفقیت به ثبت رسید.");
            }
            catch (ManagedException e)
            {
                return BadRequest(e.Message);
            }
            catch (ValidationException e)
            {
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        /// بررسی اینکه آیا حساب کاربر فعال است یا خیر
        /// </summary>
        /// <returns></returns>
        /// <exception cref="SystemException"></exception>
        [HttpGet("CheckIfUserIsActive")]
        public async Task<IActionResult> CheckIfUserIsActive()
        {
            try
            {
                var isActive = await _userService.CheckIfUserIsActive(UserId);
                return OkResult("وضعیت حساب کاربری.", isActive);
            }
            catch (ManagedException e)
            {
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        /// اطلاعات کاربری
        /// </summary>
        /// <returns></returns>
        /// <exception cref="SystemException"></exception>
        [HttpGet("GetUserInformation")]
        public async Task<IActionResult> GetUserInformation()
        {
            try
            {
                var user = await _userService.GetUserInformation(UserId);
                return OkResult("اطلاعات کاربری.", user);
            }
            catch (ManagedException e)
            {
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        /// اطلاعات شخصی
        /// </summary>
        /// <returns></returns>
        /// <exception cref="SystemException"></exception>
        [HttpGet("GetPersonalInformation")]
        public async Task<IActionResult> GetPersonalInformation()
        {
            try
            {
                var user = await _userService.GetPersonalInformation(UserId);
                return OkResult("اطلاعات شخصی.", user);
            }
            catch (ManagedException e)
            {
                return BadRequest(e.Message);
            }
        }

        [AllowAnonymous]
        [HttpGet("CheckUserExistence/{username}")]
        public async Task<IActionResult> CheckUserExistence(string username)
        {
            try
            {
                await _userService.CheckUserExistence(username);
                return OkResult("کاربر مورد نظر یافت شد.");
            }
            catch (ManagedException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("GetUserDocument")]
        public async Task<IActionResult> GetUserDocument()
        {
            try
            {
                var userDocuments = await _userService.GetUserDocuments(UserId);
                return OkResult("مدارک کاربر.", userDocuments);
            }
            catch (ManagedException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("AcceptTermsAndCondition")]
        public async Task<IActionResult> AcceptingTermsAndConditions()
        {
            try
            {
                await _userService.AcceptingTermsAndConditions(UserId);
                return OkResult("قوانین و شرایط پذیرفته شد.");
            }
            catch (ManagedException e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}