using SkyDiveTicketing.API.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace SkyDiveTicketing.API.Base
{
    [Authorize]
    [ApiController]
    public class ApiControllerBase : ControllerBase
    {

        protected string AccessToken => Request.GetAccessToken();

        protected virtual string UserName => ClaimHelper.GetClaim<string>(this.AccessToken, ClaimTypes.GivenName);
        protected virtual Guid UserId => ClaimHelper.GetClaim<Guid>(this.AccessToken, "id");

        protected ApiControllerBase()
        {
        }

        #region responses 
        [NonAction]
        public BadRequestObjectResult BadRequest(string message)
        {
            return BadRequest(new { Message = message });
        }

        [NonAction]
        public NotFoundObjectResult NotFound(string message)
        {
            return NotFound(new { Message = message });
        }

        [NonAction]
        protected virtual IActionResult OkResult()
        {
            return this.OkResult("", null);
        }

        [NonAction]
        protected virtual IActionResult OkResult(string message)
        {
            return this.OkResult(message, null);
        }

        [NonAction]
        protected virtual IActionResult OkContentResult(object content)
        {
            return this.OkResult("", content);
        }

        [NonAction]
        protected virtual IActionResult OkResult(string message, object content)
        {
            return Ok(new ResponseMessage(message, content));
        }

        [NonAction]
        protected virtual IActionResult OkResult(string message, object content, int total)
        {
            return Ok(new ResponseMessage(message, content, total));
        }
        #endregion

        public class RequestConfig
        {
            public static RequestConfig Default => new RequestConfig();

            /// <inheritdoc />
            public RequestConfig(bool returnContentOnly = false)
            {
                ReturnContentOnly = returnContentOnly;
            }

            public bool ReturnContentOnly { get; set; }

            public bool ReturnActionOutputOnly { get; set; }
        }
    }
}
