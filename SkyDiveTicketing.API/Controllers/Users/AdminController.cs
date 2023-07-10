using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkyDiveTicketing.API.Base;
using SkyDiveTicketing.API.Extensions;
using SkyDiveTicketing.Application.Base;
using SkyDiveTicketing.Application.Commands.UserCommands;
using SkyDiveTicketing.Application.Services.AdminCartableServices;
using SkyDiveTicketing.Application.Services.PassengerServices;
using SkyDiveTicketing.Application.Services.UserServices;
using SkyDiveTicketing.Application.Services.UserTypeServices;
using SkyDiveTicketing.Core.Entities;
using System.Reflection.Metadata;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace SkyDiveTicketing.API.Controllers.Users
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AdminController : ApiControllerBase
    {
        private readonly IUserService _userService;
        private readonly IAdminCartableService _adminCartableService;
        private readonly IPassengerService _passengerService;
        private readonly IUserTypeService _userTypeService;

        public AdminController(IUserService userService, IPassengerService passengerService, IUserTypeService userTypeService,
            IAdminCartableService adminCartableService)
        {
            _userService = userService;
            _passengerService = passengerService;
            _userTypeService = userTypeService;
            _adminCartableService = adminCartableService;
        }

        [HttpPost("InactivateUser")]
        public async Task<IActionResult> InactivateUser(UserCommand command)
        {
            try
            {
                command.Validate();

                await _userService.InactivateUser(command);
                return OkResult("کاربر مورد نظر غیر فعال گردید.");
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

        [HttpPut("CheckUserPersonalInformation")]
        public async Task<IActionResult> CheckUserPersonalInformation(UserCheckPersonalInformationCommand command)
        {
            try
            {
                command.Validate();

                await _userService.CheckPersonalInformation(command);
                return OkResult(command.IsConfirmed ? "اطلاعات کاربر با موفقیت تایید شد." : "مدارک کاربر رد شد.");
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
                Console.WriteLine(ex + "\n----------------------");
                return BadRequest("متاسفانه خطای سیستمی رخ داده");
            }
        }

        [HttpPut("CheckUserDocument/{documentId}/{isConfirmed}")]
        public async Task<IActionResult> CheckUserDocument(Guid documentId, bool isConfirmed)
        {
            try
            {
                await _passengerService.CheckUserDocument(documentId, isConfirmed);
                return OkResult(isConfirmed ? "مدرک کاربر با موفقیت تایید شد." : "مدرک کاربر رد شد.");
            }
            catch (ManagedException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("UpdateUser/{id}")]
        public async Task<IActionResult> UpdateUser(AdminUserCommand command, Guid id)
        {
            try
            {
                command.Validate();

                await _userService.Update(command, id);
                return OkResult("اطلاعات کاربر ویرایش شد.");
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

        [HttpPut("UpdateUserPassword/{id}")]
        public async Task<IActionResult> UpdateUserPassword(Guid id, UserResetPasswordCommand command)
        {
            try
            {
                command.Validate();

                await _userService.ResetPassword(command, id, true);
                return OkResult("اطلاعات کاربر ویرایش شد.");
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

        [HttpGet("GetUsers")]
        public IActionResult GetUsers([FromQuery] PageQuery pageQuery, string? minDate, string? maxDate, UserStatus? userStatus, string? search = "")
        {
            try
            {
                var min = minDate?.ToDateTime();
                var max = maxDate?.ToDateTime();

                var users = _userService.GetUsers(search ?? string.Empty, min, max, userStatus);
                return OkResult("اطلاعات کاربران", users.ToPagingAndSorting(pageQuery), users.Count());
            }
            catch (ManagedException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("UserDetail/{id}")]
        public async Task<IActionResult> GetUserDetail(Guid id)
        {
            try
            {
                var user = await _userService.GetUserDetail(id);
                return OkResult("اطلاعات کاربر", user);
            }
            catch (ManagedException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("CreateUser")]
        public async Task<IActionResult> CreateUser(AdminUserCommand command)
        {
            try
            {
                command.Validate();

                await _userService.CreateUser(command);
                return OkResult("کاربر جدید با موفقیت ایجاد شد.");
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

        [HttpGet("AdminCartableMessages")]
        public IActionResult GetAdminCartableMessages([FromQuery] PageQuery pageQuery, RequestType? requestType, string? search)
        {
            try
            {
                var messages = _adminCartableService.GetAdminCartableMessages(requestType, search);
                return OkResult("کارتابل رسیدگی ادمین", messages.ToPagingAndSorting(pageQuery), messages.Count());
            }
            catch (ManagedException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("RemoveFromCartable/{id}")]
        public async Task<IActionResult> GetAdminCartableMessages(Guid id)
        {
            try
            {
                await _adminCartableService.RemoveMessageFromCartable(id);
                return OkResult("عملیات حذف با موفقیت انجام شد.");
            }
            catch (ManagedException e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
