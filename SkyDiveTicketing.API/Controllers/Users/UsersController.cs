using FluentValidation;
using SkyDiveTicketing.API.Base;
using SkyDiveTicketing.Application.Base;
using SkyDiveTicketing.Application.Commands.UserCommands;
using SkyDiveTicketing.Application.Services.UserServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkyDiveTicketing.Application.Services.PassengerServices;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace SkyDiveTicketing.API.Controllers.Users
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ApiControllerBase
    {
        private IUserService _userService;
        private readonly JwtIssuerOptionsModel _jwtIssuerOptions;

        public UsersController(IUserService userService)
        {
            var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appSettings.json")
            .Build();

            _userService = userService;
            _jwtIssuerOptions = config.Get<AppSettingsModel>().JwtIssuerOptions;
        }

        [HttpPost("Register")]
        [AllowAnonymous]
        public async Task<IActionResult> Create(CreateUserCommand command)
        {
            try
            {
                command.Validate();

                await _userService.Register(command);
                return OkResult("ثبت نام شما با موفقیت انجام شد.");
            }
            catch (ManagedException e)
            {
                return BadRequest(e.Message);
            }
            catch (ValidationException e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw new SystemException("متاسفانه خطای سیستمی رخ داده");
            }
        }

        [HttpPost("OtpRequest")]
        [AllowAnonymous]
        public async Task<IActionResult> OtpRequest(OtpUserCommand command)
        {
            try
            {
                command.Validate();

                await _userService.OtpRequest(command);
                return OkResult("ارسال پیامک با موفقیت انجام شد.");
            }
            catch (ManagedException e)
            {
                return BadRequest(e.Message);
            }
            catch (ValidationException e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw new SystemException("متاسفانه خطای سیستمی رخ داده");
            }
        }

        [HttpPost("OtpRegisterConfirmation")]
        [AllowAnonymous]
        public async Task<IActionResult> OtpRegisterConfirmation(OtpUserConfirmationCommand command)
        {
            try
            {
                command.Validate();

                var userId = await _userService.OtpRegisterConfirmation(command);
                return OkResult("کد وارد شده صحیح می‌باشد.", userId);
            }
            catch (ManagedException e)
            {
                return BadRequest(e.Message);
            }
            catch (ValidationException e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw new SystemException("متاسفانه خطای سیستمی رخ داده");
            }
        }

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
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw new SystemException("متاسفانه خطای سیستمی رخ داده");
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
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw new SystemException("متاسفانه خطای سیستمی رخ داده");
            }
        }

        [HttpPut("ResetPassword")]
        public async Task<IActionResult> ResetPassword(UserResetPasswordCommand command)
        {
            try
            {
                command.Validate();

                await _userService.ResetPassword(command);
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
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw new SystemException("متاسفانه خطای سیستمی رخ داده");
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
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw new SystemException("متاسفانه خطای سیستمی رخ داده");

            }
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginCommand command)
        {
            try
            {
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
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw new SystemException("متاسفانه خطای سیستمی رخ داده");
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
        public async Task<IActionResult> OtpLogin(OtpUserConfirmationCommand command)
        {
            try
            {
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
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw new SystemException("متاسفانه خطای سیستمی رخ داده");
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
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw new SystemException("متاسفانه خطای سیستمی رخ داده");
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
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw new SystemException("متاسفانه خطای سیستمی رخ داده");
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
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw new SystemException("متاسفانه خطای سیستمی رخ داده");
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
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw new SystemException("متاسفانه خطای سیستمی رخ داده");
            }
        }

    }
}
