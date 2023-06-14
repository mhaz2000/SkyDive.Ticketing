using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkyDiveTicketing.API.Base;
using SkyDiveTicketing.Application.Base;
using SkyDiveTicketing.Application.Commands.UserCommands;
using SkyDiveTicketing.Application.Services.UserTypeServices;

namespace SkyDiveTicketing.API.Controllers.Users
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class UserTypesController : ApiControllerBase
    {
        private readonly IUserTypeService _userTypeService;

        public UserTypesController(IUserTypeService userTypeService)
        {
            _userTypeService = userTypeService;
        }

        [HttpPost]
        public async Task<IActionResult> Create(UserTypeCommand command)
        {
            try
            {
                command.Validate();

                await _userTypeService.Create(command);
                return OkResult("نوع کاربری جدید با موفقیت ثبت شد.");
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

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UserTypeCommand command)
        {
            try
            {
                command.Validate();

                await _userTypeService.Update(command, id);
                return OkResult("نوع کاربری با موفقیت ویرایش شد.");
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

        [HttpGet]
        public IActionResult Get(string search, PageQuery page)
        {
            try
            {
                var userTypes = _userTypeService.GetAllTypes(search);
                return OkResult("اطلاعات انواع کاربر.", userTypes.ToPagingAndSorting(page), userTypes.Count());
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

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            try
            {
                var flightLoad = await _userTypeService.GetUserType(id);
                return OkResult("اطلاعات نوع کاربر.", flightLoad);
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

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await _userTypeService.Remove(id);
                return OkResult("اطلاعات لود پرواز با موفقیت حذف شد.");
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
