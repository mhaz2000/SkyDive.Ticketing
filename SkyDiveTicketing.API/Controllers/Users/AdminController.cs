using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkyDiveTicketing.API.Base;
using SkyDiveTicketing.Application.Base;
using SkyDiveTicketing.Application.Commands.UserCommands;
using SkyDiveTicketing.Application.Services.PassengerServices;
using SkyDiveTicketing.Application.Services.UserServices;

namespace SkyDiveTicketing.API.Controllers.Users
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AdminController : ApiControllerBase
    {
        private readonly IUserService _userService;
        private readonly IPassengerService _passengerService;
        public AdminController(IUserService userService, IPassengerService passengerService)
        {
            _userService = userService;
            _passengerService= passengerService;
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
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return BadRequest("متاسفانه خطای سیستمی رخ داده");
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
                Console.WriteLine(ex);
                return BadRequest("متاسفانه خطای سیستمی رخ داده");
            }
        }

        [HttpPut("CheckPassengerDocument/{documentId}/{isConfirmed}")]
        public async Task<IActionResult> CheckPassengerDocument(Guid documentId, bool isConfirmed)
        {
            try
            {
                await _passengerService.CheckPassengerDocument(documentId, isConfirmed);
                return OkResult(isConfirmed ? "مدرک کاربر با موفقیت تایید شد." : "مدرک کاربر رد شد.");
            }
            catch (ManagedException e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return BadRequest("متاسفانه خطای سیستمی رخ داده");
            }
        }

        [HttpPut("AssignUserType")]
        public async Task<IActionResult> AssignUserType(AssignUserTypeCommand command)
        {
            try
            {
                command.Validate();

                await _userService.AssignUserType(command);
                return OkResult("نوع حساب کاربر با موفقیت به کاربر انتصاب داده شد.");
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
                return BadRequest("متاسفانه خطای سیستمی رخ داده");
            }
        }

        [HttpPut("UpdateUser")]
        public async Task<IActionResult> UpdateUser(UpdateUserCommand command)
        {
            try
            {
                command.Validate();

                await _userService.Update(command);
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
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return BadRequest("متاسفانه خطای سیستمی رخ داده");
            }
        }
    }
}
