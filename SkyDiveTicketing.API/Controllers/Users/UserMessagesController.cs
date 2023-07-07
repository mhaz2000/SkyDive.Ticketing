using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SkyDiveTicketing.API.Base;
using SkyDiveTicketing.Application.Base;
using SkyDiveTicketing.Application.Services.UserMessageServices;

namespace SkyDiveTicketing.API.Controllers.Users
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserMessagesController : ApiControllerBase
    {
        private readonly IUserMessageService _userMessageService;

        public UserMessagesController(IUserMessageService userMessageService)
        {
            _userMessageService = userMessageService;
        }

        [HttpGet]
        public async Task<IActionResult> Get(Guid? userId, [FromQuery] PageQuery pageQuery)
        {
            try
            {
                var messages = await _userMessageService.GetUserMessages(userId ?? UserId);
                return Ok(new
                {
                    Message = "لیست پیام های کاربر",
                    Content = messages.ToPagingAndSorting(pageQuery),
                    Total = messages.Count(),
                    NotVisited = messages.Where(c=> !c.Visited).Count()
                });
            }
            catch (ManagedException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            try
            {
                var message = await _userMessageService.GetUserMessage(id);
                return OkResult("پیام کاربر", message);
            }
            catch (ManagedException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> MessageHasBeenSeen(Guid id)
        {
            try
            {
                await _userMessageService.MessageHasBeenSeen(id);
                return OkResult("پیام توسط کاربر مشاهده شد.");
            }
            catch (ManagedException e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
